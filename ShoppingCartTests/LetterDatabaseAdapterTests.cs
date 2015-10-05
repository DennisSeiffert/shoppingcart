using System;
using NUnit.Framework;
using ShoppingCart.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using Should;
using System.IO;
using ShoppingCart;

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
			InstalledFontCollection fontFamilies = new InstalledFontCollection ();
			var fonts = fontFamilies.Families.Select (fF => new Font (
				            fF,
				            24,
				            FontStyle.Regular,
				            GraphicsUnit.Pixel)).ToArray ();
			string letters = LetterClassifier.LETTERS;
			var result = LetterDatabaseAdapter.Write (letters.ToCharArray (), fonts.Where (f => new [] { 	
				"Arial",
				"Arial Black",
				"Verdana",
				"Courier New",
				"Georgia"
			}.Contains (f.FontFamily.Name)));

			result.ShouldNotBeEmpty ();

			File.WriteAllText ("optletters.tra", result);
		}

		[Test]
		public void ShouldCreateDigitDatabase ()
		{			
			InstalledFontCollection fontFamilies = new InstalledFontCollection ();
			var fonts = fontFamilies.Families.Select (fF => new Font (
				            fF,
				            24,
				            FontStyle.Regular,
				            GraphicsUnit.Pixel)).ToArray ();
			string digits = DigitClassifier.DIGITS;
			var result = LetterDatabaseAdapter.Write (digits.ToCharArray (), fonts.Where (f => new [] {
				"Arial",
				"Arial Black",
				"Verdana",
				"Courier New",
				"Georgia"
			}.Contains (f.FontFamily.Name)));

			result.ShouldNotBeEmpty ();

			File.WriteAllText ("optdigitsblockletters.tra", result);
		}

		[Test]
		public void ShouldCreateSpecialCharactersDatabase ()
		{			
			InstalledFontCollection fontFamilies = new InstalledFontCollection ();
			var fonts = fontFamilies.Families.Select (fF => new Font (
				            fF,
				            24,
				            FontStyle.Regular,
				            GraphicsUnit.Pixel)).ToArray ();
			string specialCharacters = SpecialCharacterClassifier.CHARACTERS;
			var result = LetterDatabaseAdapter.Write (specialCharacters.ToCharArray (), fonts.Where (f => new [] {
				"Arial",
				"Arial Black",
				"Verdana",
				"Courier New",
				"Georgia"
			}.Contains (f.FontFamily.Name)));

			result.ShouldNotBeEmpty ();

			File.WriteAllText ("optspecialcharacters.tra", result);
		}
	}
}

