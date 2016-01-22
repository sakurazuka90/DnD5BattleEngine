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

	protected bool ResolveActiveHit(Player pmAttacker, Player pmTarget)
	{
		
		int lvDefValue = pmTarget.GetActiveDefence (mDefenceType);

		int lvAttackBonus = 0;

		if (mWeapon != null) {

			lvAttackBonus += pmAttacker.GetWeaponProficiency (mWeapon.Category);
		}

		lvAttackBonus += pmAttacker.GetAbilityModifier (mAbility);

		int lvAttack = DiceRoller.D20 + lvAttackBonus;

		return lvAttack >= lvDefValue;

	}

	protected bool ResolvePassiveHit(Player pmAttacker, Player pmTarget)
	{
		return false; //TODO
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

