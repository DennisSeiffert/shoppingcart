using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Accord.Math;
using System.Linq;
using Accord.Imaging.Filters;
using Accord.Controls;

namespace ShoppingCart.IO
{
	public class ImageAdapter
	{
		public static IEnumerable<Sample> Read (string filename)
		{
			int stride;
			var binaryImage = BinarizeImage (filename);
			var rgbValues = ExtractImageRawData (binaryImage, out stride);
			return ConvertToGrayScaleSamples (rgbValues, stride);
		}

		private static byte[] ExtractImageRawData (Bitmap binaryImage, out int stride)
		{			
			// Show on the screen
			// ImageBox.Show (result);

			byte[] rgbValues = null;
			BitmapData data = binaryImage.LockBits (new Rectangle (0, 0, binaryImage.Width, binaryImage.Height), ImageLockMode.ReadOnly, binaryImage.PixelFormat);
			try {
				IntPtr ptr = data.Scan0;
				int bytes = Math.Abs (data.Stride) * binaryImage.Height;
				stride = data.Stride;
				rgbValues = new byte[bytes];
				Marshal.Copy (ptr, rgbValues, 0, bytes);
			} finally {				
				binaryImage.UnlockBits (data);
			}
			return rgbValues;
		}

		private static Bitmap BinarizeImage (string filename)
		{
			Bitmap myImage = new Bitmap (filename);
			var niblack = new SauvolaThreshold ();
			Bitmap result = niblack.Apply (myImage);
			return result;
		}

		private static IEnumerable<Sample> ConvertToGrayScaleSamples (byte[] rgbValues, int stride)
		{
			var samples = new List<Sample> ();
			var grayScaleValues = new List<double> ();
			for (int i = 0; i < rgbValues.Length; i += 3) {
				grayScaleValues.Add (0.333333 * (rgbValues [i] + rgbValues [i + 1] + rgbValues [i + 2]));
				if (i > 0 && i % stride == 0) {
					samples.Add (new Sample (grayScaleValues.ToArray (), ' ', 255.0));
					grayScaleValues.Clear ();
				}
			}
			return samples;
		}
	}
}

