using System;
using System.Linq;
using System.Collections.Generic;

namespace ShoppingCart
{
	public class SpecialCharacterClassifier : NeuralNetwork, ICharacterMatching
	{
		//		public const string CHARACTERS = ",.-;:_+*&%€";
		public const string CHARACTERS = ",-";

		private readonly char[] characters;

		public SpecialCharacterClassifier (IEnumerable<Sample> samples) : base (samples, CHARACTERS.ToCharArray (), 0.01, 64, 15, CHARACTERS.Length)
		{
			characters = CHARACTERS.ToCharArray ();
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
			return this.characters [recognizedDigit];
		}

		#endregion
	}
}

