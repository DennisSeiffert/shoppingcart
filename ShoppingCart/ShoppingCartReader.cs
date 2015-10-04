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
using Accord.Imaging.Filters;
using Accord.Imaging;
using AForge;
using AForge.Imaging;
using AForge.Math;
using System.Numerics;

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
			var readShoppingCart = new List<char> ();

			using (var g = Graphics.FromImage (this.image)) {
				foreach (var line in lines) {
					var endOfPreviousBlock = 0;	
					var blocksPerLine = this.blockSegmentation.Segment (line).ToList ();
					blocksPerLine = this.blockSegmentation.RemoveEmptyBlocks (blocksPerLine).ToList ();
					blocksPerLine = this.blockSegmentation.MergeNeighboredBlocks (blocksPerLine).ToList ();
					blocksPerLine = this.blockSegmentation.RemoveSkinnyBlocks (blocksPerLine).ToList ();

					foreach (var block in blocksPerLine) {
						var distance = block.Column - endOfPreviousBlock;
						readShoppingCart.AddRange (Enumerable.Repeat (' ', (int)Math.Floor (distance / 5.0)));
						endOfPreviousBlock = block.Column + block.Width;
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
					readShoppingCart.Add ('\n');
				}	
			}				
		
			ImageBox.Show (this.image, PictureBoxSizeMode.Zoom);
			return new string (readShoppingCart.ToArray ());
		}


	}
}

