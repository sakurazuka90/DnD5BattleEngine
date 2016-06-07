using System;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{

	int mBaseAc;
	int mMaxDex;

	public Armor (string pmName, List<EquipementTypes> pmTypes, int pmBaseAc, int pmMaxDex)
	{
		mName = pmName;
		mEquipementTypes = pmTypes;
		mBaseAc = pmBaseAc;
		mMaxDex = pmMaxDex;
	}

	public override void Equip(Player pmPlayer, bool pmUpdateUi = true)
	{
		pmPlayer.mBasicAc = mBaseAc;
		pmPlayer.mMaxDexMod = mMaxDex;

		pmPlayer.UpdateAc ();

		if(pmUpdateUi)
			PlayerSpooler.UpdateAc ();

	}

	public override void UnEquip(Player pmPlayer, bool pmUpdateUi = true)
	{
		pmPlayer.mBasicAc = 10;
		pmPlayer.mMaxDexMod = 6;

		pmPlayer.UpdateAc ();

		if(pmUpdateUi)
			PlayerSpooler.UpdateAc ();

	}
}


