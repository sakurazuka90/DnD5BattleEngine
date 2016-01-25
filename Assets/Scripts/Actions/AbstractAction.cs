using System;
using UnityEngine;

using System.Collections.Generic;

public abstract class AbstractAction
{
	protected ActiveAttackDefenceTypes mDefenceType; 
	protected Weapon mWeapon;
	protected AbilityNames mAbility;

	public AbstractAction ()
	{
	}

	protected void ResolveActiveHit(Player pmAttacker, Player pmTarget, bool pmIsAdvantage)
	{
		pmAttacker.Figurine.GetComponent<AnimationPlayer> ().play = true;

		int lvDefValue = pmTarget.GetActiveDefence (mDefenceType);

		int lvAttackBonus = 0;

		if (mWeapon != null) {

			lvAttackBonus += pmAttacker.GetWeaponProficiency (mWeapon.Category);
		}

		lvAttackBonus += pmAttacker.GetAbilityModifier (mAbility);

		int lvDice = DiceRoller.D20;

		int lvAttack = lvDice + lvAttackBonus;

		if (lvAttack >= lvDefValue || lvDice == 20) {
			ResolveDamage (pmAttacker, pmTarget, lvDice == 20, pmIsAdvantage);
		} else {
			pmTarget.Figurine.GetComponent<MessageDisplayer>().message = "MISS!";
		}

		GameObject lvDrawerObject = GameObject.Find ("GridDrawer");
		GridDrawer lvDrawer = lvDrawerObject.GetComponent<GridDrawer> ();

		lvDrawer.ClearGridStatus ();

		GameObject lvSpoolerObject = GameObject.Find ("PlayerSpooler");
		PlayerSpooler lvSpooler = lvSpoolerObject.GetComponent<PlayerSpooler> ();
		lvSpooler.spool ();


	}

	protected bool ResolvePassiveHit(Player pmAttacker, Player pmTarget)
	{
		return false; //TODO
	}

	protected void ResolveDamage(Player pmAttacker, Player pmTarget, bool pmIsCritical, bool pmIsAdvantage)
	{
		int lvWeaponAmount = mWeapon.DieceNumber;

		MessageDisplayer lvMessageDisplayer = pmTarget.Figurine.GetComponent<MessageDisplayer> ();

		if (pmIsCritical) {
			lvMessageDisplayer.message = "CRITICAL HIT!";
			lvWeaponAmount = lvWeaponAmount * 2;
		}
		int lvWeaponDamage = DiceRoller.RollDice (mWeapon.DieceType, lvWeaponAmount);

		lvWeaponDamage += pmAttacker.GetAbilityModifier (mAbility);

		lvMessageDisplayer.message = lvWeaponDamage.ToString();

		pmTarget.GetDamage (lvWeaponDamage);

	}

	public void DisplayTargets(Player pmAttacker)
	{
		GameObject lvSelectorObject = GameObject.Find ("GridSelector");
		SelectFromGrid lvSelector = lvSelectorObject.GetComponent<SelectFromGrid> ();

		GameObject lvDrawerObject = GameObject.Find ("GridDrawer");
		GridDrawer lvDrawer = lvDrawerObject.GetComponent<GridDrawer> ();

		FigurineStatus lvStatus = pmAttacker.Figurine.GetComponent<FigurineStatus> ();

		int lvCellId = lvDrawer.GetGridId (lvStatus.gridX,lvStatus.gridZ);
		List<int> lvFields = lvSelector.GetAdjacentFields (lvCellId);

		List<int> lvTargetFields = new List<int> ();

		foreach (int lvField in lvFields) {
			if (lvSelector.IsEnemyField (lvField))
				lvTargetFields.Add (lvField);
		}

		lvDrawer.ClearGridStatus ();
		lvSelector.SetStateToCells (lvTargetFields, CellStates.TARGET);

	}
}

