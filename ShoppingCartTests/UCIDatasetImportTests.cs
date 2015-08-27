﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Should;
using NUnit.Framework;

namespace ShoppingCartTests
{
    [TestFixture]
    public class UCIDatasetImportTests
    {
        [Test]
        public void ShouldTransformUCIDigitInputFileFormatToSamples()
        {
            var result = ShoppingCart.Program.Convert(@"0,0,23,34,6");

            result.Values.Length.ShouldEqual(4);
            result.Values[0].ShouldEqual(0.0);
            result.Values[1].ShouldEqual(0.0);
            result.Values[2].ShouldEqual(23.0);
            result.Values[3].ShouldEqual(34.0);

            result.Digit.ShouldEqual(6);
        }
    }
}
