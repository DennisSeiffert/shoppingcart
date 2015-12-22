
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using ShoppingCart;
using Should;

namespace ShoppingCartTests
{
public class DeepNeuralNetworkTests
{
		[Test ()]
		public void ShouldBeAbleToCreateTestInstance ()
		{
			var trainedNet = File.ReadAllText ("trainedNet.nettra");
			var testFilename = "/home/dennis/PycharmProjects/untitled/CharactarRepository/arial-70.jpg";			
			var network = new DeepNeuralNetwork(trainedNet);
			var image = new Bitmap(testFilename);
			double[,] matrix;
		    new Accord.Imaging.Converters.ImageToMatrix().Convert(image, out matrix);
			double prob;
			var result = network.Detect(Sample.From2dMatrix(matrix), out prob);										
			result.ShouldEqual ('f');
		}	
}	
}
