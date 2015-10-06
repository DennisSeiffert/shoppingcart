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
using AForge.Imaging.Filters;

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

		public static double[,] NormalizeBitmap (Bitmap imageLetter)
		{
			// ImageBox.Show (imageLetter);
			var imageMatrix = ConvertToBinaryMatrix (imageLetter);
			imageMatrix = Trim (imageMatrix);
			if (imageMatrix.Columns () * imageMatrix.Rows () == 0) {
				return new double[0, 0];
			}
			Bitmap croppedImage;
			new MatrixToImage ().Convert (imageMatrix, out croppedImage);
			ResizeNearestNeighbor resize = new ResizeNearestNeighbor (32, 32);
			croppedImage = resize.Apply (croppedImage);
			// ImageBox.Show (croppedImage, System.Windows.Forms.PictureBoxSizeMode.Zoom);
			imageMatrix = ConvertToBinaryMatrix (croppedImage);
			return imageMatrix;
		}

		private static Sample Convert (string line)
		{
			var splits = line.Split (new []{ ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (splits.Length == 64) {
				splits = splits.Concat (new string[]{ "," }).ToArray ();
			}
			return new Sample (splits.Take (64).Select (d => double.Parse (d)).ToArray (), char.Parse (splits.Last ()), 1.0);
		}

		public static string Write (IEnumerable<char> letters, IEnumerable<Font> fonts)
		{
			var csv = new StringBuilder ();
			foreach (var font in fonts) {
				foreach (var letter in letters) {
					Bitmap imageLetter = new Bitmap (32, 32, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
					using (var g = Graphics.FromImage (imageLetter)) {
						g.FillRectangle (Brushes.White, new Rectangle (new Point (0, 0), imageLetter.Size));
						g.DrawString (letter.ToString (), font, Brushes.Black, 0, 0);
					}


					var imageMatrix = NormalizeBitmap (imageLetter);
					if (imageMatrix.Rows () * imageMatrix.Columns () > 0) {
						var sample = Sample.FromIntensityDistribution (imageMatrix);
						sample = new Sample (sample.Values, letter, 1.0);

						csv.AppendLine (string.Format ("{0},{1}", string.Join (",", sample.Values), letter.ToString ()));	
					}
				}
			}

			return csv.ToString ();
		}

		private static double[,] Trim (double[,] imageMatrix)
		{				
			while (imageMatrix.Rows () > 0 && imageMatrix.GetRow (0).Sum () == imageMatrix.Columns ()) {
				imageMatrix = imageMatrix.RemoveRow (0);
			}
			int lastRow = imageMatrix.Rows () - 1;
			while (imageMatrix.Rows () > 0 && imageMatrix.GetRow (lastRow).Sum () == imageMatrix.Columns ()) {
				imageMatrix = imageMatrix.RemoveRow (lastRow);
				lastRow = imageMatrix.Rows () - 1;
			}

			while (imageMatrix.Columns () > 0 && imageMatrix.GetColumn (imageMatrix.Columns () - 1).Sum () == imageMatrix.Rows ()) {
				imageMatrix = imageMatrix.RemoveColumn (imageMatrix.Columns () - 1);
			}
			while (imageMatrix.Columns () > 0 && imageMatrix.GetColumn (0).Sum () == imageMatrix.Rows ()) {
				imageMatrix = imageMatrix.RemoveColumn (0);
			}
			return imageMatrix;
		}

		private static double[,] ConvertToBinaryMatrix (Bitmap imageLetter)
		{
			double[,] imageMatrix;
			new ImageToMatrix ().Convert (imageLetter, out imageMatrix);
			imageMatrix.ApplyInPlace (v => v > 0.82 ? 1.0 : 0.0);
			return imageMatrix;
		}
	}
}

