using System;
using System.Collections.Generic;

public abstract class AbstractAmountParentSelector : PlayerSelector
{
	protected List<Player> players;
	protected AmountSpecyfication amount;


	public AbstractAmountParentSelector ()
	{
	}

	#region PlayerSelector implementation

	public Player GetPlayer ()
	{
		Player selected = null;

		foreach (Player player in players) {

			if (selected == null)
				selected = player;
			else{
				if (amount == AmountSpecyfication.HIGHEST) {
					if (GetValue(player) > GetValue(selected))
						selected = player;
				} else if (amount == AmountSpecyfication.LOWEST) {
					if (GetValue(player) < GetValue(selected))
						selected = player;
				}
			}

		}

		return selected;
	}

	protected abstract int GetValue(Player pmPlayer);

	#endregion
}


