using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart
{
	public class Sample
	{

		public Sample (double[] values) : this (values.Take (values.Length - 1).ToArray (), char.Parse (values.Last ().ToString ()))
		{			
		}

		public Sample (double[] values, char character)
		{
			this.Values = values;
			this.Normalize ();
			this.Character = character;
		}

		public double[] Values { get; private set; }

		public char Character { get; private set; }

		void Normalize ()
		{
			var maxValue = this.Values.Max ();
			if (maxValue > 0.0) {
				this.Values = this.Values.Select (v => v / maxValue).ToArray ();	
			}
		}
	}
}
