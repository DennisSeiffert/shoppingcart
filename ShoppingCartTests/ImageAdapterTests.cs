using System;
using NUnit.Framework;
using ShoppingCart;
using System.Linq;
using Should;
using ShoppingCart.IO;

namespace ShoppingCartTests
{
	[TestFixture]
	public class ImageAdapterTests
	{
		[Test]
		public void ShouldImportJpgImageFile ()
		{
			var samples = ImageAdapter.Read ("testImage.jpg", false);

			samples.Count ().ShouldBeGreaterThan (10);
		}
	}
}

