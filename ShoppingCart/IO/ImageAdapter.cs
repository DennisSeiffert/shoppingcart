using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Accord.Math;
using System.Linq;

namespace ShoppingCart.IO
{
	public class ImageAdapter
	{
		public static IEnumerable<Sample> Read (string filename)
		{
			int stride;
			var rgbValues = ExtractImageRawData (filename, out stride);
			return ConvertToGrayScaleSamples (rgbValues, stride);
		}

		private static byte[] ExtractImageRawData (string filename, out int stride)
		{
			Bitmap myImage = new Bitmap (filename);
			byte[] rgbValues = null;
			BitmapData data = myImage.LockBits (new Rectangle (0, 0, myImage.Width, myImage.Height), ImageLockMode.ReadOnly, myImage.PixelFormat);
			try {
				IntPtr ptr = data.Scan0;
				int bytes = Math.Abs (data.Stride) * myImage.Height;
				stride = data.Stride;
				rgbValues = new byte[bytes];
				Marshal.Copy (ptr, rgbValues, 0, bytes);
			} finally {				
				myImage.UnlockBits (data);
			}
			return rgbValues;
		}

		private static IEnumerable<Sample> ConvertToGrayScaleSamples (byte[] rgbValues, int stride)
		{
			var samples = new List<Sample> ();
			var grayScaleValues = new List<double> ();
			for (int i = 0; i < rgbValues.Length; i += 3) {
				grayScaleValues.Add (0.333333 * (rgbValues [i] + rgbValues [i + 1] + rgbValues [i + 2]));
				if (i > 0 && i % stride == 0) {
					samples.Add (new Sample (grayScaleValues.ToArray (), ' '));
					grayScaleValues.Clear ();
				}
			}
			return samples;
		}
	}
}

