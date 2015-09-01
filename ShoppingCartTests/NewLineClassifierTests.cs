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
			var sample = new Sample (new double[]{ 0.0, 0.0, 0.0, 1.0 });
			ICharacterMatching sut = new NewLineClassifier ();

			sut.Recognize (sample).ShouldEqual ('\n');
		}

		[Test]
		public void ShouldMisclassifySampleAsNewLine ()
		{
			var sample = new Sample (new double[]{ 0.0, 1.0, 1.0, 1.0 });
			ICharacterMatching sut = new NewLineClassifier ();

			sut.Recognize (sample).ShouldEqual (' ');
		}
	}
}

