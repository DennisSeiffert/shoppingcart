using System;
using NUnit.Framework;
using ShoppingCart;
using System.Collections.Generic;
using NSubstitute;
using Should;
using System.Linq;

namespace ShoppingCartTests
{
	[TestFixture]
	public class LineSegmentationTests
	{
		public LineSegmentationTests ()
		{
			
		}

		[Test]
		public void ShouldAggregateLinesWithCarriageReturns ()
		{
			var lines = new List<Sample> ();
			var blankLine = new Sample (new []{ 1.0, 1.0, 1.0 }, '\n', 1.0);
			var nonBlankLine = new Sample (new []{ 0.33, 0.1, 0.2 }, 'a', 1.0);
			lines.Add (nonBlankLine);
			lines.Add (nonBlankLine);
			lines.Add (nonBlankLine);
			lines.Add (blankLine);
			lines.Add (blankLine);
			lines.Add (nonBlankLine);
			lines.Add (nonBlankLine);
			lines.Add (blankLine);
			lines.Add (nonBlankLine);
			lines.Add (blankLine);
			lines.Add (blankLine);
			lines.Add (blankLine);
			lines.Add (blankLine);
			lines.Add (blankLine);
			lines.Add (blankLine);
			lines.Add (nonBlankLine);
			lines.Add (nonBlankLine);
			lines.Add (nonBlankLine);

			var sut = new LineSegmentation (Substitute.For<ICharacterMatching> ());
			IEnumerable<List<Sample>> result = sut.Segment (lines);

			result.Count ().ShouldEqual (4);
			result.ElementAt (0).Count ().ShouldEqual (4);
			result.ElementAt (1).Count ().ShouldEqual (4);
			result.ElementAt (2).Count ().ShouldEqual (3);
			result.ElementAt (3).Count ().ShouldEqual (5);
			result.ElementAt (3).Last ().ShouldBeType<CarriageReturn> ();
		}
	}
}

