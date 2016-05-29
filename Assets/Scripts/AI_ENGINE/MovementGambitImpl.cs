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
		int id = Localizer.instance.FindClosestEnemy (gambitPlayer.Figurine.GetComponent<FigurineStatus>().gridX, gambitPlayer.Figurine.GetComponent<FigurineStatus>().gridZ);


		List<int> list = SelectFromGrid.instance.GetAdjacentNonBlockedFields (id);
		list.Add (id);

		string steps = astar.instance.GetRouteAstar(GridDrawer.instance.GetGridId(gambitPlayer.Figurine.GetComponent<FigurineStatus>().gridX, gambitPlayer.Figurine.GetComponent<FigurineStatus>().gridZ), list, id).GetAstarAsMoverSteps();

		FigurineMover lvMover = gambitPlayer.Figurine.GetComponent<FigurineMover> ();

		lvMover.path = steps;
		lvMover.isAi = true;
		lvMover.isMoving = true;

		AIEngine.instance.free = false;
	}

	public bool Evaluate()
	{
		if (gambitPlayer != null && gambitPlayer.IsAbleToMove ())
			return true;
		else
			return false;
	}
}


