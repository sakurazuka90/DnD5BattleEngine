using UnityEngine;
using System.Collections;

public class Ability{

	private  int mValue;
	private  int mModifier;

	public Ability(int pmValue)
	{
		mValue = pmValue;

		int lvMod = pmValue - 10;

		if (lvMod % 2 == 0)
			mModifier = lvMod / 2;
		else
			mModifier = (lvMod - 1) / 2;

	}

	public int Modifier{
		get{ return mModifier; }
	}



	
}
