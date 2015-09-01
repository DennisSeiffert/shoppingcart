using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart
{
	public class Sample
	{

		public Sample (double[] values)
		{
			this.Values = values.Take (values.Length - 1).ToArray ();
			Normalize ();
			this.Digit = (int)values.Last ();
		}

		public double[] Values { get; private set; }

		public int Digit { get; private set; }

		void Normalize ()
		{
			var maxValue = this.Values.Max ();
			if (maxValue > 0.0) {
				this.Values = this.Values.Select (v => v / maxValue).ToArray ();	
			}
		}
	}
}
