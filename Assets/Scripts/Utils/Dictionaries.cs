using System;
using System.Collections.Generic;

public static class Dictionaries
{
	public static Dictionary<Sizes,String> sizes = new Dictionary<Sizes, string> () {
		{Sizes.TINY, "Tiny"},
		{Sizes.SMALL, "Small"},
		{Sizes.MEDIUM, "Medium"},
		{Sizes.LARGE, "Large"},
		{Sizes.HUGE, "Huge"},
		{Sizes.GARGANTUAN, "Gargantuan"}
	};

	public static Dictionary<Types,String> types = new Dictionary<Types, string> () {
		{Types.HUMANOID, "Humanoid"}
	};
		
}


