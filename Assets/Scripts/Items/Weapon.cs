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
	}

	public WeaponCategory Category{
		get { return this.mCategory; }
	}

	public override void Equip(Player pmPlayer)
	{
		pmPlayer.equippedWeaponAttack = new Attack (mName, this);
		PlayerSpooler.UpdateWeapon ();
	}

	public override void UnEquip(Player pmPlayer)
	{
		pmPlayer.equippedWeaponAttack = new Attack ("Unarmed", unarmed);
		PlayerSpooler.UpdateWeapon ();
	}


}
