using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace Caffe
{
	public static class Wrapper
	{
		[DllImport ("caffe.so")]
		public static extern IntPtr CreateTrainingInstance ();

		[DllImport ("caffe.so")]
		public static extern void ReleaseInstance (ref IntPtr instance);

		[DllImport ("caffe.so")]
		public static extern IntPtr CreateClassifyingInstance (string model_file,
		                                                       string trained_file,
		                                                       string mean_file,
		                                                       string[] labels, 
		                                                       int labelsCount);

		[DllImport ("caffe.so")]
		public static extern string Train (IntPtr pCaffeApiInstance, string solverDefinitionWithNet);

		[DllImport ("caffe.so")]
		public static extern void Classify (IntPtr pCaffeApiInstance, byte[] imageData, int height, int width, IntPtr[] results, IntPtr[] probabilities, int N);
		
		
		[DllImport ("caffe.so")]
		public static extern void ClassifyByFilename (IntPtr pCaffeApiInstance, string filename, IntPtr[] results, IntPtr[] probabilities, int N);
	}
}