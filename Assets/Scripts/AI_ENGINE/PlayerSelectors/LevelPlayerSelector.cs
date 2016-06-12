using System;
using System.Collections.Generic;

public class LevelPlayerSelector : AbstractAmountParentSelector
{
	public LevelPlayerSelector (List<Player> pmPlayers, AmountSpecyfication pmAmount)
	{
		players = pmPlayers;
		amount = pmAmount;
	}

	#region implemented abstract members of AbstractAmountParentSelector

	protected override int GetValue (Player pmPlayer)
	{
		return pmPlayer.level;
	}

	#endregion
}


