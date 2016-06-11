using System;
using System.Collections.Generic;

public class HpPlayerSelector : AbstractAmountParentSelector
{
	public HpPlayerSelector (List<Player> pmPlayers, AmountSpecyfication pmAmount)
	{
		players = pmPlayers;
		amount = pmAmount;
	}

	#region implemented abstract members of AbstractAmountParentSelector

	protected override int GetValue (Player pmPlayer)
	{
		return pmPlayer.hp;
	}

	#endregion
}


