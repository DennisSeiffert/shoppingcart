using System;

namespace ShoppingCart
{
	public class CarriageReturn : Sample
	{
		public CarriageReturn (int row) : base ('\n')
		{
			this.Row = row;
		}

		public int Row {
			get;
			private set;
		}
	}
}

