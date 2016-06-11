using System;
using System.Collections.Generic;

public class AcPlayerSelector : AbstractAmountParentSelector
{

	public AcPlayerSelector (List<Player> pmPlayers, AmountSpecyfication pmAmount)
	{
		players = pmPlayers;
		amount = pmAmount;
	}

	#region implemented abstract members of AbstractAmountParentSelector
	protected override int GetValue (Player pmPlayer)
	{
		return pmPlayer.ac;
	}
	#endregion
}


