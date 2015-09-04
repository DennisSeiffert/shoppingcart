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

		public NeuralNetwork (IEnumerable<Sample> samples, char[] trainingSet, int inputsCount, params int[] neuronsCount)
		{
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
			foreach (var sample in samples) {				
				double[] expectedResult = new double[10];
				expectedResult [trainingSet.ToList ().IndexOf (sample.Character)] = 1.0;
				var error = learning.Run (sample.Values, expectedResult);
				Console.Out.WriteLine ("Error: {0}", error);
			}
		}

		public void Save (string filename)
		{
			this.network.Save (filename);
		}
	}
}

