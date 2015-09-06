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
			line.Add (new CarriageReturn (2));

			var result = this.CreateBlocksFromLine (line);

			result.Count ().ShouldEqual (2);
			result.ElementAt (0).ShouldBeType<CharacterBlock> ();
			(result.ElementAt (0) as CharacterBlock).Column.ShouldEqual (0);
			result.ElementAt (1).ShouldBeType<CharacterBlock> ();
			(result.ElementAt (1) as CharacterBlock).Column.ShouldEqual (2);
			(result.ElementAt (1) as CharacterBlock).Row.ShouldEqual (0);
			(result.ElementAt (1) as CharacterBlock).Width.ShouldEqual (1);
			(result.ElementAt (1) as CharacterBlock).Height.ShouldEqual (2);
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

			result.Count ().ShouldEqual (2);
			result.ElementAt (0).ShouldBeType<CharacterBlock> ();
			(result.ElementAt (0) as CharacterBlock).Column.ShouldEqual (0);
			result.ElementAt (1).ShouldBeType<CharacterBlock> ();
			(result.ElementAt (1) as CharacterBlock).Column.ShouldEqual (2);
			(result.ElementAt (1) as CharacterBlock).Row.ShouldEqual (1);
			(result.ElementAt (1) as CharacterBlock).Width.ShouldEqual (1);
			(result.ElementAt (1) as CharacterBlock).Height.ShouldEqual (2);
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

		[Test]
		public void ShouldMergeNeighboredBlocks ()
		{
			var blocks = new List<CharacterBlock> ();
			blocks.Add (new CharacterBlock (0, 0, 2, 2));
			blocks.Add (new CharacterBlock (0, 2, 2, 2));
			blocks.Add (new CharacterBlock (0, 10, 2, 2));
			blocks.Add (new CharacterBlock (0, 16, 2, 2));

			var sut = new BlockSegmentation (Substitute.For<ICharacterMatching> ());
			var result = sut.MergeNeighboredBlocks (blocks);

			result.Count.ShouldEqual (3);
			result [0].Row.ShouldEqual (0);
			result [0].Column.ShouldEqual (0);
			result [0].Width.ShouldEqual (4);
			result [0].Height.ShouldEqual (2);

			result [1].Row.ShouldEqual (0);
			result [1].Column.ShouldEqual (10);
			result [1].Width.ShouldEqual (2);
			result [1].Height.ShouldEqual (2);

			result [2].Row.ShouldEqual (0);
			result [2].Column.ShouldEqual (16);
			result [2].Width.ShouldEqual (2);
			result [2].Height.ShouldEqual (2);
		}

		[Test]
		public void ShouldRemoveEmptyBlocks ()
		{
			var blocks = new List<CharacterBlock> ();
			blocks.Add (new CharacterBlock (0, 0, 0, 2));
			blocks.Add (new CharacterBlock (0, 2, 2, 0));
			blocks.Add (new CharacterBlock (0, 10, 2, 2));
			blocks.Add (new CharacterBlock (0, 16, 0, 2));

			var sut = new BlockSegmentation (Substitute.For<ICharacterMatching> ());
			var result = sut.RemoveEmptyBlocks (blocks);

			result.Count.ShouldEqual (1);
			result [0].Row.ShouldEqual (0);
			result [0].Column.ShouldEqual (10);
			result [0].Width.ShouldEqual (2);
			result [0].Height.ShouldEqual (2);
		}

		private IEnumerable<Sample> CreateBlocksFromLine (List<Sample> line)
		{
			var blanklineMatching = Substitute.For<ICharacterMatching> ();
			blanklineMatching.Detect (Arg.Any<Sample> ()).Returns ((NSubstitute.Core.CallInfo arg) => {
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

