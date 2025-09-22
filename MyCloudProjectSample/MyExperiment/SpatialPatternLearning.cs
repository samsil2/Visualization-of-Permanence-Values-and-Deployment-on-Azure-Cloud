using MyCloudProject.Common;
using NeoCortex;
using NeoCortexApi;
using NeoCortexApi.Encoders;
using NeoCortexApi.Entities;
using NeoCortexApi.Network;
using NeoCortexApi.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace NeoCortexApiSample
{
    /// <summary>
    /// Implements an experiment that demonstrates how to learn spatial patterns.
    /// SP will learn every presented input in multiple iterations.
    /// </summary>
    public class SpatialPatternLearning
    {
        private const int OutImgSize = 1024;
        public void Run(IExerimentRequest request)
        {
            Console.WriteLine($"Hello NeocortexApi! Experiment {nameof(SpatialPatternLearning)}");

            // Used as a boosting parameters
            // that ensure homeostatic plasticity effect.
            double minOctOverlapCycles = request.MinOctOverlapCycles;
            double maxBoost =request.MaxBoost;

            // We will use 200 bits to represent an input vector (pattern).
            int inputBits = request.inputBits;

            // We will build a slice of the cortex with the given number of mini-columns
            int numColumns = request.NumColumns;

            //
            // This is a set of configuration parameters used in the experiment.
            HtmConfig cfg = new HtmConfig(new int[] { inputBits }, new int[] { numColumns })
            {
                CellsPerColumn = request.CellsPerColumn,
                MaxBoost = maxBoost,
                DutyCyclePeriod = request.DutyCyclePeriod,
                MinPctOverlapDutyCycles = minOctOverlapCycles,

                GlobalInhibition = request.GlobalInhibition,
                NumActiveColumnsPerInhArea = 0.02 * numColumns,
                PotentialRadius = (int)(0.15 * inputBits),
                LocalAreaDensity = request.LocalAreaDensity,
                ActivationThreshold = request.ActivationThreshold,

                MaxSynapsesPerSegment = (int)(0.01 * numColumns),
                Random = new ThreadSafeRandom(42),
                StimulusThreshold = request.StimulusThreshold,
            };

           
            // This dictionary defines a set of typical encoder parameters.
            Dictionary<string, object> settings = new Dictionary<string, object>()
            {
                { "W", request.W},
                { "N", inputBits},
                { "Radius", request.Radius},
                { "MinVal", request.MinVal},
                { "Periodic", request.Periodic},
                { "Name", "scalar"},
                { "ClipInput", request.ClipInput},
                { "MaxVal", request.MaxVal}
            };


            EncoderBase encoder = new NeoCortexApi.Encoders.ScalarEncoder(settings);

            // We create here 100 random input values.
            List<double> inputValues = new List<double>();

            for (int i = 0; i < (int)request.MaxVal; i++)
            {
                inputValues.Add((double)i);
            }

            var sp = RunExperiment(cfg, encoder, inputValues);

            RunRustructuringExperiment(sp, encoder, inputValues);
        }



        /// <summary>
        /// Implements the experiment.
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="encoder"></param>
        /// <param name="inputValues"></param>
        /// <returns>The trained bersion of the SP.</returns>
        private static SpatialPooler RunExperiment(HtmConfig cfg, EncoderBase encoder, List<double> inputValues)
        {
            // Creates the htm memory.
            var mem = new Connections(cfg);

            bool isInStableState = false;

            //
            // HPC extends the default Spatial Pooler algorithm.
            // The purpose of HPC is to set the SP in the new-born stage at the begining of the learning process.
            // In this stage the boosting is very active, but the SP behaves instable. After this stage is over
            // (defined by the second argument) the HPC is controlling the learning process of the SP.
            // Once the SDR generated for every input gets stable, the HPC will fire event that notifies your code
            // that SP is stable now.
            HomeostaticPlasticityController hpa = new HomeostaticPlasticityController(mem, inputValues.Count * 40,
                (isStable, numPatterns, actColAvg, seenInputs) =>
                {
                    // Event should only be fired when entering the stable state.
                    // Ideal SP should never enter unstable state after stable state.
                    if (isStable == false)
                    {
                        Debug.WriteLine($"INSTABLE STATE");
                        // This should usually not happen.
                        isInStableState = false;
                    }
                    else
                    {
                        Debug.WriteLine($"STABLE STATE");
                        // Here you can perform any action if required.
                        isInStableState = true;
                    }
                });

            // It creates the instance of Spatial Pooler Multithreaded version.
            SpatialPooler sp = new SpatialPooler(hpa);
            //sp = new SpatialPoolerMT(hpa);

            // Initializes the 
            sp.Init(mem, new DistributedMemory() { ColumnDictionary = new InMemoryDistributedDictionary<int, NeoCortexApi.Entities.Column>(1) });

            // mem.TraceProximalDendritePotential(true);

            // It creates the instance of the neo-cortex layer.
            // Algorithm will be performed inside of that layer.
            CortexLayer<object, object> cortexLayer = new CortexLayer<object, object>("L1");

            // Add encoder as the very first module. This model is connected to the sensory input cells
            // that receive the input. Encoder will receive the input and forward the encoded signal
            // to the next module.
            cortexLayer.HtmModules.Add("encoder", encoder);

            // The next module in the layer is Spatial Pooler. This module will receive the output of the
            // encoder.
            cortexLayer.HtmModules.Add("sp", sp);

            double[] inputs = inputValues.ToArray();

            // Will hold the SDR of every inputs.
            Dictionary<double, int[]> prevActiveCols = new Dictionary<double, int[]>();

            // Will hold the similarity of SDKk and SDRk-1 fro every input.
            Dictionary<double, double> prevSimilarity = new Dictionary<double, double>();

            //
            // Initiaize start similarity to zero.
            foreach (var input in inputs)
            {
                prevSimilarity.Add(input, 0.0);
                prevActiveCols.Add(input, new int[0]);
            }

            // Learning process will take 1000 iterations (cycles)
            int maxSPLearningCycles = 1000;

            int numStableCycles = 0;

            for (int cycle = 0; cycle < maxSPLearningCycles; cycle++)
            {
                Debug.WriteLine($"Cycle  ** {cycle} ** Stability: {isInStableState}");

                //
                // This trains the layer on input pattern.
                foreach (var input in inputs)
                {
                    double similarity;

                    // Learn the input pattern.
                    // Output lyrOut is the output of the last module in the layer.
                    // 
                    var lyrOut = cortexLayer.Compute((object)input, true) as int[];

                    // This is a general way to get the SpatialPooler result from the layer.
                    var activeColumns = cortexLayer.GetResult("sp") as int[];

                    var actCols = activeColumns.OrderBy(c => c).ToArray();

                    similarity = MathHelpers.CalcArraySimilarity(activeColumns, prevActiveCols[input]);

                    Debug.WriteLine($"[cycle={cycle.ToString("D4")}, i={input}, cols=:{actCols.Length} s={similarity}] SDR: {Helpers.StringifyVector(actCols)}");

                    prevActiveCols[input] = activeColumns;
                    prevSimilarity[input] = similarity;
                }

                if (isInStableState)
                {
                    numStableCycles++;
                }

                if (numStableCycles > 5)
                    break;
            }

            return sp;
        }


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
        // Heatmaps are generated to visually represent the thresholded permanence values using the NeoCortexUtils.DrawHeatmaps() method.
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



    }



}
