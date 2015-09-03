using System;
using NUnit.Framework;
using Accord.Math;
using ShoppingCart;
using Should;

namespace ShoppingCartTests
{
	[TestFixture]
	public class SampleTests
	{
		
		public SampleTests ()
		{			
		}

		[Test]
		public void ShouldCreateIntensityDistributionFromDiagonalQuadraticMatrix ()
		{
			double[,] matrix = Matrix.Diagonal (15, 1.0);
			var result = Sample.FromIntensityDistribution (matrix);

			result.Values.Length.ShouldEqual (64);
			int shift = 0;
			for (int i = 0; i < result.Values.Length; i += 8) {
				result.Values [i + shift].ShouldEqual (shift < 7 ? 1.0 : 0.5);	
				shift++;
			}
		}

		[Test]
		public void ShouldCreateIntensityDistributionFromRectangularMatrix ()
		{
			double[,] matrix = Matrix.Create (17, 15, 1.0);
			var result = Sample.FromIntensityDistribution (matrix);

			result.Values.Length.ShouldEqual (64);	
		}
	}
}

