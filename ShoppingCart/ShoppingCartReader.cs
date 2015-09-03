using System;
using ShoppingCart.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Accord.Math;
using System.Threading.Tasks;

namespace ShoppingCart
{
	public class ShoppingCartReader
	{
		private ICharacterMatching digitClassifier;

		private LineSegmentation lineSegmentation;

		private ICharacterMatching blankLineClassifier;

		public ShoppingCartReader (ICharacterMatching digitClassifier, ICharacterMatching newLineClassifier, 
		                           ICharacterMatching blankLineClassifier)
		{
			this.blankLineClassifier = blankLineClassifier;
			this.lineSegmentation = new LineSegmentation (newLineClassifier);
			this.digitClassifier = digitClassifier;
		}

		public string Read (string shoppingCartImageFilename)
		{
			return this.Read (ImageAdapter.Read (shoppingCartImageFilename));
		}

		public string Read (IEnumerable<Sample> imageRows)
		{			
			var lines = this.lineSegmentation.Segment (imageRows).ToList ();

			foreach (var line in lines) {
				int leftBorder = 0;
				int rightBorder = 0;
				double[,] matrix = Matrix.Create (line.Count, 1, 0.0);
				for (int i = 0; i < line.First ().Values.Length; i++) {
					var verticalLine = line.Select (s => s.Values [i]).ToArray ();
					matrix.InsertColumn (verticalLine);
					var character = this.blankLineClassifier.Recognize (new Sample (verticalLine, 1.0));
					if (character == '|') {
						rightBorder = i;
						int height = line.Count, width = rightBorder - leftBorder;
						int segmentationWindowWidth = (int)Math.Ceiling (width / 8.0), segmentationWindowHeight = (int)Math.Ceiling (height / 8.0);

						double[] characterIntensityDistribution = new double[64];
						int quadrant = 0;
						double maxValue = 0;
						for (int stepY = 0; stepY < width; stepY += segmentationWindowHeight) {							
							for (int stepX = 0; stepX < width; stepX += segmentationWindowWidth) {
								var activatedPixels = 0.0;
								var subMatrix = matrix.Submatrix (stepY, stepY + segmentationWindowHeight, stepX, stepX + segmentationWindowWidth);
								subMatrix.Apply (e => activatedPixels += e);
								characterIntensityDistribution [quadrant] = activatedPixels;
								maxValue = Math.Max (maxValue, activatedPixels);

								quadrant++;
							}	
						}

						matrix = Matrix.Create (line.Count, 1, 0.0);

						char digit = this.digitClassifier.Recognize (new Sample (characterIntensityDistribution, maxValue));

						Console.Out.Write (digit);

						leftBorder = i;
					}
				}

			}

			return string.Empty;
		}


	}
}

