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
		gambitPlayer.equippedWeaponAttack.resolveHit (gambitPlayer, gambitPlayer.TargetPlayer);

	}




	public bool Evaluate ()
	{
		return (gambitPlayer.mTotalStandardActions > 0) && gambitPlayer.equippedWeaponAttack.IsTargerInRange (gambitPlayer) && !gambitPlayer.TargetPlayer.isDead;
	}

	#endregion
}


