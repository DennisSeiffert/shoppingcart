using System;

namespace ShoppingCart
{
	public class BlankLineClassifier : NewLineClassifier
	{
		protected override char OnRecognize (Sample sample)
		{
			if (base.OnRecognize (sample) == '\n') {
				return '|';
			}
			return ' ';
		}
	}
}

