using System;

namespace ShoppingCart
{
	public class CharacterClassifier : ICharacterMatching
	{
		private ICharacterMatching digitClassifier, letterClassifier;

		public CharacterClassifier (ICharacterMatching digitClassifier, ICharacterMatching letterClassifier)
		{
			this.letterClassifier = letterClassifier;
			this.digitClassifier = digitClassifier;
		}

		#region ICharacterMatching implementation

		char ICharacterMatching.Detect (Sample sample)
		{
			double probability;
			return (this as ICharacterMatching).Detect (sample, out probability);
		}


		char ICharacterMatching.Detect (Sample sample, out double probability)
		{
			double digitProb, letterProb;
			char digit, letter;
			digit = this.digitClassifier.Detect (sample, out digitProb);

			letter = this.letterClassifier.Detect (sample, out letterProb);

			if (letterProb > digitProb) {
				probability = letterProb;
				return letter;
			}

			probability = digitProb;
			return digit;
		}

		#endregion
	}
}

