using UnityEngine;
using System.Collections;

public class DiceRoller{

	public static int RollDice(int pmSides, int pmDiceAmount)
	{
		int lvResult = 0;

		for (int i = 0; i < pmDiceAmount; i++) {
			lvResult += Random.Range(1,pmSides);
		}

		return lvResult;
	}

	public static int D20{
		get{ return RollDice (20,1); }
	}



}
