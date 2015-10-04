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


//			int rows = imageRows.Count (), columns = imageRows.First ().Values.Length;
//			int powerOf2Rows = Accord.Math.Tools.NextPowerOf2 (rows), powerOf2Columns = Accord.Math.Tools.NextPowerOf2 (columns);
//			var complexImageData = new Complex[powerOf2Rows, powerOf2Columns];
//			for (int i = 0; i < powerOf2Rows; i++) {
//				for (int j = 0; j < powerOf2Columns; j++) {
//					if (i < rows && j < columns) {
//						complexImageData [i, j] = new Complex (imageRows.ElementAt (i).Values [j], 0.0);	
//					} else {
//						complexImageData [i, j] = new Complex ();	
//					}
//				}
//			}
//			var whitePixelPerLine = imageRows.Select (l => l.Values.Select (v => new Complex (v, 0.0)));
//			var len = whitePixelPerLine.Count;
//			var requiredLength = Accord.Math.Tools.NextPowerOf2 (len);
//			if (requiredLength > len) {
//				whitePixelPerLine.AddRange (new Complex[requiredLength - len]);	
//			}
				
//			FourierTransform.DFT2 (complexImageData, FourierTransform.Direction.Forward);		

			var readShoppingCart = new List<char> ();
			//ImageBox.Show (this.image);
//			var kirsch = new KirschEdgeDetector ();
//			var edges = kirsch.Apply (new UnmanagedImage (image.LockBits (new Rectangle (0, 0, this.image.Width, this.image.Height), 
//				            System.Drawing.Imaging.ImageLockMode.ReadWrite, this.image.PixelFormat)));
//			ImageBox.Show (edges);

//			HarrisCornersDetector hcd = new HarrisCornersDetector ();
//			hcd.Measure = HarrisCornerMeasure.Noble;
//			// process image searching for corners
//			IEnumerable<IntPoint> corners = hcd.ProcessImage (image);
//			// Create a filter to mark the corners
//			PointsMarker marker = new PointsMarker (corners);
//			
//			// Apply the corner-marking filter
//			Bitmap markers = marker.Apply (image);
			
			// Show on the screen
			//ImageBox.Show (markers);

//			FastCornersDetector fast = new FastCornersDetector () {
//				Suppress = true, // suppress non-maximum points
//				Threshold = 40   // less leads to more corners
//			};
//
//			// Process the image looking for corners
//			List<IntPoint> points = fast.ProcessImage (image);
//
//			// Create a filter to mark the corners
//			PointsMarker marker = new PointsMarker (points);
//
//			// Apply the corner-marking filter
//			Bitmap markers = marker.Apply (image);
//
//			// Show on the screen
//			ImageBox.Show (markers);



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

