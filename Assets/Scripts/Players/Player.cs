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

	private Dictionary<TestsNames,AbilityNames> mTestAbilities;

	public Player ()
	{
		mAbilities = new Dictionary<AbilityNames, Ability> ();

		mTestAbilities = new Dictionary<TestsNames, AbilityNames> ();
		mTestAbilities.Add(TestsNames.INITIATIVE,AbilityNames.DEXTERITY);
	}

	public void setAbility(AbilityNames pmName, int pmScore)
	{
		mAbilities.Remove (pmName);
		mAbilities.Add (pmName, new Ability(pmScore));
	}

	public int rollTest(TestsNames pmName)
	{
		Debug.Log (playerName + " rolled ");
		return DiceRoller.RollDice (20, 1) + mAbilities [mTestAbilities [pmName]].getModifier ();
	}

}
