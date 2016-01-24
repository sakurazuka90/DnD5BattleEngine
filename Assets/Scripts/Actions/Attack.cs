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


	public void resolveHit (Player pmAttacker, Player pmDefender)
	{
		if (mIsActive)
			ResolveActiveHit (pmAttacker, pmDefender, false);
		else
			ResolvePassiveHit (pmAttacker, pmDefender);
	}

}
