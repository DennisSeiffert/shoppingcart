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

		char ICharacterMatching.Recognize (Sample sample)
		{				
			return this.OnRecognize (sample);
		}

		protected virtual char OnRecognize (Sample sample)
		{
			var deviation = sample.Values.StandardDeviation (false);
			if (deviation < 0.1) {
				return '\n';
			}

			return ' ';
		}

		#endregion
	}
}

