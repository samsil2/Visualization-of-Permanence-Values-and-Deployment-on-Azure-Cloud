using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCloudProject.Common
{
    /// <summary>
    /// Represents the contract for an experiment execution component.
    /// 
    /// Implementations of this interface are responsible for:
    /// - Executing the experiment logic using provided input parameters.
    /// - Producing a structured result object containing output data, 
    ///   metrics, and metadata.
    /// 
    /// This abstraction allows different experiment implementations to be
    /// interchangeable, enabling dependency injection and easier testing.
    /// </summary>
    public interface IExperiment
    {
        /// <summary>
        /// Executes the experiment asynchronously using the provided request data.
        /// </summary>
        /// <param name="request">
        /// An <see cref="IExerimentRequest"/> containing all required 
        /// configuration parameters, input file references, and metadata 
        /// needed to run the experiment.
        /// </param>
        /// <returns>
        /// A <see cref="Task{IExperimentResult}"/> representing the asynchronous operation.
        /// The result contains experiment outputs, execution statistics, 
        /// and any generated metadata.
        /// </returns>
        Task<IExperimentResult> RunAsync(IExerimentRequest request);
    }
}
