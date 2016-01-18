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

	private int mSpeed;
	public int movesLeft;

	public int hp;

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

	public int getSpeed()
	{
		return mSpeed;
	}

	public void setSpeed(int pmSpeed)
	{
		mSpeed = pmSpeed;
	}

	public void ResetMovesLeft()
	{
		movesLeft = mSpeed;
		FigurineStatus lvStatus = Figurine.GetComponent<FigurineStatus> ();
		lvStatus.movesLeft = movesLeft;

	}

	public void DecreaseMovesLeft(int pmValue)
	{
		if (pmValue >= movesLeft)
			movesLeft = 0;
		else
			movesLeft -= pmValue;

		FigurineStatus lvStatus = Figurine.GetComponent<FigurineStatus> ();
		lvStatus.movesLeft = movesLeft;

		GameObject lvSpoolerObj = GameObject.Find ("PlayerSpooler");
		PlayerSpooler lvSpooler = lvSpoolerObj.GetComponent<PlayerSpooler> ();
	}



}
