using Azure.Core;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using NeoCortexApiSample;
using System;
using System.Threading.Tasks;

namespace MyExperiment
{
    /// <summary>
    /// Implements experiment that runs in the cloud environment.
    /// 
    /// This class:
    /// - Receives experiment parameters from <see cref="IExerimentRequest"/>
    /// - Executes the core algorithm (<see cref="SpatialPatternLearning"/>)
    /// - Tracks execution time and metadata
    /// - Produces an <see cref="IExperimentResult"/> for upload
    /// 
    /// Originally refactored from the Software Engineering course project to work in a cloud setting.
    /// </summary>
    public class Experiment : IExperiment
    {
        private readonly IStorageProvider storageProvider;
        private readonly ILogger logger;
        private readonly MyConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="Experiment"/> class.
        /// </summary>
        /// <param name="configSection">Configuration section containing experiment and group settings.</param>
        /// <param name="storageProvider">Storage provider for persisting and retrieving experiment data.</param>
        /// <param name="log">Logger for tracking execution details.</param>
        public Experiment(IConfigurationSection configSection, IStorageProvider storageProvider, ILogger log)
        {
            this.storageProvider = storageProvider;
            this.logger = log;

            // Bind configuration settings
            config = new MyConfig();
            configSection.Bind(config);
        }

        /// <summary>
        /// Executes the experiment workflow using the provided request parameters.
        /// </summary>
        /// <param name="request">The experiment configuration and metadata.</param>
        /// <returns>
        /// An <see cref="IExperimentResult"/> object containing experiment output and metadata.
        /// </returns>
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

        /// <summary>
        /// Creates and populates an <see cref="ExperimentResult"/> with values from the request.
        /// </summary>
        /// <param name="request">Incoming experiment configuration and metadata.</param>
        /// <returns>
        /// A populated <see cref="ExperimentResult"/> instance ready to be processed and stored.
        /// </returns>
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
    }
}
