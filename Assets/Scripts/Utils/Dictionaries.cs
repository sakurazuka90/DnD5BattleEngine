using System;
using System.Collections.Generic;

public static class Dictionaries
{
	public static Dictionary<Sizes,string> sizes = new Dictionary<Sizes, string> () {
		{Sizes.TINY, "Tiny"},
		{Sizes.SMALL, "Small"},
		{Sizes.MEDIUM, "Medium"},
		{Sizes.LARGE, "Large"},
		{Sizes.HUGE, "Huge"},
		{Sizes.GARGANTUAN, "Gargantuan"}
	};

	public static Dictionary<Types,string> types = new Dictionary<Types, string> () {
		{Types.HUMANOID, "Humanoid"}
	};

	public static Dictionary<AbilityNames,string> abilityShortcuts = new Dictionary<AbilityNames, string> () {
		{AbilityNames.STRENGTH, "STR"},
		{AbilityNames.DEXTERITY, "DEX"},
		{AbilityNames.CONSTITUTION, "CON"},
		{AbilityNames.INTELLIGENCE, "INT"},
		{AbilityNames.WISDOM, "WIS"},
		{AbilityNames.CHARISMA, "CHA"}
	};
		
}


