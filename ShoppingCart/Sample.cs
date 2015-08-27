using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart
{
    public class Sample
    {

        public Sample(double[] values)
        {
            this.Values = values.Take(values.Length - 1).ToArray();
            this.Digit = (int)values.Last();
        }

        public double[] Values { get; private set; }

        public int Digit { get; private set; }
    }
}
