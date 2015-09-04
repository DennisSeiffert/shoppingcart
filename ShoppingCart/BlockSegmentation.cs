using System;
using System.Collections.Generic;
using Accord.Math;
using System.Linq;
using ShoppingCart.IO;
using Accord.Controls;
using System.Windows.Forms;

namespace ShoppingCart
{
	public class BlockSegmentation
	{
		private ICharacterMatching blankLineClassifier;

		public BlockSegmentation (ICharacterMatching blankLineClassifier)
		{
			this.blankLineClassifier = blankLineClassifier;
		}

		public IEnumerable<Sample> Segment (IEnumerable<Sample> line)
		{	
			int blockLeftBorder = 0, blockRightBorder = 0;
			var blockTopBorder = (line.First () as CarriageReturn) != null ? (line.First () as CarriageReturn).Row + 1 : 0;
			var blockBottomBorder = (line.Last (l => l is CarriageReturn) as CarriageReturn).Row;

			var lineWithoutCarriageReturnMarkers = line.Where (r => !(r is CarriageReturn)).ToList ();
			int columns = lineWithoutCarriageReturnMarkers.Any () ? lineWithoutCarriageReturnMarkers.First ().Values.Length : 0;
			for (int i = 0; i < columns; i++) {
				var verticalLine = lineWithoutCarriageReturnMarkers.Select (s => s.Values [i]).ToArray ();
				var character = this.blankLineClassifier.Detect (new Sample (verticalLine, ' ', 1.0));
				if (character == '|') {
					blockRightBorder = i;
					int width = blockRightBorder - blockLeftBorder, height = blockBottomBorder - blockTopBorder;
					yield return new CharacterBlock (blockTopBorder, blockLeftBorder, width, height);
					blockLeftBorder = Math.Min (columns - 1, blockRightBorder + 1);
				}
			}				
			yield return new CharacterBlock (blockTopBorder, blockLeftBorder, columns - blockLeftBorder, blockBottomBorder - blockTopBorder);
		}
	}
}

