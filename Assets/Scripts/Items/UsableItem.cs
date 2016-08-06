using UnityEngine;
using System.Collections.Generic;

public class UsableItem : Item {

	public UsableItem (string pmName, List<EquipementTypes> pmTypes, SelfEffect pmSelfEffect)
	{
		mName = pmName;
		mEquipementTypes = pmTypes;
		mSelfEffect = pmSelfEffect;
	}

	SelfEffect mSelfEffect;

	public void Use(Player pmTarget)
	{
		mSelfEffect.Resolve (pmTarget);
	}

}
