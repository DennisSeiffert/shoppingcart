using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart
{
	public class Sample
	{
		double maxValue;

		public Sample (double[] values, double maxValue) : this (values.Take (values.Length - 1).ToArray (),
			                                                        char.Parse (values.Last ().ToString ()), maxValue)
		{			
		}

		public Sample (char character) : this (new double[]{ }, character, 0.0)
		{
		}

		public Sample (double[] values, char character, double maxValue)
		{
			this.maxValue = maxValue;
			this.Values = values;
			this.Normalize ();
			this.Character = character;
		}

		public double[] Values { get; private set; }

		public char Character { get; private set; }

		void Normalize ()
		{			
			if (this.maxValue > 0.0) {
				this.Values = this.Values.Select (v => v / this.maxValue).ToArray ();	
			}
		}
	}
}
