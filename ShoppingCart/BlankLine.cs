using System;

namespace ShoppingCart
{
	public class BlankLine : Sample
	{
		public BlankLine (int column) : base (' ')
		{
			this.Column = column;
		}

		public int Column { get; private set; }
	}
}

