using System;
using System.Collections.Generic;
using System.Text;

namespace MyCloudProject.Common
{
    /// <summary>
    /// Represents the result of an experiment execution, including
    /// metadata, execution times, configuration parameters, and runtime statistics.
    /// </summary>
    public interface IExperimentResult
    {
        /// <summary>
        /// Unique identifier for the experiment run.
        /// This may match the input request's ExperimentId or be generated dynamically.
        /// </summary>
        string ExperimentId { get; set; }

        /// <summary>
        /// The UTC timestamp indicating when the experiment started.
        /// </summary>
        DateTime? StartTimeUtc { get; set; }

        /// <summary>
        /// The UTC timestamp indicating when the experiment ended.
        /// </summary>
        DateTime? EndTimeUtc { get; set; }

        /// <summary>
        /// Total execution duration of the experiment in seconds.
        /// </summary>
        long DurationSec { get; set; }

        // === Experiment configuration parameters ===

        /// <summary>
        /// Parameter W - typically represents input space size or number of inputs.
        /// </summary>
        int W { get; set; }

        /// <summary>
        /// Parameter N - typically represents total number of processing units or columns.
        /// </summary>
        int N { get; set; }

        /// <summary>
        /// The inhibition radius or neighborhood size used in processing.
        /// </summary>
        double Radius { get; set; }

        /// <summary>
        /// Minimum input value allowed for normalization or scaling.
        /// </summary>
        double MinVal { get; set; }

        /// <summary>
        /// Maximum input value allowed for normalization or scaling.
        /// </summary>
        double MaxVal { get; set; }

        /// <summary>
        /// Whether periodic boundary conditions are enabled.
        /// </summary>
        bool Periodic { get; set; }

        /// <summary>
        /// Indicates whether to clip input values outside the allowed range.
        /// </summary>
        bool ClipInput { get; set; }

        /// <summary>
        /// Number of cells per processing column.
        /// </summary>
        int CellsPerColumn { get; set; }

        /// <summary>
        /// Maximum boosting factor applied during learning.
        /// </summary>
        double MaxBoost { get; set; }

        /// <summary>
        /// Number of cycles to consider when calculating duty cycles.
        /// </summary>
        int DutyCyclePeriod { get; set; }

        /// <summary>
        /// Minimum percentage of overlapping duty cycles required.
        /// </summary>
        double MinPctOverlapDutyCycles { get; set; }

        /// <summary>
        /// Whether global inhibition is enabled for the network.
        /// </summary>
        bool GlobalInhibition { get; set; }

        /// <summary>
        /// Number of active columns per inhibition area.
        /// </summary>
        double NumActiveColumnsPerInhArea { get; set; }

        /// <summary>
        /// The potential radius used in connectivity calculations.
        /// </summary>
        int PotentialRadius { get; set; }

        /// <summary>
        /// Total number of columns in the model.
        /// </summary>
        int NumColumns { get; set; }

        /// <summary>
        /// Minimum number of overlapping cycles for certain learning updates.
        /// </summary>
        double MinOctOverlapCycles { get; set; }

        /// <summary>
        /// Fraction of columns within a local area that should be active.
        /// </summary>
        double LocalAreaDensity { get; set; }

        /// <summary>
        /// Minimum number of active synapses required to activate a cell.
        /// </summary>
        int ActivationThreshold { get; set; }

        /// <summary>
        /// Maximum number of synapses allowed per segment.
        /// </summary>
        int MaxSynapsesPerSegment { get; set; }

        /// <summary>
        /// Random number generator instance used for stochastic processing.
        /// </summary>
        Random Random { get; set; }

        /// <summary>
        /// Minimum stimulation value required for activation.
        /// </summary>
        double StimulusThreshold { get; set; }

        /// <summary>
        /// Number of bits in the input representation.
        /// </summary>
        int inputBits { get; set; }
    }
}
