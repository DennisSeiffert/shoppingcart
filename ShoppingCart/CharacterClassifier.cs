using System;
using System.Linq;

namespace ShoppingCart
{
	public class CharacterClassifier : ICharacterMatching
	{
		private ICharacterMatching digitClassifier, letterClassifier, specialCharacterClassifier;

		public CharacterClassifier (ICharacterMatching digitClassifier, ICharacterMatching letterClassifier, 
		                            ICharacterMatching specialCharacterClassifier)
		{
			this.specialCharacterClassifier = specialCharacterClassifier;
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
			double[] prob = new double[3];
			char[] character = new char[3];
			character [0] = this.digitClassifier.Detect (sample, out prob [0]);
			prob [0] = 0.0;
			character [1] = this.letterClassifier.Detect (sample, out prob [1]);
			if (sample.MaybeSpecialCharacter) {				
				prob [0] = 0.0;
				prob [1] = 0.0;
				character [2] = this.specialCharacterClassifier.Detect (sample, out prob [2]);
			}				

			var i = prob.ToList ().IndexOf (prob.Max ());
			probability = prob [i];
			return character [i];
		}

		#endregion
	}
}

