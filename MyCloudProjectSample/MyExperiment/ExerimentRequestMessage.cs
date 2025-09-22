using MyCloudProject.Common;
using System;

namespace MyExperiment
{
    /// <summary>
    /// Concrete implementation of <see cref="IExerimentRequest"/>.
    /// 
    /// This class acts as the Data Transfer Object (DTO) for experiment requests
    /// received from Azure Storage Queue messages.  
    /// It contains:
    /// - General experiment metadata (ID, name, description, etc.)
    /// - Queue message metadata (MessageId, PopReceipt) for processing and deletion
    /// - Parameter configuration for the experiment run
    /// </summary>
    /// <remarks>
    /// Instances are typically created via JSON deserialization
    /// in <see cref="AzureStorageProvider.ReceiveExperimentRequestAsync"/>.
    /// </remarks>
    internal class ExerimentRequestMessage : IExerimentRequest
    {
        /// <inheritdoc />
        public string ExperimentId { get; set; }

        /// <inheritdoc />
        public string InputFile { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string Description { get; set; }

        /// <summary>
        /// Unique identifier assigned by Azure Storage Queue to this message.
        /// Required for <see cref="AzureStorageProvider.CommitRequestAsync"/> deletion.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Opaque token returned by Azure Storage Queue when receiving a message.  
        /// Must be passed along with <see cref="MessageId"/> to delete the message.
        /// </summary>
        public string MessageReceipt { get; set; }

        // --- Experiment parameter properties (copied from IExerimentRequest) ---

        public int W { get; set; }
        public int N { get; set; }
        public double Radius { get; set; }
        public double MinVal { get; set; }
        public double MaxVal { get; set; }
        public bool Periodic { get; set; }
        public bool ClipInput { get; set; }
        public int CellsPerColumn { get; set; }
        public double MaxBoost { get; set; }
        public int DutyCyclePeriod { get; set; }
        public double MinPctOverlapDutyCycles { get; set; }
        public bool GlobalInhibition { get; set; }
        public double NumActiveColumnsPerInhArea { get; set; }
        public int PotentialRadius { get; set; }
        public int NumColumns { get; set; }
        public double MinOctOverlapCycles { get; set; }
        public double LocalAreaDensity { get; set; }
        public int ActivationThreshold { get; set; }
        public int MaxSynapsesPerSegment { get; set; }

        /// <summary>
        /// Random number generator used for reproducible runs.
        /// Can be null if not explicitly set in the request.
        /// </summary>
        public Random Random { get; set; }

        public double StimulusThreshold { get; set; }
        public int inputBits { get; set; }
    }
}
