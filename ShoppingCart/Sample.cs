using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord.Math;

namespace ShoppingCart
{
	public class Sample
	{
		double maxValue;

		public Sample (double[] values, double maxValue) : this (values.Take (values.Length - 1).ToArray (),
			                                                        char.Parse (values.Last ().ToString ()), maxValue)
		{			
		}

		public Sample (char character) : this (new double[]{ }, character, 0.0)
		{
		}

		public Sample (double[] values, char character, double maxValue)
		{
			this.maxValue = maxValue;
			this.Values = values;
			this.Normalize ();
			this.Character = character;
		}

		public double[] Values { get; private set; }

		public char Character { get; private set; }

		public bool MaybeSpecialCharacter { get; set; }

		void Normalize ()
		{			
			if (this.maxValue > 0.0) {
				this.Values = this.Values.Select (v => v / this.maxValue).ToArray ();	
			}
		}

		/// <summary>
		/// returns a intensity distribution of dynamically sized quadrants from the matrix. 
		/// Nevertheless the resulting sample has a fixed length of 64 elements.
		/// This representation is slightly invariant to minimal translation and scaling.
		/// </summary>
		/// <returns>The intensity distribution.</returns>
		/// <param name="matrix">Matrix.</param>
		public static Sample FromIntensityDistribution (double[,] matrix)
		{
			if (matrix.Rows () != 32 || matrix.Columns () != 32) {
				throw new ArgumentException ("matrix must be 32x32.");
			}

			double[] characterIntensityDistribution = new double[64];
			int quadrant = 0;
			double maxValue = 0, sumOfActivatedPixels = 0.0;
			int width = 32;
			int height = 32;
			int segmentationWindowWidth = 4, segmentationWindowHeight = 4;
			bool maybeSpecialCharacter = false;

			for (int stepY = 0; stepY < height; stepY += segmentationWindowHeight) {
				for (int stepX = 0; stepX < width; stepX += segmentationWindowWidth) {
					var activatedPixels = 0.0;
					var subMatrix = matrix.Submatrix (
						                stepY, stepY + segmentationWindowHeight - 1, 
						                stepX, stepX + segmentationWindowWidth - 1);
					subMatrix.Apply (e => activatedPixels += 1.0 - e);
					characterIntensityDistribution [quadrant] = activatedPixels;
					maxValue = Math.Max (maxValue, activatedPixels);
					sumOfActivatedPixels += activatedPixels;
					quadrant++;
				}
				maybeSpecialCharacter = stepY == 12 && sumOfActivatedPixels == 0;
			}
			return new Sample (characterIntensityDistribution, ' ', maxValue) { MaybeSpecialCharacter = maybeSpecialCharacter };
		}
	}
}
