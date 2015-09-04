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
			var deviation = sample.Values.StandardDeviation (false);
			if (deviation < 0.1) {
				probability = 1.0;
				return '\n';
			}
			probability = 0.0;
			return ' ';
		}

		#endregion
	}
}

