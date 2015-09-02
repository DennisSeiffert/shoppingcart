using System;
using NUnit.Framework;
using ShoppingCart;
using System.Linq;
using Should;

namespace ShoppingCartTests
{
	[TestFixture]
	public class ImageAdapterTests
	{
		[Test]
		public void ShouldImportJpgImageFile ()
		{
			var samples = ImageAdapter.Read ("testImage.jpg");

			samples.Count ().ShouldBeGreaterThan (10);
		}
	}
}

