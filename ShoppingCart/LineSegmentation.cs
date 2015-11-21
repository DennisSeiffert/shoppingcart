using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Visualizations;

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
			var imageDataPerLineWithCarriageReturns = InsertCarriageReturnMarker (imageRows).ToList ();

			var line = new List<Sample> ();
			bool isInline = true;
			int rowIndex = -1;
			foreach (var row in imageDataPerLineWithCarriageReturns) {
				rowIndex++;
				if (!(row is CarriageReturn) && !isInline) {
					line = new List<Sample> ();
					if (rowIndex > 0) {						
						line.Add (imageDataPerLineWithCarriageReturns [rowIndex - 1]);						
					}
					line.Add (row);					
					isInline = true;
					continue;
				}
				if (!(row is CarriageReturn) && isInline) {
					line.Add (row);
				}
				if (row is CarriageReturn && isInline && line.Any ()) {
					line.Add (row);
					yield return line;
					isInline = false;
				}
			}
			if (isInline && line.Any ())
				line.Add (new CarriageReturn (rowIndex));
			yield return line;
		}

		private IEnumerable<Sample> InsertCarriageReturnMarker (IEnumerable<Sample> imageDataPerLine)
		{	
			int rowCounter = 0;
			var intensitiesPerRow = new List<double> ();
			foreach (var line in imageDataPerLine) {	
				intensitiesPerRow.Add (line.Values.Sum (v => 1.0 - v));

//				rowCounter++;
//				if (this.newLineClassifier.Detect (line) == '\n') {					
//					yield return new CarriageReturn (rowCounter - 1);
//					continue;
//				}
//				yield return line;					
			}
				
			Histogram histogram = new Histogram (intensitiesPerRow.ToArray ());
			int maxBin = histogram.Values.ToList ().IndexOf (histogram.Values.Max ());
			double lineThreshold = maxBin > -1 ? histogram.Bins [maxBin].Range.Max : 0.0;
			foreach (var line in imageDataPerLine) {	
				rowCounter++;
				if (intensitiesPerRow.ElementAt (rowCounter - 1) <= lineThreshold) {					
					yield return new CarriageReturn (rowCounter - 1);
					continue;
				}
				yield return line;					
			}
		}
	}
}

