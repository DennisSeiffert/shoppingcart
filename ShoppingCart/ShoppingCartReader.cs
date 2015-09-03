using System;
using ShoppingCart.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Accord.Math;

namespace ShoppingCart
{
	public class ShoppingCartReader
	{
		private ICharacterMatching digitClassifier;

		private ICharacterMatching newLineClassifier;

		private ICharacterMatching blankLineClassifier;

		public ShoppingCartReader (ICharacterMatching digitClassifier, ICharacterMatching newLineClassifier, 
		                           ICharacterMatching blankLineClassifier)
		{
			this.blankLineClassifier = blankLineClassifier;
			this.newLineClassifier = newLineClassifier;
			this.digitClassifier = digitClassifier;
		}

		public string Read (string shoppingCartImageFilename)
		{
			return this.Read (ImageAdapter.Read (shoppingCartImageFilename));
		}

		public string Read (IEnumerable<Sample> imageRows)
		{			
			var lines = SegmentIntoLines (imageRows).ToList ();

			foreach (var line in lines) {
				int leftBorder = 0;
				int rightBorder = 0;
				for (int i = 0; i < line.First ().Values.Length; i++) {
					var verticalLine = line.Select (s => s.Values [i]).ToArray ();
					var character = this.blankLineClassifier.Recognize (new Sample (verticalLine, 1.0));
					if (character == '\n') {
						rightBorder = i;
						var characterSegment = line.SelectMany (s => s.Values.Skip (leftBorder).Take (rightBorder - leftBorder));


						this.digitClassifier.Recognize (new Sample (characterSegment.ToArray (), 1.0));


						leftBorder = i;
					}
				}

			}

			return string.Empty;
		}

		private IEnumerable<List<Sample>> SegmentIntoLines (IEnumerable<Sample> imageRows)
		{
			var imageDataPerLineWithCarriageReturns = InsertCarriageReturnMarker (imageRows);
//			var imageDataWithoutDuplicateCarriageReturns = imageDataPerLineWithCarriageReturns.SkipWhile ((s, i) => s.Character == '\n' && i > 0
//			                                               && imageDataPerLineWithCarriageReturns.ElementAt (i - 1).Character == '\n');

//			var width = 100;
//			var bitmap = new Bitmap (width, imageDataPerLineWithCarriageReturns.Count ());
//			using (var g = Graphics.FromImage (bitmap)) {
//				for (int i = 0; i < imageDataPerLineWithCarriageReturns.Count (); i++) {
//					if (imageDataPerLineWithCarriageReturns.ElementAt (i) is CarriageReturn) {
//						g.DrawLine (new Pen (new SolidBrush (Color.Red)), 0, i, width, i);
//					}
//				}
//				g.Flush (System.Drawing.Drawing2D.FlushIntention.Sync);
//			}
//			bitmap.Save ("linesegmentation.jpg");

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

