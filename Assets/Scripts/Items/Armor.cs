using System;
using System.Collections.Generic;

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

	public override void Equip(Player pmPlayer)
	{
		pmPlayer.mBasicAc = mBaseAc;
		pmPlayer.mMaxDexMod = mMaxDex;

		pmPlayer.UpdateAc ();
	}
}


