using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart
{
	public class LetterClassifier : NeuralNetwork, ICharacterMatching
	{
		public LetterClassifier (IEnumerable<Sample> samples) : base (samples, 
			                                                             "abcdefghijklmnopqrstuvwxyz".ToCharArray (),
			                                                             64, 25, "abcdefghijklmnopqrstuvwxyz".Length)
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

			var recognizedDigit = result.ToList ().IndexOf (probability);
			return (char)recognizedDigit;
		}

		#endregion
	}
}

