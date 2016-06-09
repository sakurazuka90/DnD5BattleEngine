using System;
using System.Collections.Generic;

public class TargetPlayerGambitImpl : Gambit
{
	private Features feature;
	private PlayerTypes type;
	private PlayerAdjectives adjective;

	public TargetPlayerGambitImpl (Features pmFeature, PlayerTypes pmType, PlayerAdjectives pmAdjective)
	{
		feature = pmFeature;
		type = pmType;
		adjective = pmAdjective;
	}

	#region Gambit implementation

	public void Process ()
	{
		List<Player> players = PlayerSpooler.instance.GetPlayersByPlayerType (type); 

		switch (feature) {
		case Features.ANY:
			break;
		case Features.LOWEST_HEALTH:
			break;
		case Features.LOWEST_ARMOUR:
			break;
		case Features.LOWEST_LEVEL:
			break;
		case Features.HIGHEST_ARMOUR:
			break;
		case Features.HIGHEST_HEALTH:
			break;
		case Features.HIGHEST_LEVEL:
			break;
		case Features.SPELLCASTER:
			break;
		case Features.HEALER:
			break;
		}

	}

	public bool Evaluate ()
	{
		//No requirements I can think of at this point
		return true;
	}

	#endregion
}


