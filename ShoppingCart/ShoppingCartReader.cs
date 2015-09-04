using System;
using ShoppingCart.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Accord.Math;
using System.Threading.Tasks;
using System.Text;
using Accord.Controls;
using System.Windows.Forms;

namespace ShoppingCart
{
	public class ShoppingCartReader
	{
		private ICharacterMatching digitClassifier;

		private LineSegmentation lineSegmentation;

		private BlockSegmentation blockSegmentation;

		private Bitmap image;

		public ShoppingCartReader (ICharacterMatching digitClassifier, ICharacterMatching newLineClassifier, 
		                           ICharacterMatching blankLineClassifier)
		{			
			this.lineSegmentation = new LineSegmentation (newLineClassifier);
			this.blockSegmentation = new BlockSegmentation (blankLineClassifier);
			this.digitClassifier = digitClassifier;
		}

		public string Read (string shoppingCartImageFilename)
		{
			this.image = new Bitmap (shoppingCartImageFilename);
			return this.Read (ImageAdapter.Read (shoppingCartImageFilename));
		}

		public string Read (IEnumerable<Sample> imageRows)
		{			
			var lines = this.lineSegmentation.Segment (imageRows).ToList ();
			var readShoppingCart = new StringBuilder ();
			var blockRectangles = new List<Rectangle> ();

			foreach (var line in lines) {				
				int blockLeftBorder = 0, blockRightBorder = 0;
				var blockTopBorder = (line.First (l => l is CarriageReturn) as CarriageReturn).Row;
				var blockBottomBorder = (line.Last (l => l is CarriageReturn) as CarriageReturn).Row;
				foreach (var block in this.blockSegmentation.Segment (line)) {					
					if (block is BlankLine) {
						blockLeftBorder = blockRightBorder;
						blockRightBorder = (block as BlankLine).Column;
						continue;
					}

					int width = blockRightBorder - blockLeftBorder, height = blockBottomBorder - blockTopBorder;
					if (width * height > 0) {
						blockRectangles.Add (new Rectangle (blockLeftBorder, blockTopBorder, width, height));	
					}

					char digit = this.digitClassifier.Recognize (block);
					readShoppingCart.Append (digit.ToString ());
				} 					
				readShoppingCart.AppendLine ();
			}				

			using (var g = Graphics.FromImage (this.image)) {
				foreach (var rect in blockRectangles.Where(r => r.Width > 1)) {
					g.DrawRectangle (new Pen (Color.Red, 1.0f), rect);	
				}
			}
			ImageBox.Show (this.image, PictureBoxSizeMode.Zoom);

			return readShoppingCart.ToString ();
		}


	}
}

