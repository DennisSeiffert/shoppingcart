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

		char ICharacterMatching.Recognize (Sample sample)
		{
			var result = this.network.Compute (sample.Values);
			var maxProbability = result.Max ();

			var recognizedDigit = result.ToList ().IndexOf (maxProbability);
			return (char)recognizedDigit;
		}

		#endregion
	}
}

