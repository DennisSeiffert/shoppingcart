using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart
{
	public class LineSegmentation
	{
		private ICharacterMatching newLineClassifier;

		public LineSegmentation (ICharacterMatching newLineClassifier)
		{
			this.newLineClassifier = newLineClassifier;
		}

		internal IEnumerable<List<Sample>> Segment (IEnumerable<Sample> imageRows)
		{
			var imageDataPerLineWithCarriageReturns = InsertCarriageReturnMarker (imageRows);

			var line = new List<Sample> ();
			bool isInline = true;
			foreach (var row in imageDataPerLineWithCarriageReturns) {
				if (row is CarriageReturn && !isInline) {
					line = new List<Sample> ();
					isInline = true;
					continue;
				}
				if (!(row is CarriageReturn) && isInline) {
					line.Add (row);
				}
				if (row is CarriageReturn && isInline && line.Any ()) {
					yield return line;
					isInline = false;
				}
			}
		}

		private IEnumerable<Sample> InsertCarriageReturnMarker (IEnumerable<Sample> imageDataPerLine)
		{			
			foreach (var line in imageDataPerLine) {								
				if (this.newLineClassifier.Recognize (line) == '\n') {					
					yield return new CarriageReturn ();
					continue;
				}
				yield return line;					
			}
		}
	}
}

