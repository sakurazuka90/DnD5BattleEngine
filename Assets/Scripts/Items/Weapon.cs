using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : Item {

	private WeaponType mType;
	private WeaponCategory mCategory;

	private int mDieceType;
	private int mDieceNumber;

	private List<AbilityNames> mUsableAbilities;

	public Weapon(string pmName, WeaponType pmType, WeaponCategory pmCategory, int pmDieceType, int pmDieceNumber)
	{
		mName = pmName;
		mType = pmType;
		mDieceType = pmDieceType;
		mDieceNumber = pmDieceNumber;
		mCategory = pmCategory;
	}

	public WeaponCategory Category{
		get { return this.mCategory; }
	}


}
