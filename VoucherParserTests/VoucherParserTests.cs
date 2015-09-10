using NUnit.Framework;
using System;
using Antlr4.Runtime;

namespace VoucherParserTests
{
	[TestFixture ()]
	public class VoucherParserTests
	{
		[Test ()]
		public void ShouldParseSimpleVoucher ()
		{
			var lexer = new voucherLexer (new AntlrInputStream ("Summe 4.56"));
			CommonTokenStream tokens = new CommonTokenStream (lexer);
			var parser = new voucherParser (tokens);
			var listener = new voucherBaseListener ();
			parser.AddParseListener (listener);

			var context = parser.voucher ();
		}
	}
}

