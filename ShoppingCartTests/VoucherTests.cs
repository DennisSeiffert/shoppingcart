using System;
using NUnit.Framework;
using ShoppingCart;
using Should;

namespace ShoppingCartTests
{
	[TestFixture]
	public class VoucherTests
	{
		[Test]
		public void ShouldParseVoucherFromString ()
		{
			var sut = new Voucher ("Summe 5.67");

			sut.Total.ShouldEqual (5.67);
		}
	}
}

