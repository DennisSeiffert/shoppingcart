using System;
using System.Linq;
using Accord.Statistics;

namespace ShoppingCart
{
	public class NewLineClassifier : ICharacterMatching
	{
		public NewLineClassifier ()
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
			int lineFragments = 0;
			for (int i = 0; i < sample.Values.Length; i += 10) {
				var sum = 0.0;
				for (int index = i; index < Math.Min (sample.Values.Length, i + 10); index++)
					sum += sample.Values [index];
				if (sum > 9.95)
					lineFragments++;
			}


			if (lineFragments / (sample.Values.Length / 10.0) > 0.9) {
				probability = 1.0;
				return '\n';
			}
			probability = 0.0;
			return ' ';
		}

		#endregion
	}
}

