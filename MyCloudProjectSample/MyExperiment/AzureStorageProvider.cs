using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyExperiment
{
    /// <summary>
    /// Azure-backed implementation of <see cref="IStorageProvider"/>.
    /// 
    /// Responsibilities:
    /// - Read experiment requests from Azure Storage Queues.
    /// - Upload result artifacts (files) to Azure Blob Storage.
    /// - Persist experiment metadata to Azure Table Storage.
    /// - Commit (delete) processed queue messages.
    /// 
    /// Configuration (from MyConfig):
    /// - StorageConnectionString : Azure Storage account connection string.
    /// - Queue                   : Name of the queue for experiment requests.
    /// - ResultContainer        : Blob container name to store experiment outputs.
    /// - ResultTable            : Table name to store experiment results metadata.
    /// - GroupId                : Default PartitionKey for Table rows.
    /// </summary>
    public class AzureStorageProvider : IStorageProvider
    {
        private readonly MyConfig _config;
        private readonly ILogger? logger;

        /// <summary>
        /// Binds the provided configuration section to <see cref="MyConfig"/>.
        /// </summary>
        public AzureStorageProvider(IConfigurationSection configSection, ILogger? log = null)
        {
            _config = new MyConfig();
            configSection.Bind(_config);
            logger = log;
        }

        /// <summary>
        /// Deletes a processed queue message to prevent reprocessing.
        /// Uses MessageId + PopReceipt taken from <paramref name="request"/>.
        /// </summary>
        public async Task CommitRequestAsync(IExerimentRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                var queueClient = new QueueClient(_config.StorageConnectionString, _config.Queue);

                // Delete the message using identifiers captured during Receive
                await queueClient.DeleteMessageAsync(request.MessageId, request.MessageReceipt);
                Console.WriteLine($"Message with ID {request.MessageId} deleted successfully.");
            }
            catch (ArgumentNullException ex)
            {
                Console.Error.WriteLine($"Argument null exception: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"Argument exception: {ex.Message}");
            }
            catch (RequestFailedException ex)
            {
                Console.Error.WriteLine($"Failed to delete message: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Polls the configured queue for experiment requests and returns
        /// the first successfully deserialized message.
        /// Returns <c>null</c> when canceled or no messages are available.
        /// </summary>
        /// <remarks>
        /// Caller should eventually invoke <see cref="CommitRequestAsync"/> to delete the message.
        /// </remarks>
        public async Task<IExerimentRequest?> ReceiveExperimentRequestAsync(CancellationToken token)
        {
            var queueClient = new QueueClient(_config.StorageConnectionString, _config.Queue);

            while (!token.IsCancellationRequested)
            {
                // Retrieve a batch (default up to 32). You can pass maxMessages/visibilityTimeout if needed.
                QueueMessage[] messages = await queueClient.ReceiveMessagesAsync();

                if (messages != null && messages.Length > 0)
                {
                    foreach (var message in messages)
                    {
                        try
                        {
                            // NOTE: message.Body is a BinaryData; ToString() yields the UTF-8 JSON string.
                            string msgTxt = message.Body.ToString();
                            await Console.Out.WriteLineAsync(msgTxt);

                            // Deserialize into your request DTO
                            var request = JsonSerializer.Deserialize<ExerimentRequestMessage>(msgTxt);
                            if (request != null)
                            {
                                // Attach queue identifiers needed for DeleteMessage
                                request.MessageId = message.MessageId;
                                request.MessageReceipt = message.PopReceipt;
                                return request;
                            }
                        }
                        catch (JsonException ex)
                        {
                            logger?.LogError("{Time} - JSON deserialization error: {Error}", DateTime.Now, ex.Message);
                        }
                        catch (Exception ex)
                        {
                            logger?.LogError("{Time} - Error while processing a queue message: {Error}", DateTime.Now, ex.Message);
                        }
                    }
                }

                // Backoff before the next poll when queue is empty
                await Task.Delay(1000, token);
            }

            return null;
        }

        /// <summary>
        /// Inserts a new experiment result metadata row into Azure Table Storage.
        /// Uses a unique RowKey per run to ensure rows are appended (not overwritten).
        /// </summary>
        /// <remarks>
        /// RowKey strategy:
        /// - baseKey = ExperimentResult.RowKey OR ExperimentId OR "exp"
        /// - uniquifier = MessageId OR UTC timestamp (yyyyMMddHHmmssfff)
        /// - RowKey = $"{baseKey}:{uniquifier}"
        /// </remarks>
        public async Task UploadExperimentResult(IExperimentResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            var tableClient = new TableClient(_config.StorageConnectionString, _config.ResultTable);
            await tableClient.CreateIfNotExistsAsync();

            // Build a unique RowKey for append-only semantics
            string baseKey =
                (result as ExperimentResult)?.RowKey
                ?? result.ExperimentId
                ?? "exp";

            string uniquifier =
                (result as ExperimentResult)?.MessageId
                ?? DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            string rowKey = $"{baseKey}:{uniquifier}";

            string partitionKey =
                (result as ExperimentResult)?.PartitionKey
                ?? _config.GroupId
                ?? "default";

            var entity = new TableEntity(partitionKey, rowKey)
            {
                // Interface members
                ["ExperimentId"] = result.ExperimentId ?? string.Empty,
                ["StartTimeUtc"] = result.StartTimeUtc.HasValue
                    ? new DateTimeOffset(result.StartTimeUtc.Value, TimeSpan.Zero)
                    : (DateTimeOffset?)null,
                ["EndTimeUtc"] = result.EndTimeUtc.HasValue
                    ? new DateTimeOffset(result.EndTimeUtc.Value, TimeSpan.Zero)
                    : (DateTimeOffset?)null,
                ["DurationSec"] = result.DurationSec,
            };

            // Add concrete-only properties if available
            if (result is ExperimentResult r)
            {
                // Human-readable metadata
                entity["Name"] = r.Name ?? string.Empty;
                entity["Description"] = r.Description ?? string.Empty;

                // Queue bookkeeping
                entity["MessageId"] = r.MessageId ?? string.Empty;
                entity["MessageReceipt"] = r.MessageReceipt ?? string.Empty;

                // Parameters
                entity["W"] = r.W;
                entity["N"] = r.N;
                entity["Radius"] = r.Radius;
                entity["MinVal"] = r.MinVal;
                entity["MaxVal"] = r.MaxVal;
                entity["Periodic"] = r.Periodic;
                entity["ClipInput"] = r.ClipInput;
                entity["CellsPerColumn"] = r.CellsPerColumn;
                entity["MaxBoost"] = r.MaxBoost;
                entity["DutyCyclePeriod"] = r.DutyCyclePeriod;
                entity["MinPctOverlapDutyCycles"] = r.MinPctOverlapDutyCycles;
                entity["GlobalInhibition"] = r.GlobalInhibition;
                entity["NumActiveColumnsPerInhArea"] = r.NumActiveColumnsPerInhArea;
                entity["PotentialRadius"] = r.PotentialRadius;
                entity["NumColumns"] = r.NumColumns;
                entity["MinOctOverlapCycles"] = r.MinOctOverlapCycles;
                entity["LocalAreaDensity"] = r.LocalAreaDensity;
                entity["ActivationThreshold"] = r.ActivationThreshold;
                entity["MaxSynapsesPerSegment"] = r.MaxSynapsesPerSegment;
                entity["StimulusThreshold"] = r.StimulusThreshold;
                entity["InputBits"] = r.inputBits;

                entity["OutFolder"] = r.outFolder ?? string.Empty;
                entity["PermFolder"] = r.permFolder ?? string.Empty;
                entity["ProbFolder"] = r.probFolder ?? string.Empty;
            }

            // Insert-only (append). If PK+RK already exists, this throws 409 Conflict.
            await tableClient.AddEntityAsync(entity);
        }

        /// <summary>
        /// Uploads all subfolders under the running binary directory (bin)
        /// into Blob Storage under the prefix: {experimentName}/{folderName}/...
        /// Skips the "runtimes" folder for noise reduction.
        /// </summary>
        /// <remarks>
        /// Consider restricting which folders to upload if bin contains large/unrelated content.
        /// </remarks>
        public async Task UploadResultAsync(string experimentName, IExperimentResult result)
        {
            if (string.IsNullOrWhiteSpace(experimentName))
                throw new ArgumentException("Experiment name must be provided.", nameof(experimentName));

            var blobServiceClient = new BlobServiceClient(_config.StorageConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_config.ResultContainer);
            await containerClient.CreateIfNotExistsAsync();

            // Use the same writable base as the experiment
            string baseOut = Environment.GetEnvironmentVariable("OUTPUT_ROOT") ?? "/tmp/outputfile";
            Directory.CreateDirectory(baseOut);

            // Only upload the folders you produced
            var folders = new[]
            {
        "AllPermanenceValues",
        "AllProbabilities",
        "ThresholdValues",
        "RunRustructuringExperiment"
    };

            var r = result as ExperimentResult;

            foreach (var folderName in folders)
            {
                string folderPath = Path.Combine(baseOut, folderName);
                if (!Directory.Exists(folderPath))
                {
                    logger?.LogWarning("Result folder not found: {Folder}", folderPath);
                    continue;
                }

                // Prefix in blob storage: experimentName/folderName/...
                string blobPrefix = $"{experimentName}/{folderName}";
                await UploadFolderAsync(containerClient, folderPath, blobPrefix);

                // Build a friendly “folder URL” (really just a prefix)
                string folderUrl = $"{containerClient.Uri}/{Uri.EscapeDataString(experimentName)}/{Uri.EscapeDataString(folderName)}/";

                // Save onto result so UploadExperimentResult can persist to Table
                if (r != null)
                {
                    if (folderName == "RunRustructuringExperiment") r.outFolder = folderUrl;
                    if (folderName == "AllPermanenceValues") r.permFolder = folderUrl;
                    if (folderName == "AllProbabilities") r.probFolder = folderUrl;
                    // If you also want ThresholdValues in table, add a property and assign here.
                }

                logger?.LogInformation("Uploaded folder {FolderName} to {Url}", folderName, folderUrl);
            }
        }


        /// <summary>
        /// Recursively uploads all files under <paramref name="localFolderPath"/>
        /// to the given container using the provided blob prefix.
        /// </summary>
        private static async Task UploadFolderAsync(BlobContainerClient container, string localFolderPath, string blobPrefix)
        {
            // Enumerate all files recursively
            foreach (var filePath in Directory.EnumerateFiles(localFolderPath, "*", SearchOption.AllDirectories))
            {
                // Build a stable relative path, normalize to Unix-style separators for blob names
                string relativePath = Path.GetRelativePath(localFolderPath, filePath).Replace("\\", "/");
                string blobName = $"{blobPrefix}/{relativePath}";

                var blobClient = container.GetBlobClient(blobName);

                await using var stream = File.OpenRead(filePath);
                await blobClient.UploadAsync(stream, overwrite: true);
            }
        }
    }
}
