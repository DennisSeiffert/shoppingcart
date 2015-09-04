using System;

namespace ShoppingCart
{
	public class CharacterBlock : Sample
	{
		public CharacterBlock (int row, int column, int width, int height) : base (' ')
		{
			this.Height = height;
			this.Width = width;
			this.Row = row;
			this.Column = column;
		}

		public int Height { get; private set; }

		public int Width { get; private set; }

		public int Row { get; private set; }

		public int Column { get; private set; }
	}
}

