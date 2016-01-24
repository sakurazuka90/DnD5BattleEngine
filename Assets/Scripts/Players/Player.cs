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

	private int mTotalHp;
	public int hp;
	private int mProficiencyBonus;

	public Attack equippedWeaponAttack;
	public int ac;


	private List<WeaponCategory> mWeaponCategoryProficiency;

	public Player ()
	{
		mAbilities = new Dictionary<AbilityNames, Ability> ();

		mTestAbilities = new Dictionary<TestsNames, AbilityNames> ();
		mTestAbilities.Add(TestsNames.INITIATIVE,AbilityNames.DEXTERITY);
		mWeaponCategoryProficiency = new List<WeaponCategory> ();
	}

	public void SetAbility(AbilityNames pmName, int pmScore)
	{
		mAbilities.Remove (pmName);
		mAbilities.Add (pmName, new Ability(pmScore));
	}

	public int GetAbilityModifier(AbilityNames pmName)
	{
		return mAbilities [pmName].Modifier;
	}

	public int RollTest(TestsNames pmName)
	{
		//Debug.Log (playerName + " rolled ");
		return DiceRoller.RollDice (20, 1) + mAbilities [mTestAbilities [pmName]].Modifier;
	}

	public int GetSpeed()
	{
		return mSpeed;
	}

	public void SetSpeed(int pmSpeed)
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
	}

	public int GetActiveDefence(ActiveAttackDefenceTypes pmType)
	{
		switch (pmType) {
		case ActiveAttackDefenceTypes.AC:
			return this.ac;
		default:
			return 0;
		}
	}

	public int GetWeaponProficiency(WeaponCategory pmTestedCategory)
	{
		if (mProficiencyBonus == 0)
			return 0;
		
		if (mWeaponCategoryProficiency.Contains (pmTestedCategory))
			return mProficiencyBonus;
		else
			return 0;
	}

	public void AddWeaponProficiency(WeaponCategory pmCategory)
	{
		if (!mWeaponCategoryProficiency.Contains (pmCategory))
			mWeaponCategoryProficiency.Add (pmCategory);
	}

	public int HpTotal
	{
		get{ return mTotalHp;}
		set{ this.mTotalHp = value;}
	}

	public int Proficiency
	{
		get{ return mProficiencyBonus;}
		set{ this.mProficiencyBonus = value;}
	}

	public void GetDamage(int pmDamage)
	{
		if (hp > pmDamage)
			hp = -pmDamage;
		else
			hp = 0;
	}



}
