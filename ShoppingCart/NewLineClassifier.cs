using System;
using System.Linq;

namespace ShoppingCart
{
	public class NewLineClassifier : ICharacterMatching
	{
		public NewLineClassifier ()
		{
		}

		#region ICharacterMatching implementation

		char ICharacterMatching.Recognize (Sample sample)
		{
			if (sample.Values.Sum () / sample.Values.Length < 0.05) {
				return '\n';
			}

			return ' ';
		}

		#endregion
	}
}

