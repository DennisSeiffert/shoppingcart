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
			line.Add (new Sample (new double[]{ 0.0, 1.0, 0.0 }, ' ', 1.0));
			line.Add (new Sample (new double[]{ 0.0, 1.0, 0.0 }, ' ', 1.0));

			var result = this.CreateBlocksFromLine (line);

			result.Count ().ShouldEqual (3);
			result.First ().Values.Sum ().ShouldEqual (2.0);
			result.ElementAt (1).ShouldBeType<BlankLine> ();
			(result.ElementAt (1) as BlankLine).Column.ShouldEqual (1);
			result.Last ().Values.Sum ().ShouldEqual (2.0);
		}

		[Test]
		public void ShouldCreateBlocksFromLineWithCarriageReturns ()
		{
			var line = new List<Sample> ();
			line.Add (new CarriageReturn (0));
			line.Add (new Sample (new double[]{ 0.0, 1.0, 0.0 }, ' ', 1.0));
			line.Add (new Sample (new double[]{ 0.0, 1.0, 0.0 }, ' ', 1.0));
			line.Add (new CarriageReturn (3));

			var result = CreateBlocksFromLine (line);

			result.Count ().ShouldEqual (3);
			result.First ().Values.Sum ().ShouldEqual (2.0);
			result.ElementAt (1).ShouldBeType<BlankLine> ();
			(result.ElementAt (1) as BlankLine).Column.ShouldEqual (1);
			result.Last ().Values.Sum ().ShouldEqual (2.0);
		}

		[Test]
		public void ShouldCreateEmptyBlockWhenLineContainsCarriageReturnsOnly ()
		{
			var line = new List<Sample> ();
			line.Add (new CarriageReturn (0));
			line.Add (new CarriageReturn (1));

			var result = CreateBlocksFromLine (line);

			result.Count ().ShouldEqual (1);
			result.First ().Values.Sum ().ShouldEqual (0.0);
		}

		private IEnumerable<Sample> CreateBlocksFromLine (List<Sample> line)
		{
			var blanklineMatching = Substitute.For<ICharacterMatching> ();
			blanklineMatching.Recognize (Arg.Any<Sample> ()).Returns ((NSubstitute.Core.CallInfo arg) => {
				var sample = arg.Arg<Sample> ();
				if (sample.Values.Sum () == sample.Values.Length)
					return '|';
				return ' ';
			});
			var sut = new BlockSegmentation (blanklineMatching);
			var result = sut.Segment (line);
			return result;
		}
	}
}

