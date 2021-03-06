﻿using UnityEngine;
using System.Collections;

public class Attack : AbstractAction{

	private bool mIsActive;
	private string mName;

	public Attack(string pmName, Weapon pmWeapon)
	{
		mName = pmName;
		mIsActive = true;
		mWeapon = pmWeapon;

		if (WeaponType.MELEE == pmWeapon.WeaponType)
			mAbility = AbilityNames.STRENGTH;
		else
			mAbility = AbilityNames.DEXTERITY;

	}

	public string Name{
		get{ return mName; }
	}


	public void resolveHit (Player pmAttacker, Player pmDefender, bool pmIsMissleShoot = false)
	{
		if (mIsActive)
			ResolveActiveHit (pmAttacker, pmDefender, false, pmIsMissleShoot);
		else
			ResolvePassiveHit (pmAttacker, pmDefender);
	}

}
