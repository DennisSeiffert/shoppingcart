﻿using Accord.Neuro;
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
			var digitClassifier = new OptDigitClassifier (OptDigitDatabaseAdapter.Read (@"../resources/optdigits.tra"));
			//digitClassifier.Save ("digitRecognition.ann");

			foreach (var testSample in OptDigitDatabaseAdapter.Read(@"../resources/optdigits.tes")) {
				var digit = (digitClassifier as ICharacterMatching).Recognize (testSample);
				Console.Out.WriteLine ("expected result: {0}", testSample.Digit);
				Console.Out.WriteLine ("actual: {0}", digit);
			}
		}


	}
}
