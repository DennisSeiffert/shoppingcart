using System;

namespace ShoppingCart
{
	public interface ICharacterMatching
	{
		char Detect (Sample sample);

		char Detect (Sample sample, out double probability);
	}

}

