using System;
using System.Collections.Generic;

public class TargetPlayerGambitImpl : Gambit
{
	private Features feature;
	private PlayerTypes type;
	private PlayerAdjectives adjective;

	Player gambitPlayer;

	public TargetPlayerGambitImpl (Features pmFeature, PlayerTypes pmType, PlayerAdjectives pmAdjective, Player pmGambitPlayer)
	{
		feature = pmFeature;
		type = pmType;
		adjective = pmAdjective;
		gambitPlayer = pmGambitPlayer;
	}

	#region Gambit implementation

	public void Process ()
	{
		List<Player> players = PlayerSpooler.instance.GetPlayersByPlayerType (type); 
		Player selected = null;

		switch (feature) {
		case Features.ANY:
			break;
		case Features.LOWEST_HEALTH:
			HpPlayerSelector hpPlayerSelector = new HpPlayerSelector (players, AmountSpecyfication.LOWEST);
			selected = hpPlayerSelector.GetPlayer ();
			break;
		case Features.LOWEST_ARMOUR:
			AcPlayerSelector acPlayerSelector = new AcPlayerSelector (players, AmountSpecyfication.LOWEST);
			selected = acPlayerSelector.GetPlayer ();
			break;
		case Features.LOWEST_LEVEL:
			break;
		case Features.HIGHEST_ARMOUR:
			AcPlayerSelector acPlayerSelector2 = new AcPlayerSelector (players, AmountSpecyfication.HIGHEST);
			selected = acPlayerSelector2.GetPlayer ();
			break;
		case Features.HIGHEST_HEALTH:
			HpPlayerSelector hpPlayerSelector2 = new HpPlayerSelector (players, AmountSpecyfication.LOWEST);
			selected = hpPlayerSelector2.GetPlayer ();
			break;
		case Features.HIGHEST_LEVEL:
			break;
		case Features.SPELLCASTER:
			break;
		case Features.HEALER:
			break;
		}


		gambitPlayer.TargetPlayer = selected;
	}

	public bool Evaluate ()
	{
		//No requirements I can think of at this point
		return true;
	}

	#endregion
}


