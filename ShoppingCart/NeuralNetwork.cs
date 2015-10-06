using System;
using AForge.Neuro;
using Accord.Neuro;
using AForge.Neuro.Learning;
using System.Collections.Generic;
using System.Linq;
using Accord.Controls;
using System.Windows.Forms;

namespace ShoppingCart
{
	public class NeuralNetwork
	{
		protected ActivationNetwork network;

		double trainingThreshold;

		public NeuralNetwork (IEnumerable<Sample> samples, char[] trainingSet, double trainingThreshold, int inputsCount, params int[] neuronsCount)
		{
			this.trainingThreshold = trainingThreshold;
			this.network = this.InitializeNetwork (inputsCount, neuronsCount);
			this.Train (trainingSet, samples);
		}

		public NeuralNetwork (string filename)
		{
			this.network = (ActivationNetwork)ActivationNetwork.Load (filename);
		}

		private ActivationNetwork InitializeNetwork (int inputsCount, params int[] neuronsCount)
		{
			var sigmoid = new SigmoidFunction ();
			var network = new ActivationNetwork (sigmoid, inputsCount, neuronsCount);
			new NguyenWidrow (network).Randomize ();
			return network;
		}

		private void Train (char[] trainingSet, IEnumerable<Sample> samples)
		{
			var learning = new BackPropagationLearning (this.network);
			var error = double.MaxValue;
			var maximumError = 0.11;
			var averageMinimumError = 1.0;
			var errorsPerCharacter = new Dictionary<char, double> ();
			var innerSamples = samples.ToList ();
			while (averageMinimumError > this.trainingThreshold) {	
				maximumError = 0.0;
				foreach (var sample in innerSamples) {				
					double[] expectedResult = new double[this.network.Layers.Last ().Neurons.Length];
					expectedResult [trainingSet.ToList ().IndexOf (sample.Character)] = 1.0;
					error = learning.Run (sample.Values, expectedResult);
					maximumError = Math.Max (error, maximumError);
					//Console.Out.WriteLine ("Error: {0}", error);

					if (!errorsPerCharacter.ContainsKey (sample.Character)) {
						errorsPerCharacter.Add (sample.Character, error);
					} else {
						errorsPerCharacter [sample.Character] = error;
					}

					averageMinimumError = errorsPerCharacter.Values.Average ();
				}
			}

			foreach (var errorPerCharacter in errorsPerCharacter) {
				Console.Out.WriteLine ("Fehler für '{0}' : {1}", errorPerCharacter.Key, errorPerCharacter.Value);
			}
		}

		public void Save (string filename)
		{
			this.network.Save (filename);
		}
	}
}

