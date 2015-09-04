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
			var lineWithoutCarriageReturnMarkers = line.Where (r => !(r is CarriageReturn)).ToList ();

			double[,] matrix = Matrix.Create (lineWithoutCarriageReturnMarkers.Count (), 0, 0.0);
			int columns = lineWithoutCarriageReturnMarkers.Any () ? lineWithoutCarriageReturnMarkers.First ().Values.Length : 0;
			for (int i = 0; i < columns; i++) {
				var verticalLine = lineWithoutCarriageReturnMarkers.Select (s => s.Values [i]).ToArray ();
				var character = this.blankLineClassifier.Detect (new Sample (verticalLine, ' ', 1.0));
				if (character == '|') {																																						
					yield return Sample.FromIntensityDistribution (matrix);
					yield return new BlankLine (i);

					matrix = Matrix.Create (lineWithoutCarriageReturnMarkers.Count (), 0, 0.0);								
				} else {
					matrix = matrix.InsertColumn (verticalLine);
				}
			}

			if (matrix.Columns () > 0) {
				yield return Sample.FromIntensityDistribution (matrix);
			} else {
				yield return new BlankLine (0);
			}
		}
	}
}

