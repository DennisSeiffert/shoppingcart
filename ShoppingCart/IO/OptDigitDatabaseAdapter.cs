using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShoppingCart.IO
{
	public class OptDigitDatabaseAdapter
	{
		public static IEnumerable<Sample> Read (string filename)
		{
			return Read (File.ReadLines (filename));
		}

		public static IEnumerable<Sample> Read (IEnumerable<string> lines)
		{
			foreach (var line in lines) {
				yield return Convert (line);
			}
		}

		private static Sample Convert (string line)
		{
			return new Sample (line.Split (new []{ ',' }, StringSplitOptions.RemoveEmptyEntries).Select (d => double.Parse (d)).ToArray ());
		}
	}
}

