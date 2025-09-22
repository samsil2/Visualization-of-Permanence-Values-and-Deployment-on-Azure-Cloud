# Implementation of the Visualization of Permanence Values
## _Cloud Computing Project 2025_

# Introduction
This project investigates the Reconstruct() function within the Hierarchical Temporal Memory (HTM) framework’s NeoCortexAPI, with the objective of exploring its full capabilities. Serving as the inverse operation of the Spatial Pooler (SP), the Reconstruct() function plays a pivotal role in our study. Under the title "Visualization of Reconstructed Permanence Values," the research focuses on demonstrating how this function enables the reconstruction of input sequences processed by the Spatial Pooler. By employing both numerical data and images as inputs, the study seeks to elucidate the relationship between the original inputs and their reconstructed outputs within the HTM architecture. This work aims to advance the understanding and practical utilization of HTM technology.

# Objective of Cloud Computing Project
The objective of this study is to integrate cloud computing technologies to enhance the deployment and scalability of our research on the Reconstruct() function within the Hierarchical Temporal Memory (HTM) framework. By employing Docker containers in conjunction with Microsoft Azure cloud services, we aim to efficiently execute and manage the project in a cloud-based environment. This strategy is intended to streamline the execution of experiments while enabling scalable and adaptable analyses of the Reconstruct() function. Furthermore, the integration of these technologies is expected to improve the accessibility, performance, and flexibility of the developed visualization and analysis tools, thereby contributing to a more comprehensive understanding of HTM technology and its potential applications within cloud-based computational frameworks.

# Implementation Details of Software Engineering Project
For a better understanding of the implementation, running, and how it works, please have a look at the Software Engineering project:

- **[Repository Link](https://github.com/samsil2/neocortexapi_Samsil_Arefin)**: This is the main repository where you can find the source code and other related files.
- **[Readme](https://github.com/samsil2/neocortexapi_Samsil_Arefin/blob/master/source/MySEProject/Documentation/Readme.md)**: The README file contains detailed information about the project, including how to run and use it.

## Features
- Experiments for Numbers
- Reconstructions of Encoded Inputs
- Reconstructed permanence values of active and inactive colums
- Threshold values
- Heatmap Generation

## Implemented Feature for Cloud Computing
- AllPermanenceValues, AllProbabilities, ThresholdValues are exported as csv and stored in table and blob storage
- Generate heatmps and stored in blob storage.
- Setup queue messages
- Dockerize the project and Deployment on Azure Cloud.


## The Cloud Environment Workflow and Architecture of Processing Experiment
![Architecture](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/Architecture.jpg?)

*Fig. 1. Workflow and Architecture*

## Cloud Project Program Files Links:
- **[AzureStorageProvider.cs Link](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/MyExperiment/AzureStorageProvider.cs)**
- **[Experiment.cs Link](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/MyExperiment/Experiment.cs)**
- **[Program.cs Link](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/MyCloudProject/Program.cs)**

## Components for Running the Project
**Note:** The project is currently not active on Azure Cloud. However, you can re-launch it by following the steps outlined below and configuring the components specified in Azure Cloud.

## Key Components
| **Component**                                                                                  | **Name**             | **Description**                                                       |
|------------------------------------------------------------------------------------------------|----------------------|-----------------------------------------------------------------------|
| [Resource Group](https://portal.azure.com/#@msdndaenet.onmicrosoft.com/resource/subscriptions/d60f2036-12f5-499d-af22-ef3afc698896/resourceGroups/RG-SamsilArefin/overview) | RG-SamsilArefin          | ---                                                                   |
| [Container Registry](https://portal.azure.com/#@msdndaenet.onmicrosoft.com/resource/subscriptions/d60f2036-12f5-499d-af22-ef3afc698896/resourceGroups/RG-SamsilArefin/providers/Microsoft.ContainerRegistry/registries/contrainerregistrycc/overview) | contrainerregistrycc           | ---                                                                   |
| [Container Registry Server](contrainerregistrycc.azurecr.io)                                    |contrainerregistrycc.azurecr.io| ---                                                                   |
| [Repository](https://portal.azure.com/#@msdndaenet.onmicrosoft.com/resource/subscriptions/d60f2036-12f5-499d-af22-ef3afc698896/resourceGroups/RG-SamsilArefin/providers/Microsoft.ContainerRegistry/registries/contrainerregistrycc/repository) | mycloudproject:v2 |                                                 |
| [Container Instance](https://portal.azure.com/#@msdndaenet.onmicrosoft.com/resource/subscriptions/d60f2036-12f5-499d-af22-ef3afc698896/resourceGroups/RG-SamsilArefin/providers/Microsoft.ContainerInstance/containerGroups/samsilarefincontainerinstance/overview) | samsilarefincontainerinstance | Name of the container instance where experiment runs                  |
| [Storage Account](https://portal.azure.com/#@msdndaenet.onmicrosoft.com/resource/subscriptions/d60f2036-12f5-499d-af22-ef3afc698896/resourceGroups/RG-SamsilArefin/providers/Microsoft.Storage/storageAccounts/ccproject25samsilarefin/overview) | ccproject25samsilarefin          | ---                                                                   |
| [Queue Storage](https://portal.azure.com/#@msdndaenet.onmicrosoft.com/resource/subscriptions/d60f2036-12f5-499d-af22-ef3afc698896/resourceGroups/RG-SamsilArefin/providers/Microsoft.Storage/storageAccounts/ccproject25samsilarefin/storagebrowser) | trigger-queue     | Queue where trigger message is passed to initiate experiment           |
| [Blob Container](https://portal.azure.com/#@msdndaenet.onmicrosoft.com/resource/subscriptions/d60f2036-12f5-499d-af22-ef3afc698896/resourceGroups/RG-SamsilArefin/providers/Microsoft.Storage/storageAccounts/ccproject25samsilarefin/storagebrowser) | result-files      | Containing the output Images and csv genarated by the Experiment       |
| [Table Storage](https://portal.azure.com/#@msdndaenet.onmicrosoft.com/resource/subscriptions/d60f2036-12f5-499d-af22-ef3afc698896/resourceGroups/RG-SamsilArefin/providers/Microsoft.Storage/storageAccounts/ccproject25samsilarefin/storagebrowser) | results     | Table used to store results of the experiment                         |

## Processing the queue Message Experiment
~~~json
{
  "ExperimentId": "EXP-2025-001",
  "Name": "Reconstruction",
  "Description": "Null",
  "W": 15,
  "N": 200,
  "Radius": -1.0,
  "MinVal": 0.0,
  "MaxVal": 100,
  "Periodic": false,
  "ClipInput": false,
  "CellsPerColumn": 10,
  "MaxBoost": 5.0,
  "DutyCyclePeriod": 100,
  "MinPctOverlapDutyCycles": 1.0,
  "GlobalInhibition": false,
  "InputBits": 200,
  "NumColumns": 1024,
  "MinPctOverlapCycles": 1.0,
  "LocalAreaDensity": -1,
  "ActivationThreshold": 10,
  "StimulusThreshold": 10
}

~~~

## Description of the Queue Message

| **Field**                  | **Description**                                                                                  | **Example Value**       |
|----------------------------|--------------------------------------------------------------------------------------------------|-------------------------|
| `ExperimentId`             | Unique identifier for the experiment instance.                                                  | `EXP-2025-001`          |
| `Name`                     | Short name or title of the experiment.                                                          | `Reconstruction`        |
| `Description`              | Optional text describing the experiment.                                                        | `Null`                  |
| `W`                        | Number of active bits (sparsity) in the encoded input vector.                                    | `15`                    |
| `N`                        | Total number of bits in the input encoding.                                                      | `200`                   |
| `Radius`                   | Inhibition radius for the Spatial Pooler (SP). `-1.0` means automatic calculation.               | `-1.0`                  |
| `MinVal`                   | Minimum input value for normalization.                                                           | `0.0`                   |
| `MaxVal`                   | Maximum input value for normalization.                                                           | `100`                   |
| `Periodic`                 | Whether the input space is periodic (wraps around).                                              | `false`                 |
| `ClipInput`                | Whether to clip inputs outside the `[MinVal, MaxVal]` range.                                     | `false`                 |
| `CellsPerColumn`           | Number of cells per column in the HTM region.                                                     | `10`                    |
| `MaxBoost`                 | Maximum boosting factor to encourage column participation.                                       | `5.0`                    |
| `DutyCyclePeriod`          | Number of iterations over which activity duty cycles are calculated.                             | `100`                   |
| `MinPctOverlapDutyCycles`  | Minimum allowed active duty cycle as a percentage of maximum in a neighborhood.                  | `1.0`                   |
| `GlobalInhibition`         | Whether inhibition is applied globally across the entire region.                                 | `false`                 |
| `InputBits`                | Number of bits representing the input signal.                                                    | `200`                   |
| `NumColumns`               | Total number of columns in the HTM Spatial Pooler.                                               | `1024`                  |
| `MinPctOverlapCycles`      | Minimum number of overlapping active bits required to consider a column for learning.            | `1.0`                   |
| `LocalAreaDensity`         | Fraction of columns to be active within a local inhibition area (`-1` = automatic).               | `-1`                    |
| `ActivationThreshold`      | Minimum number of active connected synapses for a column to become active.                       | `10`                    |
| `StimulusThreshold`        | Minimum number of connected synapses for a column to be considered during inhibition.            | `10`                    |

### Implementation Details

## Method Documentation: `ReceiveExperimentRequestAsync`

~~~csharp
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
~~~

### Purpose
The `ReceiveExperimentRequestAsync` method is designed to handle receiving and processing messages from an Azure queue. The messages are expected to be JSON formatted, representing experiment requests. This method performs the following tasks:
- Establishes a connection to the Azure Storage Queue using the configured connection string and queue name.
- Continuously polls the queue for new messages until the cancellation token is triggered.
- Retrieves and deserializes JSON messages into `ExerimentRequestMessage` objects.
- Attaches `MessageId` and `PopReceipt` to each request for later deletion after processing.
- Implements error handling for JSON parsing errors and general processing exceptions.
- Uses a delay when the queue is empty to reduce unnecessary polling frequency.

## Method Documentation: `UploadExperimentResult`
~~~csharp
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


~~~

### Purpose
- Creates a connection to the configured Azure Table Storage and ensures the result table exists.
- Generates a **unique RowKey** per experiment run to prevent overwriting existing records (append-only strategy).
- Uses `PartitionKey`(`prof`) and `RowKey` to organize and uniquely identify experiment results.
- Maps both **common fields** (from `IExperimentResult`) and **specific fields** (from `ExperimentResult`) into an `Azure.Data.Tables.TableEntity`.
- Persists experiment metadata, parameters, and output folder references for future retrieval and analysis.
- Inserts the entity using **insert-only** semantics, throwing an error if a duplicate key is detected.

## Method Documentation: `UploadResultAsync`
~~~csharp
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
~~~

### Purpose
Uploads experiment output folders (e.g., permanence values, probabilities, threshold data) from the local output directory to Azure Blob Storage under a structured `{experimentName}/{folderName}` path. Automatically skips irrelevant folders (e.g., `runtimes`) and stores generated folder URLs back into the experiment result for later reference in metadata. Ensures output data is organized, accessible, and linked with experiment records in Azure Table Storage.

## Method Documentation: `UploadFolderAsync`
~~~csharp
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
~~~

### Purpose
Recursively uploads all files from a specified local folder to an Azure Blob Storage container under a given blob prefix.  
Preserves relative folder structure in blob names and ensures files are overwritten if they already exist.

## Method Documentation: `CommitRequestAsync`
~~~csharp
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
~~~

### Purpose
Deletes a processed Azure Queue message using its `MessageId` and `PopReceipt` to prevent reprocessing.  
Includes error handling for null arguments, invalid parameters, and Azure request failures.

## Method Documentation: `RunAsync`

~~~csharp
 public Task<IExperimentResult> RunAsync(IExerimentRequest request)
        {
            // Create and map result object with metadata and parameters from the request
            var res = DataEntryPoint(request);

            res.StartTimeUtc = DateTime.UtcNow;
            Console.WriteLine("Experiment Started");

            // Measure execution time
            Console.WriteLine("Experiment running ...");
            var sw = System.Diagnostics.Stopwatch.StartNew();

            // Core ML logic execution
            var experiment = new SpatialPatternLearning();
            experiment.Run(request); // If async is supported: await experiment.RunAsync(request)

            sw.Stop();
            Console.WriteLine("Experiment Completed");

            // Populate result metadata
            res.EndTimeUtc = DateTime.UtcNow;
            res.DurationSec = (long)sw.Elapsed.TotalSeconds;

            // Return completed result
            return Task.FromResult<IExperimentResult>(res);
        }
~~~

### Purpose
Executes the experiment workflow using parameters from the request, including initializing metadata, running the core HTM logic, and measuring execution time.  
Returns an `IExperimentResult` containing both the experiment outputs and associated metadata.

## Method Documentation: `DataEntryPoint`

~~~csharp
private ExperimentResult DataEntryPoint(IExerimentRequest request)
        {
            return new ExperimentResult(
                this.config.GroupId,
                request.ExperimentId ?? Guid.NewGuid().ToString())
            {
                // Metadata
                ExperimentId = request.ExperimentId,
                Name = request.Name,
                Description = request.Description,
                MessageId = request.MessageId,
                MessageReceipt = request.MessageReceipt,

                // Parameters
                W = request.W,
                N = request.N,
                Radius = request.Radius,
                MinVal = request.MinVal,
                MaxVal = request.MaxVal,
                Periodic = request.Periodic,
                ClipInput = request.ClipInput,
                CellsPerColumn = request.CellsPerColumn,
                MaxBoost = request.MaxBoost,
                DutyCyclePeriod = request.DutyCyclePeriod,
                MinPctOverlapDutyCycles = request.MinPctOverlapDutyCycles,
                GlobalInhibition = request.GlobalInhibition,
                NumActiveColumnsPerInhArea = request.NumActiveColumnsPerInhArea,
                PotentialRadius = request.PotentialRadius,
                NumColumns = request.NumColumns,
                MinOctOverlapCycles = request.MinOctOverlapCycles,
                LocalAreaDensity = request.LocalAreaDensity,
                ActivationThreshold = request.ActivationThreshold,
                MaxSynapsesPerSegment = request.MaxSynapsesPerSegment,
                Random = request.Random,
                StimulusThreshold = request.StimulusThreshold,
                inputBits = request.inputBits
            };
        }
~~~

### Purpose
Creates and populates an `ExperimentResult` instance with values from the provided experiment request.  
Includes both configuration parameters and metadata, producing a ready-to-use object for further processing and storage.

## `MyConfig`

~~~csharp
 public class MyConfig
    {
        public string StorageConnectionString { get; set; }

        /// <summary>
        /// The name of the container in the blob storage, where training files are located.
        /// </summary>
        public string TrainingContainer { get; set; }

        /// <summary>
        /// The name of the container in the blob storage, where result files will be uploaded.
        /// </summary>
        public string ResultContainer { get; set; }

        /// <summary>
        /// The name of the table where result information will be uploaded.
        /// </summary>
        public string ResultTable { get; set; }

        /// <summary>
        /// The name of the queue used to trigger the computation.
        /// </summary>
        public string Queue{ get; set; }

        /// <summary>
        /// Set here the name of your group.
        /// </summary>
        public string GroupId { get; set; }

    }
~~~

### Purpose
Defines the configuration settings required for running the experiment workflow.  
Stores connection details for Azure services, including Blob Storage, Table Storage, and Queue Storage.  
Specifies the container names for training and result data, the table name for result metadata, and the queue name for triggering computations.  
Includes a `GroupId` to logically group experiments under a specific team or project.  
Centralizes configuration values to simplify deployment, management, and environment changes.

- **GroupId**:
  - Identifier for the team or project group.

- **StorageConnectionString**: `your-connection-string-here`
  - Connection string for accessing Azure Storage. Replace `your-connection-string-here` with your actual connection string.

- **TrainingContainer**: `training-files`
  - Name of the Azure Storage container where training files are stored.

- **ResultContainer**: `result-files`
  - Name of the Azure Storage container where result files are stored.

- **ResultTable**: `results`
  - Name of the Azure Table where experiment results are stored.

- **Queue**: `trigger-queue`
  - Name of the Azure Queue used for triggering experiment requests.

## `Main`
~~~csharp
 private static async Task Main(string[] args)
        {

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            Console.Error.WriteLine("[FATAL] " + e.ExceptionObject);

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Console.Error.WriteLine("[TASK] " + e.Exception);
                e.SetObserved();
            };
            // Support graceful shutdown (Ctrl+C).
            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;      // prevent abrupt process kill
                cts.Cancel();         // request cooperative cancellation
            };

            Console.WriteLine($"Started experiment: {_projectName}");

            // 1) Configuration
            var cfgRoot = Common.InitHelpers.InitConfiguration(args);
            var cfgSec = cfgRoot.GetSection("MyConfig");

            // 2) Logging
            var logFactory = InitHelpers.InitLogging(cfgRoot);
            var logger = logFactory.CreateLogger("Train.Console");
            logger?.LogInformation("{Time} - Started experiment: {Project}", DateTime.Now, _projectName);

            // 3) Dependencies (storage provider + experiment implementation)
            IStorageProvider storageProvider = new AzureStorageProvider(cfgSec);
            IExperiment experiment = new Experiment(cfgSec, storageProvider, logger /* additional DI as needed */);

            // MAIN LOOP: keep pulling messages until canceled.
            while (!cts.Token.IsCancellationRequested)
            {
                // ─────────────────────────────────────────────────────────────────────────
                // Step 3: Receive request from the queue
                // ─────────────────────────────────────────────────────────────────────────
                IExerimentRequest request = await storageProvider.ReceiveExperimentRequestAsync(cts.Token)
                                                                .ConfigureAwait(false);

                if (request == null)
                {
                    // No message available; back off briefly and continue polling
                    await Task.Delay(500, cts.Token).ConfigureAwait(false);
                    logger?.LogTrace("Queue empty...");
                    continue;
                }

                // We have a request; process it end-to-end
                try
                {
                    logger?.LogInformation("Received request ExperimentId={ExperimentId}, MessageId={MessageId}",
                        request.ExperimentId, request.MessageId);

                    // ─────────────────────────────────────────────────────────────────────
                    // Steps 4–5: Execute experiment (your SE project code runs here)
                    // ─────────────────────────────────────────────────────────────────────
                    IExperimentResult result = await experiment.RunAsync(request).ConfigureAwait(false);
                    logger?.LogInformation("Experiment finished: ExperimentId={ExperimentId} Duration={Duration}s",
                        result.ExperimentId, result.DurationSec);

                    // ─────────────────────────────────────────────────────────────────────
                    // Step 5a: Upload blobs (all result folders under bin/)
                    // Groups under container/result-files/{experimentName}/...
                    // ─────────────────────────────────────────────────────────────────────
                    await storageProvider.UploadResultAsync("outputfile", result).ConfigureAwait(false);
                    Console.WriteLine("Files Uploaded");
                    logger?.LogInformation("Blob upload complete for ExperimentId={ExperimentId}", result.ExperimentId);

                    // ─────────────────────────────────────────────────────────────────────
                    // Step 5b: Persist summary row into Azure Table Storage
                    // RowKey strategy uses ExperimentId + uniquifier (see provider)
                    // ─────────────────────────────────────────────────────────────────────
                    await storageProvider.UploadExperimentResult(result).ConfigureAwait(false);
                    Console.WriteLine("Tables updated");
                    logger?.LogInformation("Table row upserted for ExperimentId={ExperimentId}", result.ExperimentId);

                    // ─────────────────────────────────────────────────────────────────────
                    // Step 6: Commit the request (e.g., delete queue message)
                    // Implementation is inside your provider.
                    // ─────────────────────────────────────────────────────────────────────
                    await storageProvider.CommitRequestAsync(request).ConfigureAwait(false);
                    logger?.LogInformation("Request committed: MessageId={MessageId}", request.MessageId);
                }
                catch (OperationCanceledException) when (cts.IsCancellationRequested)
                {
                    // Graceful shutdown requested
                    logger?.LogWarning("Cancellation requested. Exiting main loop...");
                    break;
                }
                catch (Exception ex)
                {
                    // IMPORTANT: If an exception occurs after you pulled the message but before
                    // you commit/delete it, the message will become visible again after the
                    // queue visibility timeout. Make sure CommitRequestAsync handles poison
                    // message escalation if needed.
                    logger?.LogError(ex, "Unhandled exception while processing an experiment request.");
                    // Optionally: move to a poison queue or mark in Table storage.
                }
            }

            logger?.LogInformation("{Time} - Experiment exit: {Project}", DateTime.Now, _projectName);
        }
~~~

### Overview
The `Main` method is an asynchronous entry point and serves as the application entry point for running the cloud experiment pipeline.  
Initializes configuration, logging, and dependencies, then continuously processes experiment requests from an Azure Queue.  
Runs experiments, uploads results to Blob Storage, stores metadata in Table Storage, and commits processed queue messages until manually stopped.

### Process Flow
1. **Set Up Global Exception Handling**  
   - Capture unhandled exceptions (`UnhandledException`) and unobserved task exceptions (`UnobservedTaskException`) for logging.

2. **Enable Graceful Shutdown**  
   - Create a `CancellationTokenSource` and hook into `Console.CancelKeyPress` to allow clean termination via Ctrl+C.

3. **Load Configuration**  
   - Use `InitConfiguration(args)` to load settings from config files or command-line arguments.  
   - Extract the `MyConfig` section containing Azure storage and project parameters.

4. **Initialize Logging**  
   - Set up a logger using `InitLogging()` for tracking program activity and errors.

5. **Initialize Dependencies**  
   - Create an `AzureStorageProvider` for interacting with Azure Queue, Blob, and Table Storage.  
   - Instantiate `Experiment` with storage and logger dependencies.

6. **Main Loop – Continuous Processing**  
   - **Receive Experiment Request:**  
     Poll the Azure Queue with `ReceiveExperimentRequestAsync()`.  
     If no message is available, log trace and wait briefly before retrying.
   
   - **Log and Process Request:**  
     Log the experiment request details (`ExperimentId`, `MessageId`).

   - **Run Experiment:**  
     Execute `RunAsync(request)` to run the HTM experiment and measure execution time.

   - **Upload Results:**  
     Call `UploadResultAsync("outputfile", result)` to send output folders to Azure Blob Storage.

   - **Save Metadata:**  
     Use `UploadExperimentResult(result)` to store experiment metadata in Azure Table Storage.

   - **Commit Message:**  
     Call `CommitRequestAsync(request)` to delete the processed queue message and prevent reprocessing.

7. **Error Handling During Processing**  
   - Catch `OperationCanceledException` for clean shutdown.  
   - Catch general exceptions, log them, and allow Azure Queue’s visibility timeout to handle retries or poison message escalation.

8. **Graceful Exit**  
   - On cancellation, log the shutdown event and terminate the main loop cleanly.
  
# Configuration Documentation

## Logging

- **IncludeScopes**: `false`
  - Determines whether to include scopes in log messages.
- **LogLevel**:
  - **Default**: `Debug`
    - Sets the default logging level to `Debug`.
  - **System**: `Information`
    - Logs system-related messages at the `Information` level.
  - **Microsoft**: `Information`
    - Logs Microsoft-related messages at the `Information` level.

## SE Project Method: `ThresholdProbabilities`

~~~csharp
public static double[] ThresholdProbabilities(IEnumerable<double> values, double threshold)
        {
            // Returning null for null input values
            if (values == null)
            {
                return null;
            }

            // Get the length of the values enumerable
            int length = values.Count();

            // Create a one-dimensional array to hold thresholded values
            double[] result = new double[length];

            int index = 0;
            foreach (var numericValue in values)
            {
                // Determine the thresholded value based on the threshold
                double thresholdedValue = (numericValue >= threshold) ? 1.0 : 0.0;

                // Assign the thresholded value to the result array
                result[index++] = thresholdedValue;
            }

            return result;
        }
~~~

### Purpose
Converts a collection of numeric probability values into a binary array based on a specified threshold.  
Values greater than or equal to the threshold are set to `1.0`, and values below it are set to `0.0`.

## SE Project Method: ` DrawHeatmaps`

~~~chsarp
public static void DrawHeatmaps(List<double[,]> twoDimArrays, string filePath,
                                 int bmpWidth = 1024, int bmpHeight = 1024,
                                 decimal redStart = 200, decimal yellowMiddle = 127, decimal greenStart = 20)
        {
            int widthOfAll = 0, heightOfAll = 0;

            foreach (var arr in twoDimArrays)
            {
                widthOfAll += arr.GetLength(0);
                heightOfAll += arr.GetLength(1);
            }

            if (widthOfAll > bmpWidth || heightOfAll > bmpHeight)
                throw new ArgumentException("Size of all included arrays must be less than specified 'bmpWidth' and 'bmpHeight'");

            using (System.Drawing.Bitmap myBitmap = new System.Drawing.Bitmap(bmpWidth, bmpHeight))
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(myBitmap))
            {
                int k = 0;

                for (int n = 0; n < twoDimArrays.Count; n++)
                {
                    var arr = twoDimArrays[n];

                    int w = arr.GetLength(0);
                    int h = arr.GetLength(1);

                    var scale = Math.Max(1, ((bmpWidth) / twoDimArrays.Count) / (w + 1)); // +1 is for offset between pictures in X dim.

                    for (int Xcount = 0; Xcount < w; Xcount++)
                    {
                        for (int Ycount = 0; Ycount < h; Ycount++)
                        {
                            for (int padX = 0; padX < scale; padX++)
                            {
                                for (int padY = 0; padY < scale; padY++)
                                {
                                    myBitmap.SetPixel(n * (bmpWidth / twoDimArrays.Count) + Xcount * scale + padX, Ycount * scale + padY, GetColor(redStart, yellowMiddle, greenStart, (Decimal)arr[Xcount, Ycount]));
                                    k++;
                                }
                            }
                        }
                    }
                }

                // Draw text on the bitmap
                DrawLegends(g, bmpWidth, bmpHeight, redStart, yellowMiddle, greenStart);

                // Save the heatmap to file
                myBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

            }
        }
~~~

### Purpose
- Generates heatmap images from a list of 2D numeric arrays and saves them as PNG files.  
- Maps numeric values to colors (green, yellow, red) based on `greenStart`, `yellowMiddle`, and `redStart` thresholds.  
- Scales and positions each array proportionally within the specified bitmap dimensions (`bmpWidth`, `bmpHeight`).  
- Adds legends to the bitmap to indicate color meaning for the heatmap values.  
- Throws an error if the combined size of arrays exceeds the specified bitmap size.

## SE Project Method: ` RunRustructuringExperiment`
~~~csharp
        private static readonly string OutputRoot =
        Environment.GetEnvironmentVariable("OUTPUT_ROOT") ?? "/tmp/outputfile";
        //<summary>
        // RunRustructuringExperiment Method:
        // This method conducts an experiment to reconstruct input sequences using the Hierarchical Temporal Memory (HTM) algorithm.
        // It takes three parameters: sp (SpatialPooler), encoder (EncoderBase), and inputValues (List<double>).
        // The method iterates through each input value, encoding it into an int[] array using the encoder.Encode() method.
        // It then computes the SDR representation of the encoded input using the sp.Compute() method.
        // The reconstructed probabilities of each column being active are obtained using the sp.Reconstruct() method.
        // Permanence values are extracted from the reconstructed probabilities and thresholded to identify active columns.
        // Heatmaps are generated to visually represent the thresholded permanence values using the NeoCortexUtils.DrawHeatmaps()     method.
        // The resulting heatmap images are saved in an output folder with filenames based on the corresponding input value.
        // Finally, the method waits for a key press before exiting.
        private void RunRustructuringExperiment(SpatialPooler sp, EncoderBase encoder, List<double> inputValues)
        {
            // Prepare output folders under a writable base
            Directory.CreateDirectory(OutputRoot);

            string permFolder = Path.Combine(OutputRoot, "AllPermanenceValues");
            string probFolder = Path.Combine(OutputRoot, "AllProbabilities");
            string threshFolder = Path.Combine(OutputRoot, "ThresholdValues");
            string heatmapFolder = Path.Combine(OutputRoot, nameof(RunRustructuringExperiment));

            Directory.CreateDirectory(permFolder);
            Directory.CreateDirectory(probFolder);
            Directory.CreateDirectory(threshFolder);
            Directory.CreateDirectory(heatmapFolder);

            foreach (var input in inputValues)
            {
                var inpSdr = encoder.Encode(input);
                var actCols = sp.Compute(inpSdr, false);
                var probabilities = sp.Reconstruct(actCols);

                // Build full permanence map
                var allPermanenceValues = new Dictionary<int, double>();
                foreach (var kvp in probabilities)
                    allPermanenceValues[kvp.Key] = kvp.Value;

                // Ensure all columns are present
                for (int col = 0; col < 200; col++)
                    if (!allPermanenceValues.ContainsKey(col))
                        allPermanenceValues[col] = 0.0;

                var orderedPermanences = allPermanenceValues
                                            .OrderBy(kvp => kvp.Key)
                                            .Select(kvp => kvp.Value)
                                            .ToList();

                // CSVs
                var permCsvPath = Path.Combine(permFolder, $"{input}_permanence.csv");
                using (var w = new StreamWriter(permCsvPath))
                {
                    w.WriteLine("Column,Permanence");
                    for (int col = 0; col < orderedPermanences.Count; col++)
                        w.WriteLine($"{col},{orderedPermanences[col]}");
                }

                var probCsvPath = Path.Combine(probFolder, $"{input}_probabilities.csv");
                using (var w = new StreamWriter(probCsvPath))
                {
                    w.WriteLine("Column,Probability");
                    foreach (var kvp in allPermanenceValues.OrderBy(k => k.Key))
                        w.WriteLine($"{kvp.Key},{kvp.Value}");
                }

                var thresholdValues = Helpers.ThresholdProbabilities(orderedPermanences, 0.52);
                var threshCsvPath = Path.Combine(threshFolder, $"{input}_thresholds.csv");
                using (var w = new StreamWriter(threshCsvPath))
                {
                    w.WriteLine("Column,ThresholdValue");
                    for (int col = 0; col < thresholdValues.Count(); col++)
                        w.WriteLine($"{col},{thresholdValues[col]}");
                }

                // Heatmap
                var colDims = new[] { 64, 64 };
                var arrays = new List<double[,]>
        {
            ArrayUtils.Make2DArray<double>(thresholdValues.ToList().ToArray(), colDims[0], colDims[1])
        };

                var outputImage = Path.Combine(heatmapFolder, $"{input}_threshold_heatmap.png");
                NeoCortexUtils.DrawHeatmaps(arrays, outputImage, 1024, 1024, 200, 127, 20);
            }
        }

~~~

### 🔄 Flow Process of `RunRustructuringExperiment`
1. **Prepare Output Folders**  
   - Define a writable base directory (`OUTPUT_ROOT` or `/tmp/outputfile`).  
   - Create dedicated subfolders:
     - `AllPermanenceValues` → CSVs for reconstructed permanence values.  
     - `AllProbabilities` → CSVs for reconstructed probability values.  
     - `ThresholdValues` → CSVs for thresholded permanence values.  
     - `RunRustructuringExperiment` → Generated heatmap images.

2. **Process Each Input Value**  
   - **Encode Input:**  
     Use `encoder.Encode(input)` to convert the numeric input into an SDR (Sparse Distributed Representation).  
   - **Run Spatial Pooler:**  
     Call `sp.Compute()` to process the SDR and produce active columns.  
   - **Reconstruct Permanences:**  
     Use `sp.Reconstruct()` to estimate each column’s permanence value.

3. **Prepare Full Permanence Map**  
   - Store reconstructed values in a dictionary keyed by column index.  
   - Ensure all columns (0–199) exist in the map, filling missing ones with `0.0`.

4. **Save Permanence Values to CSV**  
   - Sort permanences by column index.  
   - Write `Column, Permanence` rows to a CSV in the `AllPermanenceValues` folder.

5. **Save Probabilities to CSV**  
   - Write `Column, Probability` rows to a CSV in the `AllProbabilities` folder.

6. **Threshold Permanence Values**  
   - Apply `Helpers.ThresholdProbabilities()` with a fixed threshold of `0.52`.  
   - Write the binary thresholded values (`0` or `1`) to a CSV in the `ThresholdValues` folder.

7. **Generate Heatmap Images**  
   - Reshape thresholded values into a 2D grid (64×64).  
   - Call `NeoCortexUtils.DrawHeatmaps()` to create a heatmap image.  
   - Save the `.png` file to the `RunRustructuringExperiment` folder.

8. **Repeat for All Inputs**  
   - The above process is repeated for each input value in `inputValues`.

## Dockerfile
~~~csharp
# Stage 1: Base runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyCloudProject/MyCloudProject.csproj", "MyCloudProject/"]
COPY ["MyCloudProject.Common/MyCloudProject.Common.csproj", "MyCloudProject.Common/"]
COPY ["MyExperiment/MyExperiment.csproj", "MyExperiment/"]
RUN dotnet restore "./MyCloudProject/MyCloudProject.csproj"
COPY . .
WORKDIR "/src/MyCloudProject"
RUN dotnet build "./MyCloudProject.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyCloudProject.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final runtime
FROM base AS final
WORKDIR /app

# Install required native libs for System.Drawing
RUN apt-get update && apt-get install -y --no-install-recommends \
    libgdiplus \
    libc6-dev \
    libx11-6 \
 && rm -rf /var/lib/apt/lists/*

# Ensure libdl.so symlink exists
RUN [ -e /usr/lib/x86_64-linux-gnu/libdl.so ] || \
    ln -s /usr/lib/x86_64-linux-gnu/libdl.so.2 /usr/lib/x86_64-linux-gnu/libdl.so

ENV LD_LIBRARY_PATH=/usr/lib:/usr/lib/x86_64-linux-gnu
ENV OUTPUT_ROOT=/tmp/outputfile
ENV DOTNET_ENVIRONMENT=Production

COPY --from=publish /app/publish .
COPY MyCloudProject/appsettings.json ./appsettings.json

ENTRYPOINT ["dotnet", "MyCloudProject.dll"]
~~~

### Purpose
Builds and packages the .NET 8.0 cloud experiment application into a multi-stage Docker image for efficient deployment.  
Includes installation of native libraries (`libgdiplus`, `libc6-dev`, `libx11-6`) required for **System.Drawing** to generate heatmaps inside Linux containers.  
Ensures `libdl.so` symlink and `LD_LIBRARY_PATH` are set for runtime compatibility.  
Configures environment variables and runs the application in a lightweight runtime image.

# 📊 Result  Discussion and Visualization of Permanence Values
### 1. Experiment Outcome
- The experiment successfully trained a **Hierarchical Temporal Memory (HTM) Spatial Pooler** to learn and recognize a sequence of scalar inputs.  
- The **`RunExperiment`** phase iteratively presented 0–`MaxVal` input values to the Spatial Pooler until it reached a stable state, as monitored by the `HomeostaticPlasticityController`.
- Once trained, the **`RunRustructuringExperiment`** reconstructed the permanence values for each column using the `sp.Reconstruct()` method, enabling visualization of the learned spatial patterns.

### 2. Output Artifacts
The process generated several structured outputs:
- **AllPermanenceValues (CSV):** Contains the permanence values for every column, representing the learned synaptic strengths after training.
- **AllProbabilities (CSV):** Contains the raw probability values output by `sp.Reconstruct()` for each column being active.
- **ThresholdValues (CSV):** Binary column activity indicators (1 = active, 0 = inactive) after applying a 0.52 probability threshold.
- **Heatmaps (PNG):** Visual representation of the binary activity patterns in a 64×64 grid format, enabling quick pattern recognition.

### 3. Interpretation of Heatmaps
- **Green areas** indicate columns below the `greenStart` threshold, representing low permanence values.
- **Red areas** indicate high permanence values above `redStart`, representing stable, strongly connected synapses.
- Across training iterations, the heatmaps transitioned from random noisy patterns to structured, repeatable patterns, reflecting learning stabilization.

### 4. Performance and Stability
- The **Homeostatic Plasticity Controller** helped the Spatial Pooler reach stability by balancing column activity across the network, preventing column inactivity.
- Stability was reached after ~5 stable cycles post-training, indicating efficient convergence given the selected parameters (`MaxBoost`, `DutyCyclePeriod`, `ActivationThreshold`, etc.).

### 5. Discussion
- The reconstruction via `sp.Reconstruct()` provides an interpretable mapping between active columns and original input features, which is critical for understanding HTM’s internal representations.
- By exporting CSVs and heatmaps, the experiment enables both **quantitative** (CSV analysis) and **qualitative** (visual inspection) assessment of HTM learning.
- The thresholding step is key to highlighting discrete active regions, making the heatmaps more interpretable compared to continuous probability maps.
- Running in a cloud environment (via Azure Storage + Docker) ensures that large-scale experiments can be processed, stored, and visualized efficiently.

## 📊 Heatmap Analysis
As a final outcome, 100 heatmaps are generated. Below, 4 representative heatmaps are shown for analysis purposes.

![results](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/comparison.png)
*Fig. 2 — Sample of generated heatmaps for analyzing the experiment outcomes*

The visualization of permanence values through 2D heatmaps provides an intuitive understanding of how the Spatial Pooler (SP) reconstruction process operates when threshold values are applied. Each heatmap corresponds to the binary permanence representation reconstructed by the Reconstruct() method, where 1 is mapped to red (active connection) and 0 is mapped to green (inactive connection). The threshold CSV files (0_thresholds.csv – 3_thresholds.csv) define the filtering conditions, directly producing the corresponding heatmaps (0_heatmap.png – 3_heatmap.png).

## Heatmap Observations of Fig. 2

- Threshold 0: A very strict threshold filters out most permanence values, producing maps dominated by green. Only a few red clusters appear, indicating a very limited number of strong synapses. While this reduces noise, it also risks under-representing weaker but potentially relevant connections (false negatives).
- Threshold 1: A moderate threshold reveals clearer vertical red bands alongside green areas. This suggests more permanence values are recognized as stable connections. It offers a balanced view, preserving significant synapses while avoiding excessive activation.
- Threshold 2: A looser threshold increases the density of red regions. More synapses are marked as strongly connected, improving sensitivity to subtle activations. However, this may also overemphasize weak connections, leading to false positives and reduced interpretability.
- Threshold 3: The least strict setting results in widespread red regions across the map. Nearly all permanence values surpass the threshold, suggesting a highly sensitive but noisy representation where specificity is lost. While this guarantees that no strong synapses are missed, it compromises clarity by labeling almost everything as stable.

Threshold tuning plays a critical role in balancing stability and noise within the reconstruction process of the Spatial Pooler. When the threshold is set too strictly, as in `Threshold 0`, the system underestimates connectivity and fails to capture weaker synapses, resulting in potential loss of relevant information. Conversely, very loose thresholds, such as `Threshold 3`, overestimate connectivity and mark nearly all synapses as stable, which reduces interpretability. The intermediate `thresholds (1–2)` offer the most meaningful representation by retaining essential permanence patterns without overwhelming the visualization with noise. Heatmaps therefore provide an intuitive way to distinguish between stable and unstable synapses, making the SP reconstruction process more interpretable. Overall, this experiment demonstrates that careful threshold selection is vital for maintaining biologically plausible and effective anomaly detection.

## Example Images

## 📷 A Complete Image of Running Locally

![Local Run](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/locally%20run.png?)

*Fig. 3 — Complete execution of the project running in a local environment.*

## 📂 Local Output Directories

![Local Output Directories](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/local%20output%20dir.png?)

*Fig. 4 — Local output directories generated after running the project.*

## 📂 Probability Values

![Probability Values](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/1_probabilities.png?)

*Fig. 5 — Example of probability values generated by the experiment.*

## 📊 Permanence Values

![Permanence Values](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/1_permanence.png?)

*Fig. 6 — Example of permanence values generated by the experiment.*

## 🎯 Threshold Values

![Threshold Values](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/1_thresolds.png?)

*Fig. 7 — Example of thresholded permanence values used to identify active columns.*

## 🌡️ Heatmap

![1_Threshold Heatmap](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/1_threshold_heatmap.png?)

*Fig. 8. 1_Threshold Heatmap visualization of thresholded permanence values in a 64×64 grid.*

# Azure Cloud Adaptation

## Experiment Processing Workflow

### 1. Create and Push Docker Image

1. **Define Docker Image**: Create a `Dockerfile` to specify the Docker image configuration.

2. **Build Docker Image**: Use the Docker build command to create the Docker image with a specific tag.

3. **Tag Docker Image**: Tag the image to prepare it for upload to a container registry.
  
4. **Push Docker Image**: Upload the tagged Docker image to the Azure Container Registry.

### Azure Container Registry

![Azure Container Registry](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/azure%20container%20registery.png?)

*Fig. 9 — Azure Container Registry used for storing and managing Docker images for the project.*


### 2. Resource Group: `RG-SamsilArefin`

![Resource Group](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/rg.png?)

*Fig. 10 — Azure Resource Group used for managing and organizing project resources.*


### 3. Run Docker Container Instance

![Run Docker Container Instance](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/azure%20container%20instance.jpeg?)

*Fig. 11. — Running the project inside an Azure Container Instance.*

### 4. Storage Browser

![Azure Storage Browser](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/storage%20browser.png?)

*Fig. 12. — Azure Storage Browser showing containers, blobs, and related experiment files.*

### 5. Queue Message

![Queue Message](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/queue%20msg.png?)

*Fig. 13 — Example of a queue message used to trigger the experiment.*

### 6. Blob Container

![Blob Storage](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/blob%20storage.png?)

*Fig. 14 — Example of a Blob Storage container containing the experiment output files.*

### 7. Table Storage

![Table Storage](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/table%20storage.png)

*Fig. 15 — Example of Azure Table Storage used for storing experiment metadata and results.*

### 8. Threshold Output in Blob Container

![Threshold Output in Blob Container](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2024-2025/blob/samsil-arefin/Source/MyCloudProjectSample/Documentation/threshold%20value%20in%20azure.png)

*Fig. 16 — Example of thresholded output values stored in the Blob Container for the experiment.*




## 📄 Result Table Schema

This table outlines the parameters and properties used when creating an entry in the Azure Table Storage **Result Table**.

| **Component**                  | **Description**                                                                                      |
|---------------------------------|------------------------------------------------------------------------------------------------------|
| `PartitionKey`                  | Logical grouping key for the entity in Table Storage.                                               |
| `RowKey`                        | Unique identifier for the entity within a partition.                                                |
| `Timestamp`                     | Timestamp maintained by Azure Table Storage.                                                        |
| `ETag`                          | ETag used for concurrency control in Table Storage.                                                  |
| `ExperimentId`                  | Unique identifier for the experiment.                                                               |
| `Name`                          | Friendly name for the experiment.                                                                   |
| `Description`                   | Description of the experiment, its purpose, or configuration details.                               |
| `StartTimeUtc`                  | UTC timestamp for when the experiment started.                                                      |
| `EndTimeUtc`                    | UTC timestamp for when the experiment ended.                                                        |
| `DurationSec`                   | Duration of the experiment execution, in seconds.                                                   |
| `MessageId`                     | ID of the message retrieved from the queue that triggered this experiment.                          |
| `MessageReceipt`                | Pop receipt of the message retrieved from the queue, required for deletion.                         |
| `outFolder`                     | Path to the output folder for general experiment results.                                            |
| `permFolder`                    | Path to the folder containing permanence values.                                                     |
| `probFolder`                    | Path to the folder containing probability results.                                                   |
| `W`                              | Width parameter used in the experiment's algorithm.                                                 |
| `N`                              | Number of columns or neurons in the model.                                                           |
| `Radius`                         | Receptive field radius for connections.                                                              |
| `MinVal`                         | Minimum input value in the dataset.                                                                  |
| `MaxVal`                         | Maximum input value in the dataset.                                                                  |
| `Periodic`                       | Indicates if the input is periodic.                                                                  |
| `ClipInput`                      | Indicates if inputs should be clipped to a certain range.                                            |
| `CellsPerColumn`                 | Number of cells per column in the model.                                                             |
| `MaxBoost`                       | Maximum boosting factor for underperforming columns.                                                 |
| `DutyCyclePeriod`                | Duty cycle period for tracking activity over time.                                                   |
| `MinPctOverlapDutyCycles`        | Minimum percentage overlap duty cycles for boosting.                                                 |
| `GlobalInhibition`               | Enables or disables global inhibition.                                                               |
| `NumActiveColumnsPerInhArea`     | Number of active columns per inhibition area.                                                        |
| `PotentialRadius`                | Potential radius defining the initial connection area.                                               |
| `NumColumns`                     | Number of columns in the spatial pooler.                                                             |
| `MinOctOverlapCycles`            | Minimum number of overlapping cycles for a segment.                                                  |
| `LocalAreaDensity`               | Local area density for active columns.                                                               |
| `ActivationThreshold`            | Minimum number of active synapses required for activation.                                           |
| `MaxSynapsesPerSegment`          | Maximum number of synapses per segment.                                                              |
| `Random`                         | Random number generator instance for reproducibility.                                                |
| `StimulusThreshold`              | Minimum stimulus threshold for activation.                                                           |
| `inputBits`                      | Number of input bits.                                                                                |

