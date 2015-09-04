using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Accord.Math;
using System.Linq;
using Accord.Imaging.Filters;
using Accord.Controls;
using Accord.Imaging.Converters;
using AForge.Imaging.Filters;
using System.Windows.Forms;

namespace ShoppingCart.IO
{
	public class ImageAdapter
	{
		public static IEnumerable<Sample> Read (string filename, bool binarizeImage = true)
		{
			int stride;
			Bitmap image = null;
			if (binarizeImage) {
				image = BinarizeImage (filename);	
			} else {
				image = new Bitmap (filename);
			}

			var rgbValues = ExtractImageRawData (image, out stride);
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

		public static Bitmap Write (double[,] matrix)
		{
			// Create the converter to convert the matrix to a image
			MatrixToImage conv = new MatrixToImage (min: 0, max: 1);
			// Declare an image and store the pixels on it
			Bitmap image;
			conv.Convert (matrix, out image);

			// Show the image on screen
			image = new ResizeNearestNeighbor (32, 32).Apply (image);
			// ImageBox.Show (image, PictureBoxSizeMode.Zoom);

			return image;
		}
	}
}

