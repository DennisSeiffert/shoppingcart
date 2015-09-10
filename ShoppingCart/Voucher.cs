using System;
using Antlr4.Runtime;
using System.Linq;

namespace ShoppingCart
{
	public class Voucher : voucherBaseListener
	{
		public Voucher (string voucherText)
		{	
			var lexer = new voucherLexer (new AntlrInputStream (voucherText));
			CommonTokenStream tokens = new CommonTokenStream (lexer);
			var parser = new voucherParser (tokens);
			parser.AddParseListener (this);

			parser.voucher ();
		}

		public override void ExitLine (voucherParser.LineContext context)
		{
			base.ExitLine (context);

			bool isTotalSum = context.children.Any (c => c.GetText () == "Summe");
			if (isTotalSum) {
				this.Total = double.Parse (context.price ().GetText ());
			}
		}

		public double Total { get; private set; }
	}
}

