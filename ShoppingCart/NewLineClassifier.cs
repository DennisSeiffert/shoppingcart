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
			if (sample.Values.Average () > 0.5) {
				return '\n';
			}

			return ' ';
		}

		#endregion
	}
}

