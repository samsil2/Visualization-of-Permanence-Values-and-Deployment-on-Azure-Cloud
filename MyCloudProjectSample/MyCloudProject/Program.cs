using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Data.Tables.Sas;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using MyExperiment;

namespace MyCloudProject
{
    /// <summary>
    /// Console entry point for running the cloud experiment pipeline.
    /// 
    /// Responsibilities:
    /// 1. Initialize configuration and logging.
    /// 2. Poll the trigger queue for incoming experiment requests.
    /// 3. Execute the experiment with the provided request payload.
    /// 4. Upload generated artifacts (blobs) and persist summary (table).
    /// 5. Commit the request (delete message).
    /// 
    /// This program is designed to run continuously until canceled (Ctrl+C).
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Descriptive project name for logs/telemetry.
        /// </summary>
        private static readonly string _projectName = "ML 24/25-4 Implementation: Visualization of Permanence Values";


        /// <summary>
        /// Application entry point. Sets up configuration, logging, and the main processing loop.
        /// </summary>
        /// <param name="args">Optional command line arguments (e.g., --env or custom config paths).</param>
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
    }
}
