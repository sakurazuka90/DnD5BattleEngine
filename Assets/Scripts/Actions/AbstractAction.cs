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

	protected void ResolveActiveHit (Player pmAttacker, Player pmTarget, bool pmIsAdvantage, bool pmIsMissleShoot = false)
	{
		if (mWeapon.WeaponType == WeaponType.MELEE ||(pmIsMissleShoot && mWeapon.WeaponType == WeaponType.RANGED)) {

			if(mWeapon.WeaponType == WeaponType.MELEE)
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

				pmTarget.LastHitter = pmAttacker;

				ResolveDamage (pmAttacker, pmTarget, lvDice == 20, pmIsAdvantage);
			} else {
				pmTarget.Figurine.GetComponent<MessageDisplayer> ().SetMessage ("MiSS!");
				pmTarget.Figurine.GetComponentInChildren<Animator> ().SetBool ("isEvading", true);
			}

			GridDrawer.instance.ClearGridStatus ();

			GameObject lvSpoolerObject = GameObject.Find ("PlayerSpooler");
			PlayerSpooler lvSpooler = lvSpoolerObject.GetComponent<PlayerSpooler> ();
			lvSpooler.spool ();
		} else {
			FireProjectile (pmAttacker, pmTarget, pmIsAdvantage);
		}
	}

	protected bool ResolvePassiveHit (Player pmAttacker, Player pmTarget)
	{
		return false; //TODO
	}

	public void ResolveDamage (Player pmAttacker, Player pmTarget, bool pmIsCritical, bool pmIsAdvantage)
	{
		int lvWeaponAmount = mWeapon.DieceNumber;

		MessageDisplayer lvMessageDisplayer = pmTarget.Figurine.GetComponent<MessageDisplayer> ();

		if (pmIsCritical) {
			lvMessageDisplayer.SetMessage("CRITICAL HIT!");
			lvWeaponAmount = lvWeaponAmount * 2;
		}
		int lvWeaponDamage = DiceRoller.RollDice (mWeapon.DieceType, lvWeaponAmount);

		lvWeaponDamage += pmAttacker.GetAbilityModifier (mAbility);

		lvMessageDisplayer.SetMessage(lvWeaponDamage.ToString ());

		pmTarget.GetDamage (lvWeaponDamage);

	}

	public void FireProjectile(Player pmAttacker, Player pmTarget, bool pmIsAdvantage)
	{
		AttackInstantiator lvInstantiator = pmAttacker.Figurine.GetComponent<AttackInstantiator> ();

		lvInstantiator.lvAttacker = pmAttacker;
		lvInstantiator.lvTarget = pmTarget;
		lvInstantiator.speed = 15.0f;
		lvInstantiator.shoot = true;
		lvInstantiator.isAdvantage = pmIsAdvantage;
	}

	public void DisplayTargets (Player pmAttacker)
	{
		GameObject lvSelectorObject = GameObject.Find ("GridSelector");
		SelectFromGrid lvSelector = lvSelectorObject.GetComponent<SelectFromGrid> ();

		FigurineStatus lvStatus = pmAttacker.Figurine.GetComponent<FigurineStatus> ();

		int lvCellId = GridDrawer.instance.GetGridId (lvStatus.gridX, lvStatus.gridZ);

		if (mWeapon != null && mWeapon.WeaponType != WeaponType.RANGED) {
			List<int> lvFields = lvSelector.GetAdjacentFields (lvCellId);

			List<int> lvTargetFields = new List<int> ();

			foreach (int lvField in lvFields) {
				if (lvSelector.IsEnemyField (lvField))
					lvTargetFields.Add (lvField);
			}

			GridDrawer.instance.ClearGridStatus ();
			lvSelector.SetStateToCells (lvTargetFields, CellStates.TARGET);
		} else {
			lvSelector.DisplaySimpleRangeMoore (lvCellId, mWeapon.rangeNormal, mWeapon.rangeLong);

		}

	}

	public bool IsTargerInRange(Player pmPlayer)
	{
		//pmPlayer.

		//if(mWeapon.WeaponType == WeaponType.MELEE)
			

		return false;
	}
}

