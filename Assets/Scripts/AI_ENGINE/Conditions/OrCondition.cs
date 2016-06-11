using System;
using System.Collections.Generic;

public class OrCondition
{
	List<Condition> conditions;

	public OrCondition ()
	{
		conditions = new List<Condition> ();
	}



	#region Condition implementation

	public bool Evaluate ()
	{
		bool result = false;

		foreach (Condition lvCon in conditions) {
			result = result || lvCon.Evaluate ();
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


