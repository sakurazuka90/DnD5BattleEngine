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
		if (gambitPlayer.equippedWeaponAttack.IsTargerInRange (gambitPlayer.TargetPlayer)) {
			gambitPlayer.equippedWeaponAttack.resolveHit (gambitPlayer, gambitPlayer.TargetPlayer);
		}
		//gambitPlayer.equippedWeaponAttack.
	}




	public bool Evaluate ()
	{
		return gambitPlayer.mTotalStandardActions > 0;
	}

	#endregion
}


