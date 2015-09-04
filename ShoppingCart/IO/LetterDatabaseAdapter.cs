using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Accord.Imaging.Converters;
using Accord.IO;
using System.Text;
using Accord.Controls;
using Accord.Math;
using Accord.Imaging;
using System.IO;

namespace ShoppingCart.IO
{
	public class LetterDatabaseAdapter
	{
		public LetterDatabaseAdapter ()
		{
		}

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
			var splits = line.Split (new []{ ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (splits.Length == 64) {
				splits = splits.Concat (new string[]{ "," }).ToArray ();
			}
			return new Sample (splits.Take (64).Select (d => double.Parse (d)).ToArray (), char.Parse (splits.Last ()), 1.0);
		}

		public string Write (IEnumerable<char> letters, IEnumerable<Font> fonts)
		{
			var csv = new StringBuilder ();
			foreach (var font in fonts) {
				foreach (var letter in letters) {
					Bitmap imageLetter = new Bitmap (32, 32, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
					using (var g = Graphics.FromImage (imageLetter)) {
						g.FillRectangle (Brushes.White, new Rectangle (new Point (0, 0), imageLetter.Size));
						g.DrawString (letter.ToString (), font, Brushes.Black, 0, 0);
					}

					// ImageBox.Show (imageLetter);
					double[,] imageMatrix;					 
					new ImageToMatrix ().Convert (imageLetter, out imageMatrix);
					imageMatrix.ApplyInPlace (v => v > 0.8 ? 1.0 : 0.0);
					var sample = Sample.FromIntensityDistribution (imageMatrix);
					sample = new Sample (sample.Values, letter, 1.0);

					if (sample.Values.Sum () > 0.0) {
						csv.AppendLine (string.Format ("{0},{1}", string.Join (",", sample.Values), letter.ToString ()));	
					}
				}
			}

			return csv.ToString ();
		}
	}
}

