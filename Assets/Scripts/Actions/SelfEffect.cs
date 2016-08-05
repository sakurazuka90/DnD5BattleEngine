using System;

public class SelfEffect
{
	private string mName;
	private Spell mSpell;

	private int mDieceNumber;
	private int mDieceSides;
	private int mStaticValue;
	private int mEffectType;

	public SelfEffect(string pmName, Spell pmSpell, int pmDieceNumber, int pmDieceSides, int pmStaticValue, int pmEffectType)
	{
		mName = pmName;
		mSpell = pmSpell;
		mDieceSides = pmDieceSides;
		mDieceNumber = pmDieceNumber;
		mStaticValue = pmStaticValue;
		mEffectType = pmEffectType;
	}

	public void Resolve(Player pmUser)
	{
		if (mSpell != null) {
			//RESOLVE SPELL
		}

		//ROLL
		int value = DiceRoller.RollDice (mDieceSides, mDieceNumber);

		if (mEffectType == 1)
			value *= -1;

		pmUser.GetDamage (value);


	}
}


