using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : Item {

	private WeaponType mType;

	private int mDieceType;
	private int mDieceNumber;

	private List<AbilityNames> mUsableAbilities;

	public Weapon(string pmName, WeaponType pmType, int pmDieceType, int pmDieceNumber)
	{
		mName = pmName;
		mType = pmType;
		mDieceType = pmDieceType;
		mDieceNumber = pmDieceNumber;
	}


}
