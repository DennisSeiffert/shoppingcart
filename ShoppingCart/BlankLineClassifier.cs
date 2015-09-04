using System;
using System.Linq;

namespace ShoppingCart
{
	public class BlankLineClassifier : ICharacterMatching
	{
		#region ICharacterMatching implementation

		char ICharacterMatching.Detect (Sample sample)
		{
			double prob = 0.0;
			return (this as ICharacterMatching).Detect (sample, out prob);
		}


		char ICharacterMatching.Detect (Sample sample, out double probability)
		{
			var sum = sample.Values.Sum ();
			if (sum / sample.Values.Length > 0.95) {
				probability = 1.0;
				return '|';
			}
			probability = 0.0;
			return ' ';
		}

		#endregion

	}
}

