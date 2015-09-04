using System;
using NUnit.Framework;
using ShoppingCart;
using Should;

namespace ShoppingCartTests
{
	[TestFixture ()]
	public class NewLineClassifierTests
	{
		[Test]
		public void ShouldClassifySampleAsNewLine ()
		{
			var sample = new Sample (new double[]{ 0.6, 0.5, 0.5, 1.0 }, 1.0);
			ICharacterMatching sut = new NewLineClassifier ();

			sut.Detect (sample).ShouldEqual ('\n');
		}

		[Test]
		public void ShouldMisclassifySampleAsNewLine ()
		{
			var sample = new Sample (new double[]{ 0.0, 0.4, 0.2, 1.0 }, 1.0);
			ICharacterMatching sut = new NewLineClassifier ();

			sut.Detect (sample).ShouldEqual (' ');
		}
	}
}

