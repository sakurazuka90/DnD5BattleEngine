using System;
using System.Collections.Generic;

public class MovementGambitImpl:Gambit
{

	Player gambitPlayer;

	public MovementGambitImpl ()
	{
	}

	public Player GambitPlayer{
		set{ gambitPlayer = value; }
	}

	public void Process()
	{
		//int id = Localizer.instance.FindClosestEnemy (gambitPlayer.Figurine.GetComponent<FigurineStatus>().gridX, gambitPlayer.Figurine.GetComponent<FigurineStatus>().gridZ);

		int id = gambitPlayer.TargetPlayer.GetCellIndex ();

		List<int> list = SelectFromGrid.instance.GetAdjacentNonBlockedFields (id);
		list.Add (id);

		string steps = astar.instance.GetRouteAstar(GridDrawer.instance.GetGridId(gambitPlayer.Figurine.GetComponent<FigurineStatus>().gridX, gambitPlayer.Figurine.GetComponent<FigurineStatus>().gridZ), list, id).GetAstarAsMoverSteps();

		FigurineMover lvMover = gambitPlayer.Figurine.GetComponent<FigurineMover> ();

		lvMover.path = steps;
		lvMover.isAi = true;

		lvMover.movePoints = gambitPlayer.movesLeft;
		lvMover.isMoving = true;

		AIEngine.instance.free = false;
	}

	public bool Evaluate()
	{
		if (gambitPlayer != null && gambitPlayer.IsAbleToMove () && doNeadToMove() && gambitPlayer.TargetPlayer != null) {

			if (gambitPlayer.mTotalMoveActions == 0 && gambitPlayer.mTotalStandardActions > 0 && gambitPlayer.movesLeft == 0)
				gambitPlayer.ConvertStandardActionToMove ();

			return true;
		}
		else
			return false;
	}

	public bool doNeadToMove()
	{
		int id = Localizer.instance.FindClosestEnemy (gambitPlayer.Figurine.GetComponent<FigurineStatus>().gridX, gambitPlayer.Figurine.GetComponent<FigurineStatus>().gridZ);

		List<int> list = SelectFromGrid.instance.GetAdjacentNonBlockedFields (id);

		int targetField = GridDrawer.instance.GetGridId (gambitPlayer.Figurine.GetComponent<FigurineStatus> ().gridX, gambitPlayer.Figurine.GetComponent<FigurineStatus> ().gridZ);

		if (list.Contains (targetField))
			return false;
		else
			return true;
	}
}


