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

		char ICharacterMatching.Recognize (Sample sample)
		{
			return ' ';
		}

		#endregion
	}
}

