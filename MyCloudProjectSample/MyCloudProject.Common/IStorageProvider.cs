using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCloudProject.Common
{
    /// <summary>
    /// Defines the contract for all storage operations required by the experiment system.
    /// 
    /// Implementations of this interface handle:
    /// - Receiving experiment requests from a queue
    /// - Uploading experiment results to cloud storage
    /// - Storing experiment metadata in tables
    /// - Removing processed requests from the queue
    /// 
    /// This abstraction allows for different storage backends (e.g., Azure, AWS, local file system)
    /// without changing the experiment execution logic.
    /// </summary>
    public interface IStorageProvider
    {
        /// <summary>
        /// Receives the next experiment request message from the queue.
        /// 
        /// This method should:
        /// 1. Poll the configured queue
        /// 2. Retrieve the next available message
        /// 3. Deserialize it into an <see cref="IExerimentRequest"/> object
        /// 4. Return null if the queue is empty
        /// </summary>
        /// <param name="token">
        /// A cancellation token to stop queue polling gracefully.
        /// </param>
        /// <returns>
        /// An <see cref="IExerimentRequest"/> representing the experiment request,
        /// or <c>null</c> if the queue is empty.
        /// </returns>
        Task<IExerimentRequest> ReceiveExperimentRequestAsync(CancellationToken token);

        /// <summary>
        /// Uploads the full output of the experiment to cloud blob storage (or similar).
        /// 
        /// Typically, this involves uploading generated files, logs, or any artifacts
        /// produced during the experiment execution.
        /// </summary>
        /// <param name="experimentName">
        /// The name to be used for storing the experiment output remotely.
        /// This can act as a folder/container prefix in blob storage.
        /// </param>
        /// <param name="result">
        /// The result object containing experiment output data and metadata.
        /// </param>
        /// <remarks>
        /// This corresponds to step 5 (in reverse direction) in the architecture diagram.
        /// </remarks>
        Task UploadResultAsync(string experimentName, IExperimentResult result);

        /// <summary>
        /// Confirms successful processing of a queue message and removes it from the queue.
        /// 
        /// This prevents the message from being reprocessed.
        /// </summary>
        /// <param name="request">
        /// The request object originally returned by <see cref="ReceiveExperimentRequestAsync"/>.
        /// </param>
        Task CommitRequestAsync(IExerimentRequest request);

        /// <summary>
        /// Uploads the experiment result metadata to a table storage system.
        /// 
        /// This typically stores:
        /// - Experiment identifiers
        /// - Start/end times
        /// - Configuration parameters
        /// - File paths or URLs to results in blob storage
        /// 
        /// This method is designed for structured metadata storage (Azure Table Storage).
        /// </summary>
        /// <param name="result">
        /// The experiment result containing both metadata and links to actual output files.
        /// </param>
        Task UploadExperimentResult(IExperimentResult result);
    }
}
