using System;
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
		throw new NotImplementedException ();
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


