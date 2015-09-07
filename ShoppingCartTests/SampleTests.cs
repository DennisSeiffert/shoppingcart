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
			double[,] matrix = Matrix.Diagonal (32, 1.0);
			var result = Sample.FromIntensityDistribution (matrix);

			result.Values.Length.ShouldEqual (64);
			int shift = 0;
			for (int i = 0; i < result.Values.Length; i += 9) {
				result.Values [i].ShouldEqual (0.75);	
				shift++;
			}
		}

		[Test]
		public void ShouldCreateIntensityDistributionFromRectangularMatrix ()
		{
			double[,] matrix = Matrix.Create (32, 32, 1.0);
			var result = Sample.FromIntensityDistribution (matrix);

			result.Values.Length.ShouldEqual (64);	
		}
	}
}

