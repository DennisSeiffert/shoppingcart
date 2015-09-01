using System;
using AForge.Neuro;
using Accord.Neuro;
using AForge.Neuro.Learning;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart
{
	public class OptDigitClassifier : ICharacterMatching
	{
		private ActivationNetwork network;

		public OptDigitClassifier (IEnumerable<Sample> samples)
		{
			this.network = this.InitializeNetwork ();
			this.Train (samples);
		}

		public OptDigitClassifier (string filename)
		{
			this.network = (ActivationNetwork)ActivationNetwork.Load (filename);
		}

		private ActivationNetwork InitializeNetwork ()
		{
			var sigmoid = new SigmoidFunction ();
			var network = new ActivationNetwork (sigmoid, 64, 15, 10);
			new NguyenWidrow (network).Randomize ();
			return network;
		}

		private void Train (IEnumerable<Sample> samples)
		{
			var learning = new BackPropagationLearning (this.network);
			foreach (var sample in samples) {
				double[] expectedResult = new double[10];
				expectedResult [sample.Digit] = 1.0;
				var error = learning.Run (sample.Values, expectedResult);
				Console.Out.WriteLine ("Error: {0}", error);
			}
		}

		public void Save (string filename)
		{
			this.network.Save (filename);
		}

		#region ICharacterMatching implementation

		char ICharacterMatching.Recognize (Sample sample)
		{
			var result = this.network.Compute (sample.Values);
			var recognizedDigit = result.ToList ().IndexOf (result.Max ());
			return (char)recognizedDigit;
		}

		#endregion
	}
}

