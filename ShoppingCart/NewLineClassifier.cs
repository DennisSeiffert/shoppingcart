using System;
using System.Linq;
using Accord.Statistics;

namespace ShoppingCart
{
	public class NewLineClassifier : ICharacterMatching
	{
		public NewLineClassifier ()
		{
		}

		#region ICharacterMatching implementation

		char ICharacterMatching.Recognize (Sample sample)
		{	
//			var amountOfBrighterPixels = sample.Values.Where (v => v > 0.6).Count ();
//			var amountOfDarkerPixels = sample.Values.Where (v => v < 0.2).Count ();


			//var variance = sample.Values.Variance (false);
			var deviation = sample.Values.StandardDeviation (false);
			//var mean = sample.Values.Mean ();
			//var entropy = sample.Values.Entropy ();

			// Console.Out.WriteLine ("Variance: {0}, Deviation: {1}, mean: {2}, entropy: {3}", variance, deviation, mean, entropy); 

			//	if (amountOfBrighterPixels > 0 && (amountOfDarkerPixels / amountOfBrighterPixels) < 0.1) {
			if (deviation < 0.1) {
				return '\n';
			}

			return ' ';
		}

		#endregion
	}
}

