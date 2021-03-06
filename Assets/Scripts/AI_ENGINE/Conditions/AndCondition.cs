﻿using System;
using System.Collections.Generic;

public class AndCondition : Condition
{
	List<Condition> conditions;


	public AndCondition ()
	{
		conditions = new List<Condition> ();
	}

	#region Condition implementation

	public bool Evaluate ()
	{
		bool result = true;

		foreach (Condition lvCon in conditions) {
			result = result && lvCon.Evaluate ();
		}

		return result;
	}

	#endregion

	public void Add(Condition pmCondition)
	{
		conditions.Add (pmCondition);
	}

	public void AddRange(List<Condition> pmConditions)
	{
		conditions.AddRange (pmConditions);
	}
}


