﻿using System;
using AForge.Neuro;
using Accord.Neuro;
using AForge.Neuro.Learning;
using System.Collections.Generic;
using System.Linq;
using Accord.Controls;
using System.Windows.Forms;

namespace ShoppingCart
{
	public class DigitClassifier : NeuralNetwork, ICharacterMatching
	{
		public DigitClassifier (IEnumerable<Sample> samples) : base (samples, "0123456789".ToCharArray (), 64, 15, 10)
		{
		}

		public DigitClassifier (string filename) : base (filename)
		{			
		}

		#region ICharacterMatching implementation

		char ICharacterMatching.Detect (Sample sample)
		{
			double prob = 0.0;
			return (this as ICharacterMatching).Detect (sample, out prob);
		}


		char ICharacterMatching.Detect (Sample sample, out double probability)
		{
			var result = this.network.Compute (sample.Values);
			probability = result.Max ();
			//			if (maxProbability < 0.2) {
			//				return ' ';
			//			}

			var recognizedDigit = result.ToList ().IndexOf (probability);
			return recognizedDigit.ToString ().ToCharArray ().First ();		
		}

		#endregion
	}
}
