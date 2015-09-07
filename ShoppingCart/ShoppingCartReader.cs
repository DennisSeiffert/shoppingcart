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
using System.Drawing.Text;

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
			this.image = ImageAdapter.BinarizeImage (shoppingCartImageFilename);
			return this.Read (ImageAdapter.Read (this.image));
		}

		public string Read (IEnumerable<Sample> imageRows)
		{			
			var lines = this.lineSegmentation.Segment (imageRows).ToList ();
			var blocks = new List<CharacterBlock> ();

			foreach (var line in lines) {				
				blocks.AddRange (this.blockSegmentation.Segment (line));
			}								

			blocks = this.blockSegmentation.RemoveEmptyBlocks (blocks).ToList ();
			blocks = this.blockSegmentation.MergeNeighboredBlocks (blocks).ToList ();
			blocks = this.blockSegmentation.RemoveSkinnyBlocks (blocks).ToList ();

			var readShoppingCart = new List<char> ();
			using (var g = Graphics.FromImage (this.image)) {
				foreach (var block in blocks) {
					g.DrawRectangle (new Pen (Color.Red, 1.0f), block.ToRectangle ());	

					var imageMatrix = Matrix.Create<double> (0, block.Width);
					for (int i = block.Row; i < block.Row + block.Height; i++) {						
						imageMatrix = imageMatrix.InsertRow (imageRows.ElementAt (i).Values.Submatrix (block.Column, block.Column + block.Width - 1));
					}

					Bitmap blockImage;
					new Accord.Imaging.Converters.MatrixToImage ().Convert (imageMatrix, out blockImage);
//					ImageBox.Show (blockImage);
					imageMatrix = LetterDatabaseAdapter.NormalizeBitmap (blockImage);

					if (imageMatrix.Length > 0) {										
						var intensityBlock = Sample.FromIntensityDistribution (imageMatrix);

						char digit = this.characterClassifier.Detect (intensityBlock);

						g.DrawString (new string (digit, 1), new Font ("Arial", 12), Brushes.Blue, block.Column, Math.Max (block.Row - 15, 0));

						readShoppingCart.Add (digit);
					}
				}
			}
			ImageBox.Show (this.image, PictureBoxSizeMode.Zoom);
					
			return new string (readShoppingCart.ToArray ());
		}


	}
}

