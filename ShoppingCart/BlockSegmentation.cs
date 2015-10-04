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

		public IList<CharacterBlock> MergeNeighboredBlocks (IList<CharacterBlock> blocks)
		{
			for (int i = 1; i < blocks.Count; i++) {
				var current = blocks [i];
				var previous = blocks [i - 1];
				if (previous.Row == current.Row && current.Column - previous.Column + previous.Width < 5) {
					blocks [i] = new CharacterBlock (previous.Row, previous.Column, current.Column - previous.Column + current.Width, current.Height);
					blocks.RemoveAt (i - 1);
					--i;
				}
			}

			return blocks;
		}

		public IList<CharacterBlock> RemoveEmptyBlocks (IList<CharacterBlock> blocks)
		{			
			for (int i = 0; i < blocks.Count; i++) {
				if (blocks [i].IsEmpty ()) {
					blocks.RemoveAt (i);
					--i;
				}
			}
			return blocks;
		}

		public IList<CharacterBlock> RemoveSkinnyBlocks (IList<CharacterBlock> blocks)
		{
			for (int i = 0; i < blocks.Count; i++) {
				if (blocks [i].Width < 2) {
					blocks.RemoveAt (i);
					--i;
				}
			}
			return blocks;
		}

		public IEnumerable<CharacterBlock> Segment (IEnumerable<Sample> line)
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

