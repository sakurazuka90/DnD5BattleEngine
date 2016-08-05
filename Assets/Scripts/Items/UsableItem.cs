using UnityEngine;
using System.Collections.Generic;

public class UsableItem : Item {

	public UsableItem (string pmName, List<EquipementTypes> pmTypes)
	{
		mName = pmName;
		mEquipementTypes = pmTypes;
	}

}
