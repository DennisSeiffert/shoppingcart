using System;
using System.Linq;

namespace ShoppingCart
{
	public class BlankLineClassifier : ICharacterMatching
	{
		#region ICharacterMatching implementation

		char ICharacterMatching.Recognize (Sample sample)
		{
			return this.OnRecognize (sample);
		}

		#endregion

		private char OnRecognize (Sample sample)
		{
			var sum = sample.Values.Sum ();
			if (sum / sample.Values.Length > 0.9) {
				return '|';
			}
			return ' ';
		}
	}
}

