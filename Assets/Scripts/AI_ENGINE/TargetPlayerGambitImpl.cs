using System;

public class TargetPlayerGambitImpl : Gambit
{
	private Features feature;
	private PlayerTypes type;
	private PlayerAdjectives adjective;

	public TargetPlayerGambitImpl (Features pmFeature, PlayerTypes pmType, PlayerAdjectives pmAdjective)
	{
		feature = pmFeature;
		type = pmType;
		adjective = pmAdjective;
	}

	#region Gambit implementation

	public void Process ()
	{
		if (type == PlayerTypes.ENEMY) {

		} else if (type == PlayerTypes.PLAYER) {
		}
	}

	public bool Evaluate ()
	{
		//No requirements I can think of at this point
		return true;
	}

	#endregion
}


