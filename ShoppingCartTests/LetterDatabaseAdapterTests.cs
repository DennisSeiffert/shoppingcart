using System;
using NUnit.Framework;
using ShoppingCart.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using Should;
using System.IO;

namespace ShoppingCartTests
{
	[TestFixture]
	public class LetterDatabaseAdapterTests
	{
		public LetterDatabaseAdapterTests ()
		{
		}

		[Test]
		public void ShouldCreateLetterDatabase ()
		{
			var sut = new LetterDatabaseAdapter ();
			InstalledFontCollection fontFamilies = new InstalledFontCollection ();
			var fonts = fontFamilies.Families.Select (fF => new Font (
				            fF,
				            24,
				            FontStyle.Regular,
				            GraphicsUnit.Pixel)).ToArray ();
			string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZüöäÜÖÄ,.-+*=;:_";
			var result = sut.Write (letters.ToCharArray (), fonts.Take (100));

			result.ShouldNotBeEmpty ();

			// File.WriteAllText ("optletters.tra", result);
		}
	}
}

