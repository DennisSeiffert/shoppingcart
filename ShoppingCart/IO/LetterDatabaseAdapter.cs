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

namespace ShoppingCart.IO
{
	public class LetterDatabaseAdapter
	{
		public LetterDatabaseAdapter ()
		{
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

