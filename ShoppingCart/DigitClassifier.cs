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
	public class DigitClassifier : NeuralNetwork, ICharacterMatching
	{
		public const string DIGITS = "0123456789";
		private char[] digits = DIGITS.ToCharArray ();

		public DigitClassifier (IEnumerable<Sample> samples) : base (samples, DIGITS.ToCharArray (), 0.001, 64, 15, DIGITS.Length)
		{
		}

		public DigitClassifier (string filename) : base (filename)
		{			
		}

		#region ICharacterMatching implementation

		char ICharacterMatching.Detect (Sample sample)
		{
			double prob = 0.0;
			return (this as ICharacterMatching).Detect (sample, out prob);
		}


		char ICharacterMatching.Detect (Sample sample, out double probability)
		{
			var result = this.network.Compute (sample.Values);
			probability = result.Max ();

			if (probability < 0.8) {
				probability = 0.0;
				return ' ';
			}

			var indexOfRecognizedDigit = result.ToList ().IndexOf (probability);
			return digits [indexOfRecognizedDigit];		
		}

		#endregion
	}
}
