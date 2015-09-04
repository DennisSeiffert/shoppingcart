using System;
using NUnit.Framework;
using ShoppingCart;
using System.Collections.Generic;
using NSubstitute;
using System.Linq;
using Should;

namespace ShoppingCartTests
{
	[TestFixture]
	public class BlockSegmentationTests
	{
		public BlockSegmentationTests ()
		{
		}

		[Test]
		public void ShouldCreateBlocksFromLine ()
		{
			var line = new List<Sample> ();
			line.Add (new Sample (new double[]{ 1.0, 0.0, 1.0 }, ' ', 1.0));
			line.Add (new Sample (new double[]{ 1.0, 0.0, 1.0 }, ' ', 1.0));

			var result = this.CreateBlocksFromLine (line);

			result.Count ().ShouldEqual (2);
			result.First ().Values.Sum ().ShouldEqual (2.0);
			result.Last ().Values.Sum ().ShouldEqual (2.0);
		}

		[Test]
		public void ShouldCreateBlocksFromLineWithCarriageReturns ()
		{
			var line = new List<Sample> ();
			line.Add (new CarriageReturn (0));
			line.Add (new Sample (new double[]{ 1.0, 0.0, 1.0 }, ' ', 1.0));
			line.Add (new Sample (new double[]{ 1.0, 0.0, 1.0 }, ' ', 1.0));
			line.Add (new CarriageReturn (3));

			var result = CreateBlocksFromLine (line);

			result.Count ().ShouldEqual (2);
			result.First ().Values.Sum ().ShouldEqual (2.0);
			result.Last ().Values.Sum ().ShouldEqual (2.0);
		}

		private IEnumerable<Sample> CreateBlocksFromLine (List<Sample> line)
		{
			var blanklineMatching = Substitute.For<ICharacterMatching> ();
			blanklineMatching.Recognize (Arg.Any<Sample> ()).Returns ((NSubstitute.Core.CallInfo arg) => {
				var sample = arg.Arg<Sample> ();
				if (sample.Values.Sum () == 0)
					return '|';
				return ' ';
			});
			var sut = new BlockSegmentation (blanklineMatching);
			var result = sut.Segment (line);
			return result;
		}
	}
}

