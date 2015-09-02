using System;
using ShoppingCart.IO;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart
{
	public class ShoppingCartReader
	{
		private ICharacterMatching digitClassifier;

		private ICharacterMatching newLineClassifier;

		public ShoppingCartReader (ICharacterMatching digitClassifier, ICharacterMatching newLineClassifier)
		{
			this.newLineClassifier = newLineClassifier;
			this.digitClassifier = digitClassifier;
		}

		public string Read (string shoppingCartImageFilename)
		{
			return this.Read (ImageAdapter.Read (shoppingCartImageFilename));
		}

		public string Read (IEnumerable<Sample> imageDataPerLine)
		{
			var imageDataPerLineWithCarriageReturns = ExtractCarriageReturns (imageDataPerLine).ToList ();

			return string.Empty;
		}

		private IEnumerable<Sample> ExtractCarriageReturns (IEnumerable<Sample> imageDataPerLine)
		{			
			foreach (var line in imageDataPerLine) {								
				if (this.newLineClassifier.Recognize (line) == '\n') {					
					yield return new Sample ('\n');
				}
				yield return line;					
			}
		}
	}
}

