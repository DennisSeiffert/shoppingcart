using Accord.Neuro;
using Accord.Neuro.ActivationFunctions;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using AForge.Neuro;
using AForge.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart
{
	public class Program
	{
		public static void Main (string[] args)
		{
			var sigmoid = new SigmoidFunction ();
			var network = new ActivationNetwork (sigmoid, 64, 15, 10);
			new NguyenWidrow (network).Randomize ();
			var learning = new BackPropagationLearning (network);
           
			foreach (var sample in Read(@"../resources/optdigits.tra")) {
				double[] expectedResult = new double[10];
				expectedResult [sample.Digit] = 1.0;
				var error = learning.Run (sample.Values, expectedResult);
				Console.Out.WriteLine ("Error: {0}", error);
			}
            

			network.Save ("digitRecognition.ann");

			foreach (var testSample in Read(@"../resources/optdigits.tes")) {
				var result = network.Compute (testSample.Values);

				Console.Out.WriteLine ("expected result: {0}", testSample.Digit);
				var actualDigit = result.ToList ().IndexOf (result.Max ());
				Console.Out.WriteLine ("actual: {0}", actualDigit);
			}

			//var result = network.Compute(new[] { 2.0, 2.0 });
			Console.In.ReadLine ();
		}

		public static IEnumerable<Sample> Read (string filename)
		{
			foreach (var line in File.ReadLines(filename)) {
				yield return Convert (line);
			}
		}

		public static Sample Convert (string line)
		{
			return new Sample (line.Split (new []{ ',' }, StringSplitOptions.RemoveEmptyEntries).Select (d => double.Parse (d)).ToArray ());
		}
	}
}
