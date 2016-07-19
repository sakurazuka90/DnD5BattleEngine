using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player {

	private int mInitiativeBonus;
	private Dictionary<AbilityNames,Ability> mAbilities;
	private Dictionary<TestsNames,AbilityNames> mTestAbilities;
	private int mSpeed;
	private int mTotalHp;
	public int mTotalMoveActions = 1;
	public int mTotalStandardActions = 1;
	public int mTotalBonusActions = 1;
	private int mProficiencyBonus;
	private int mSurvivalSucceded = 0;
	private int mSurvivalFailed = 0;
	private List<WeaponCategory> mWeaponCategoryProficiency;


	public GameObject Figurine;
	public Sprite PlayerSprite;
	public string playerName;
	public int movesLeft;
	public int hp;
	public Attack equippedWeaponAttack;
	public int ac;
	public bool isDead = false;
	public bool isStable = false;
	private Dictionary<string,Item> mInventory;
	public int level;

	private Sizes size;

	public int mBasicAc = 10;
	public int mMaxDexMod = 6;

	public bool isAi = false;

	private string _figurineModelName;

	private List<Gambit> gambitList;

	public int databaseEqWeaponId = 0;

	private Player targetPlayer = null;

	private Player lastHitter = null;

	private List<int> saves = null;

	private int type = 0;
	private string subtype = "";

	public List<int> Saves
	{
		get{ return saves;}
		set{ this.saves = value;}
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

	public Dictionary<string,Item> Inventory{
		get{ return mInventory;}
		set{ this.mInventory = value;}
	}

	public string FigurineModelName{
		get{ return _figurineModelName;}
		set{ this._figurineModelName = value;}
	}

	public List<Gambit> GambitList{
		get{ return gambitList; }
		set{ this.gambitList = value; }
	}

	public Player TargetPlayer{
		get{ return targetPlayer; }
		set{ this.targetPlayer = value; }
	}

	public Player LastHitter{
		get{ return lastHitter; }
		set{ this.lastHitter = value; }
	}

	public Sizes Size{
		get{ return size; }
		set{ this.size = value; }
	}

	public Player ()
	{
		mAbilities = new Dictionary<AbilityNames, Ability> ();

		mTestAbilities = new Dictionary<TestsNames, AbilityNames> ();
		mTestAbilities.Add(TestsNames.INITIATIVE,AbilityNames.DEXTERITY);
		mWeaponCategoryProficiency = new List<WeaponCategory> ();

		// WILL BE REMOVED WHEN GAMBITS WILL BE LOADED FROM DB
		FillGambitList();
	}

	public void SetAbility(AbilityNames pmName, int pmScore)
	{
		mAbilities.Remove (pmName);
		mAbilities.Add (pmName, new Ability(pmScore));
	}

	public int GetAbilityValue(AbilityNames pmName)
	{
		return mAbilities [pmName].Value;
	}

	public int GetAbilityModifier(AbilityNames pmName)
	{
		return mAbilities [pmName].Modifier;
	}

	public int RollTest(TestsNames pmName)
	{
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



	public void GetDamage(int pmDamage)
	{
		if (hp == 0 && !isDead) {
			mSurvivalFailed += 2;

			if (mSurvivalFailed >= 3) {
				mSurvivalFailed = 0;
				mSurvivalSucceded = 0;
				isDead = true;
				Figurine.GetComponent<MessageDisplayer>().SetMessage("DEAD!");
			}

		} else {

			if (hp > pmDamage)
				hp -= pmDamage;
			else
				hp = 0;

			if (hp == 0) {
				this.Figurine.GetComponentInChildren<Animator> ().SetBool ("isDead", true);
			}
		}
	}

	public void MakeSurvivalCheck()
	{
		int lvRoll = DiceRoller.D20;

		if (lvRoll >= 10) {
			mSurvivalSucceded += 1;

			Figurine.GetComponent<MessageDisplayer>().SetMessage("SURVIVAL SUCCES!");

			if (lvRoll == 20) {
				Figurine.GetComponent<MessageDisplayer>().SetMessage("CRITICAL SUCCES - STABLE!");
				mSurvivalFailed = 0;
				mSurvivalSucceded = 0;
				hp = 1;
			}
		}  else {
			mSurvivalFailed += 1;

			Figurine.GetComponent<MessageDisplayer>().SetMessage("SURVIVAL FAILURE!");
			if (lvRoll == 1) {
				Figurine.GetComponent<MessageDisplayer>().SetMessage("CRITICAL FAILURE - DEAD!");
				mSurvivalFailed = 0;
				mSurvivalSucceded = 0;
				isDead = true;
			}
		}

		if (hp == 0 && !isDead) {
			if (mSurvivalSucceded >= 3) {
				mSurvivalFailed = 0;
				mSurvivalSucceded = 0;
				isStable = true;
				Figurine.GetComponent<MessageDisplayer>().SetMessage("STABLE!");
			} else if (mSurvivalFailed >= 3) {
				mSurvivalFailed = 0;
				mSurvivalSucceded = 0;
				isDead = true;
				Figurine.GetComponent<MessageDisplayer>().SetMessage("DEAD!");
			}
		}

	}

	public void UpdateAc()
	{
		int lvAc = mBasicAc;

		if (mAbilities [AbilityNames.DEXTERITY].Modifier > mMaxDexMod)
			lvAc += mMaxDexMod;
		else
			lvAc += mAbilities [AbilityNames.DEXTERITY].Modifier;

		ac = lvAc;
	}

	public bool IsAbleToMove()
	{
		return mTotalMoveActions > 0 || mTotalStandardActions > 0;
	}

	public void FillGambitList()
	{
		MovementGambitImpl lvGambit = new MovementGambitImpl ();
		lvGambit.GambitPlayer = this;

		gambitList = new List<Gambit> ();

		TargetPlayerGambitImpl lvTargetGambit = new TargetPlayerGambitImpl (Features.HIGHEST_ARMOUR, PlayerTypes.PLAYER, PlayerAdjectives.CLOSEST, this);

		AttackGambitImpl lvAttackGambit = new AttackGambitImpl ();
		lvAttackGambit.GambitPlayer = this;

		gambitList.Add (lvGambit);
		gambitList.Add (lvTargetGambit);
		gambitList.Add (lvAttackGambit);
	}

	public void RefillActionsPool()
	{
		mTotalBonusActions = 1;
		mTotalMoveActions = 1;
		mTotalStandardActions = 1;
	}

	public void UseActionForMovement()
	{
		if (mTotalMoveActions > 0)
			mTotalMoveActions--;
		else if (mTotalStandardActions > 0)
			mTotalStandardActions--;
	}

	public void ConvertStandardActionToMove()
	{
		ResetMovesLeft ();
		mTotalMoveActions++;
		mTotalStandardActions--;
	}

	public int GetCellIndex()
	{
		return GridDrawer.instance.GetGridId (Figurine.GetComponent<FigurineStatus> ().gridX, Figurine.GetComponent<FigurineStatus> ().gridZ);
	}


}
