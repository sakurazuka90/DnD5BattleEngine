using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player {

	private int mInitiativeBonus;

	public GameObject Figurine;

	public Sprite PlayerSprite;

	public string playerName;

	private Dictionary<AbilityNames,Ability> mAbilities;

	private Dictionary<TestsNames,List<Dictionary<BonusConditionNames,int>>> mBonuses;

	public Player ()
	{

	}

	public void setAbility(AbilityNames pmName, int pmScore)
	{
		mAbilities.Remove (pmName);
		mAbilities.Add (pmName, new Ability(pmScore));
	}

	public int rollInitiative()
	{
		return 0;
	}

}
