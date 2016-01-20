using System;

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

}

