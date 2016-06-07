using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Weapon : Item {

	private WeaponType mType;
	private WeaponCategory mCategory;

	private int mDieceType;
	private int mDieceNumber;
	private Color mSelectedColor = new Color (67.0f / 255.0f, 64.0f / 255.0f, 1.0f, 1.0f); // BLUE
	private Color mDeselectedColor = new Color (1.0f,1.0f,1.0f,100.0f/255.0f); //Default white

	public int rangeNormal = 0;
	public int rangeLong = 0;

	public static Weapon unarmed = new Weapon ("Unarmed", WeaponType.MELEE,WeaponCategory.SIMPLE,3,1,new List<EquipementTypes>());

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
		rangeNormal = 0;
		rangeLong = 0;
	}

	public Weapon(string pmName, WeaponType pmType, WeaponCategory pmCategory, int pmDieceType, int pmDieceNumber, List<EquipementTypes> pmTypes, int pmRangeNormal, int pmRangeLong)
	{
		mName = pmName;
		mType = pmType;
		mDieceType = pmDieceType;
		mDieceNumber = pmDieceNumber;
		mCategory = pmCategory;
		mEquipementTypes = pmTypes;
		rangeNormal = pmRangeNormal;
		rangeLong = pmRangeLong;
	}

	public WeaponCategory Category{
		get { return this.mCategory; }
	}

	public WeaponType Type{
		get { return this.mType; }
	}

	public override void Equip(Player pmPlayer, bool pmUpdateUi)
	{
		pmPlayer.equippedWeaponAttack = new Attack (mName, this);
		if(pmUpdateUi)
			PlayerSpooler.UpdateWeapon ();
	}

	public override void UnEquip(Player pmPlayer, bool pmUpdateUi)
	{
		pmPlayer.equippedWeaponAttack = new Attack ("Unarmed", unarmed);
		if(pmUpdateUi)
			PlayerSpooler.UpdateWeapon ();
	}


}
