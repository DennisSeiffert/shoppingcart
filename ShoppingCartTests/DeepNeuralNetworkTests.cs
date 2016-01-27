
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
	[TestCase("/commondata/Programming/Repositories/shoppingcart/ShoppingCartTests/B_testImage.jpg", 'B')]	
		[TestCase("/home/dennis/PycharmProjects/untitled/CharacterRepository/arial-31.jpg", 'f')]
		[TestCase("/home/dennis/PycharmProjects/untitled/CharacterRepository/arial-32.jpg", 'g')]
		[TestCase("/home/dennis/PycharmProjects/untitled/CharacterRepository/Courier_New-32.jpg", 'g')]			
		public void ShouldBeAbleToCreateTestInstance (string testImage, char character)
		{
			var trainedNet = File.ReadAllText ("/commondata/Programming/Repositories/shoppingcart/ShoppingCart/trainedNet.nettra");
			var testFilename = testImage;			
			var network = new DeepNeuralNetwork(trainedNet);
			var image = new Bitmap(testFilename);
			double[,] matrix;
		    new Accord.Imaging.Converters.ImageToMatrix().Convert(image, out matrix);
			double prob;
			var result = network.Detect(Sample.From2dMatrix(matrix), out prob);										
			result.ShouldEqual (character);
		}	
}	
}
