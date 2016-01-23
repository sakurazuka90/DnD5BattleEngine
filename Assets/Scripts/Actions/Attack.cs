using UnityEngine;
using System.Collections;

public class Attack : AbstractAction{

	private bool mIsActive;
	private string mName;

	public Attack(string pmName, Weapon pmWeapon)
	{
		mName = pmName;
		mIsActive = true;
		mWeapon = pmWeapon;
	}

	public string Name{
		get{ return mName; }
	}


	public bool resolveHit (Player pmAttacker, Player pmDefender)
	{
		if (mIsActive)
			return ResolveActiveHit (pmAttacker, pmDefender);
		else
			return ResolvePassiveHit (pmAttacker, pmDefender);
	}

}
