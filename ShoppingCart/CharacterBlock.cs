using System;
using System.Drawing;

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

		public bool IsEmpty ()
		{
			return Width * Height == 0;
		}

		public Rectangle ToRectangle ()
		{
			return new Rectangle (this.Column, this.Row, this.Width, this.Height);
		}

		public int Height { get; private set; }

		public int Width { get; private set; }

		public int Row { get; private set; }

		public int Column { get; private set; }
	}
}

