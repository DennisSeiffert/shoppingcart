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
		private ICharacterMatching characterClassifier;

		private LineSegmentation lineSegmentation;

		private BlockSegmentation blockSegmentation;

		private Bitmap image;

		public ShoppingCartReader (ICharacterMatching characterClassifier, ICharacterMatching newLineClassifier, 
		                           ICharacterMatching blankLineClassifier)
		{
			this.lineSegmentation = new LineSegmentation (newLineClassifier);
			this.blockSegmentation = new BlockSegmentation (blankLineClassifier);
			this.characterClassifier = characterClassifier;
		}

		public string Read (string shoppingCartImageFilename)
		{
			this.image = new Bitmap (shoppingCartImageFilename);
			return this.Read (ImageAdapter.Read (shoppingCartImageFilename));
		}

		public string Read (IEnumerable<Sample> imageRows)
		{			
			var lines = this.lineSegmentation.Segment (imageRows).ToList ();
			var blockRectangles = new List<Rectangle> ();

			foreach (var line in lines) {				
				foreach (var block in this.blockSegmentation.Segment (line).Cast<CharacterBlock>()) {						
					if (block.Width * block.Height > 0) {
						blockRectangles.Add (new Rectangle (block.Column, block.Row, block.Width, block.Height));	
					}
				} 					
			}				

			for (int i = 1; i < blockRectangles.Count; i++) {
				var current = blockRectangles [i];
				var previous = blockRectangles [i - 1];
				if (previous.Y == current.Y && current.X - previous.X + previous.Width < 5) {
					current = new Rectangle (previous.X, previous.Y, current.X - previous.X + current.Width, current.Height);
					blockRectangles.RemoveAt (i - 1);
					--i;
				}
			}	
//			double[,] imageMatrix = new double[imageRows.Count (), imageRows.First ().Values.Length];
//			unsafe {
//				for (int i = 0; i < imageRows.Count (); i++) {
//					fixed(double* pFirstValue = imageRows.ElementAt (i).Values, pElement = imageMatrix [i, ]) {
//						double* firstValue = pFirstValue;
//						double* element = pElement;
//						element = firstValue;
//					}
//				}
//			}

			using (var g = Graphics.FromImage (this.image)) {
				foreach (var rect in blockRectangles.Where(r => r.Width > 1)) {
					g.DrawRectangle (new Pen (Color.Red, 1.0f), rect);	


//					char digit = this.characterClassifier.Detect (block);
//					readShoppingCart.Append (digit.ToString ());
				}
			}
			ImageBox.Show (this.image, PictureBoxSizeMode.Zoom);

			var readShoppingCart = new StringBuilder ();
			return readShoppingCart.ToString ();
		}


	}
}

