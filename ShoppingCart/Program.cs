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
using ShoppingCart.IO;

namespace ShoppingCart
{
	public class Program
	{
		public static void Main (string[] args)
		{	
//			var digitClassifier = new DigitClassifier (OptDigitDatabaseAdapter.Read (@"../resources/optdigits.tra"));
			var digitClassifier = new DigitClassifier (LetterDatabaseAdapter.Read (@"../resources/optdigitsblockletters.tra"));
			//digitClassifier.Save ("digitRecognition.ann");

			var letterClassifier = new LetterClassifier (LetterDatabaseAdapter.Read (@"../resources/optletters.tra").Where (s => LetterClassifier.LETTERS.ToCharArray ().Contains (s.Character)));

			var specialCharacterClassifier = new SpecialCharacterClassifier (LetterDatabaseAdapter.Read (@"../resources/optspecialcharacters.tra"));

			foreach (var testSample in OptDigitDatabaseAdapter.Read(@"../resources/optdigits.tes")) {
				var digit = (digitClassifier as ICharacterMatching).Detect (testSample);
				Console.Out.WriteLine ("expected result: {0}", testSample.Character);
				Console.Out.WriteLine ("actual: {0}", digit);
			}

			var shoppingCartReader = new ShoppingCartReader (new CharacterClassifier (digitClassifier, letterClassifier, specialCharacterClassifier), new NewLineClassifier (), new BlankLineClassifier ());
			Console.Out.WriteLine (shoppingCartReader.Read (args [0]));
		}


	}
}
