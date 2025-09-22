using Azure;
using Azure.Data.Tables;
using MyCloudProject.Common;
using System;

namespace MyExperiment
{
    /// <summary>
    /// Represents the result of an experiment and provides storage mapping for Azure Table Storage.
    /// 
    /// Implements:
    /// - <see cref="ITableEntity"/> for Azure Table Storage persistence.
    /// - <see cref="IExperimentResult"/> for experiment result standardization.
    /// </summary>
    public class ExperimentResult : ITableEntity, IExperimentResult
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExperimentResult"/> with the specified partition and row keys.
        /// </summary>
        /// <param name="partitionKey">Partition key for logical grouping in Azure Table Storage.</param>
        /// <param name="rowKey">Row key for uniquely identifying the entity within the partition.</param>
        public ExperimentResult(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        #region Azure Table Storage Metadata

        /// <summary>
        /// Logical grouping key for the entity in Table Storage.
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// Unique identifier for the entity within a partition.
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Timestamp maintained by Azure Table Storage.
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// ETag used for concurrency control in Table Storage.
        /// </summary>
        public ETag ETag { get; set; }

        #endregion

        #region Experiment Metadata

        /// <summary>
        /// Unique identifier for the experiment.
        /// </summary>
        public string ExperimentId { get; set; }

        /// <summary>
        /// Friendly name for the experiment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the experiment, its purpose, or configuration details.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// UTC timestamp for when the experiment started.
        /// </summary>
        public DateTime? StartTimeUtc { get; set; }

        /// <summary>
        /// UTC timestamp for when the experiment ended.
        /// </summary>
        public DateTime? EndTimeUtc { get; set; }

        /// <summary>
        /// Duration of the experiment execution, in seconds.
        /// </summary>
        public long DurationSec { get; set; }

        /// <summary>
        /// ID of the message retrieved from the queue that triggered this experiment.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Pop receipt of the message retrieved from the queue, required for deletion.
        /// </summary>
        public string MessageReceipt { get; set; }

        #endregion

        #region Output Paths

        /// <summary>
        /// Path to the output folder for general experiment results.
        /// </summary>
        public string outFolder { get; set; }

        /// <summary>
        /// Path to the folder containing permanence values.
        /// </summary>
        public string permFolder { get; set; }

        /// <summary>
        /// Path to the folder containing probability results.
        /// </summary>
        public string probFolder { get; set; }

        #endregion

        #region Experiment Parameters

        /// <summary>
        /// Width parameter used in the experiment's algorithm.
        /// </summary>
        public int W { get; set; }

        /// <summary>
        /// Number of columns or neurons in the model.
        /// </summary>
        public int N { get; set; }

        /// <summary>
        /// Receptive field radius for connections.
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Minimum input value in the dataset.
        /// </summary>
        public double MinVal { get; set; }

        /// <summary>
        /// Maximum input value in the dataset.
        /// </summary>
        public double MaxVal { get; set; }

        /// <summary>
        /// Indicates if the input is periodic.
        /// </summary>
        public bool Periodic { get; set; }

        /// <summary>
        /// Indicates if inputs should be clipped to a certain range.
        /// </summary>
        public bool ClipInput { get; set; }

        /// <summary>
        /// Number of cells per column in the model.
        /// </summary>
        public int CellsPerColumn { get; set; }

        /// <summary>
        /// Maximum boosting factor for underperforming columns.
        /// </summary>
        public double MaxBoost { get; set; }

        /// <summary>
        /// Duty cycle period for tracking activity over time.
        /// </summary>
        public int DutyCyclePeriod { get; set; }

        /// <summary>
        /// Minimum percentage overlap duty cycles for boosting.
        /// </summary>
        public double MinPctOverlapDutyCycles { get; set; }

        /// <summary>
        /// Enables or disables global inhibition.
        /// </summary>
        public bool GlobalInhibition { get; set; }

        /// <summary>
        /// Number of active columns per inhibition area.
        /// </summary>
        public double NumActiveColumnsPerInhArea { get; set; }

        /// <summary>
        /// Potential radius defining the initial connection area.
        /// </summary>
        public int PotentialRadius { get; set; }

        /// <summary>
        /// Number of columns in the spatial pooler.
        /// </summary>
        public int NumColumns { get; set; }

        /// <summary>
        /// Minimum number of overlapping cycles for a segment.
        /// </summary>
        public double MinOctOverlapCycles { get; set; }

        /// <summary>
        /// Local area density for active columns.
        /// </summary>
        public double LocalAreaDensity { get; set; }

        /// <summary>
        /// Minimum number of active synapses required for activation.
        /// </summary>
        public int ActivationThreshold { get; set; }

        /// <summary>
        /// Maximum number of synapses per segment.
        /// </summary>
        public int MaxSynapsesPerSegment { get; set; }

        /// <summary>
        /// Random number generator instance for reproducibility.
        /// </summary>
        public Random Random { get; set; }

        /// <summary>
        /// Minimum stimulus threshold for activation.
        /// </summary>
        public double StimulusThreshold { get; set; }

        /// <summary>
        /// Number of input bits.
        /// </summary>
        public int inputBits { get; set; }

        #endregion
    }
}
