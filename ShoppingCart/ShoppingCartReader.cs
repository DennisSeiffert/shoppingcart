using System;
using ShoppingCart.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Accord.Math;
using System.Threading.Tasks;
using System.Text;

namespace ShoppingCart
{
	public class ShoppingCartReader
	{
		private ICharacterMatching digitClassifier;

		private LineSegmentation lineSegmentation;

		private BlockSegmentation blockSegmentation;

		public ShoppingCartReader (ICharacterMatching digitClassifier, ICharacterMatching newLineClassifier, 
		                           ICharacterMatching blankLineClassifier)
		{			
			this.lineSegmentation = new LineSegmentation (newLineClassifier);
			this.blockSegmentation = new BlockSegmentation (blankLineClassifier);
			this.digitClassifier = digitClassifier;
		}

		public string Read (string shoppingCartImageFilename)
		{
			return this.Read (ImageAdapter.Read (shoppingCartImageFilename));
		}

		public string Read (IEnumerable<Sample> imageRows)
		{			
			var lines = this.lineSegmentation.Segment (imageRows).ToList ();
			var readShoppingCart = new StringBuilder ();

			foreach (var line in lines) {
				foreach (var block in this.blockSegmentation.Segment (line)) {
					char digit = this.digitClassifier.Recognize (block);
					readShoppingCart.Append (digit.ToString ());
				} 					
				readShoppingCart.AppendLine ();
			}

			return readShoppingCart.ToString ();
		}


	}
}

