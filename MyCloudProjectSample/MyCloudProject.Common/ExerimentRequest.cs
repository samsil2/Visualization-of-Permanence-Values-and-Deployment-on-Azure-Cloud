using System;
using System.Collections.Generic;
using System.Text;

namespace MyCloudProject.Common
{
    /// <summary>
    /// Defines the contract for an experiment request message.
    /// 
    /// This request object contains all parameters and metadata 
    /// required to execute an experiment, including:
    /// - Identification details (ExperimentId, Name, Description)
    /// - Execution metadata (message queue tracking info)
    /// - Model configuration parameters (e.g., W, N, Radius)
    /// - Input dataset information
    /// 
    /// Implementations of this interface are typically serialized 
    /// into messages stored in Azure Storage Queues and later 
    /// deserialized by the experiment runner.
    /// </summary>
    public interface IExerimentRequest
    {
        /// <summary>
        /// Unique identifier for the experiment request.
        /// Often used as the primary key for logging and storage.
        /// </summary>
        string ExperimentId { get; set; }

        /// <summary>
        /// Path or name of the input file to be used for the experiment.
        /// May refer to a local path, blob name, or other storage location.
        /// </summary>
        string InputFile { get; set; }

        /// <summary>
        /// Human-readable name for the experiment.
        /// Useful for logging and reports.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Optional description of the experiment's purpose or configuration.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Azure Queue Storage message ID for tracking this request.
        /// This is assigned when the message is received from the queue.
        /// </summary>
        string MessageId { get; set; }

        /// <summary>
        /// Azure Queue Storage message pop receipt, used for message deletion/commit.
        /// </summary>
        string MessageReceipt { get; set; }

        // ───────────────────────────────
        // Model / Experiment Parameters
        // ───────────────────────────────

        /// <summary>
        /// Number of bits representing the size of the input encoding window.
        /// </summary>
        int W { get; set; }

        /// <summary>
        /// Number of columns (input space dimensionality).
        /// </summary>
        int N { get; set; }

        /// <summary>
        /// Radius around a column to determine potential synapse connections.
        /// </summary>
        double Radius { get; set; }

        /// <summary>
        /// Minimum value for input scaling/normalization.
        /// </summary>
        double MinVal { get; set; }

        /// <summary>
        /// Maximum value for input scaling/normalization.
        /// </summary>
        double MaxVal { get; set; }

        /// <summary>
        /// Indicates whether the input space is periodic (wrap-around allowed).
        /// </summary>
        bool Periodic { get; set; }

        /// <summary>
        /// If true, clip input values outside the [MinVal, MaxVal] range.
        /// </summary>
        bool ClipInput { get; set; }

        /// <summary>
        /// Number of cells per column in the model.
        /// </summary>
        int CellsPerColumn { get; set; }

        /// <summary>
        /// Maximum boosting factor applied to column overlap scores.
        /// </summary>
        double MaxBoost { get; set; }

        /// <summary>
        /// Time period (in iterations) for calculating duty cycles.
        /// </summary>
        int DutyCyclePeriod { get; set; }

        /// <summary>
        /// Minimum percentage overlap duty cycles for a column to be considered active.
        /// </summary>
        double MinPctOverlapDutyCycles { get; set; }

        /// <summary>
        /// Enables or disables global inhibition across the network.
        /// </summary>
        bool GlobalInhibition { get; set; }

        /// <summary>
        /// Number of active columns allowed per inhibition area.
        /// </summary>
        double NumActiveColumnsPerInhArea { get; set; }

        /// <summary>
        /// Radius used for selecting potential synapses during initialization.
        /// </summary>
        int PotentialRadius { get; set; }

        /// <summary>
        /// Total number of columns in the model.
        /// </summary>
        int NumColumns { get; set; }

        /// <summary>
        /// Minimum overlap cycles for a column to remain in the connected state.
        /// </summary>
        double MinOctOverlapCycles { get; set; }

        /// <summary>
        /// Fraction of input bits that should be active within a local area.
        /// </summary>
        double LocalAreaDensity { get; set; }

        /// <summary>
        /// Number of active synapses required for a segment to become active.
        /// </summary>
        int ActivationThreshold { get; set; }

        /// <summary>
        /// Maximum number of synapses allowed per segment.
        /// </summary>
        int MaxSynapsesPerSegment { get; set; }

        /// <summary>
        /// Random number generator for stochastic elements of the experiment.
        /// </summary>
        Random Random { get; set; }

        /// <summary>
        /// Minimum number of active synapses required to trigger a column response.
        /// </summary>
        double StimulusThreshold { get; set; }

        /// <summary>
        /// Total number of bits used for input encoding.
        /// </summary>
        int inputBits { get; set; }
    }
}
