using System;

public class AttackGambitImpl:Gambit
{
	Player gambitPlayer;

	public AttackGambitImpl ()
	{
	}

	public Player GambitPlayer{
		set{ gambitPlayer = value; }
	}

	#region Gambit implementation

	public void Process ()
	{
		gambitPlayer.equippedWeaponAttack.
	}

	public bool Evaluate ()
	{
		return gambitPlayer.mTotalStandardActions > 0;
	}

	#endregion
}


