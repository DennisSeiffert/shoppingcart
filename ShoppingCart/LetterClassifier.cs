using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart
{
	public class LetterClassifier : NeuralNetwork, ICharacterMatching
	{
		public const string LETTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZüöäÜÖÄ,.-+*=;:_";
		//public const string LETTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		//public const string LETTERS = "ABCDEFGHI";
		private char[] letters;

		public LetterClassifier (IEnumerable<Sample> samples) : base (samples, 
			                                                             LETTERS.ToCharArray (),
			                                                             64, 35, LETTERS.Length)
		{
			letters = LETTERS.ToCharArray ();
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
			return this.letters [recognizedDigit];
		}

		#endregion
	}
}

