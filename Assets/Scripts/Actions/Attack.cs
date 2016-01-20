using UnityEngine;
using System.Collections;

public class Attack : AbstractAction{

	private bool mIsActive;
	private string mName;

	public Attack(string pmName)
	{
		mName = pmName;
		mIsActive = true;
	}

	public string Name{
		get{ return mName; }
	}


	public bool resolveHit (Player pmAttacker, Player pmDefender)
	{
		if (mIsActive)
			return ResolveActiveHit (pmAttacker, pmDefender);
		else
			return ResolvePassiveHit (pmAttacker, pmDefender);
	}

}
