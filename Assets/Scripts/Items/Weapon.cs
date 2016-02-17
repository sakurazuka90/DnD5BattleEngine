using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : Item {

	private WeaponType mType;
	private WeaponCategory mCategory;

	private int mDieceType;
	private int mDieceNumber;

	public int DieceNumber{
		get { return this.mDieceNumber; }
	}

	public int DieceType{
		get { return this.mDieceType; }
	}

	private List<AbilityNames> mUsableAbilities;

	public Weapon(string pmName, WeaponType pmType, WeaponCategory pmCategory, int pmDieceType, int pmDieceNumber, List<EquipementTypes> pmTypes)
	{
		mName = pmName;
		mType = pmType;
		mDieceType = pmDieceType;
		mDieceNumber = pmDieceNumber;
		mCategory = pmCategory;
		mEquipementTypes = pmTypes;
	}

	public WeaponCategory Category{
		get { return this.mCategory; }
	}


}
