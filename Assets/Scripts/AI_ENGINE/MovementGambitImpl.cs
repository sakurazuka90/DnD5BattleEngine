using System;

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

		string steps = astar.instance.GetAstarAsMoverSteps (GridDrawer.instance.GetGridId(gambitPlayer.Figurine.GetComponent<FigurineStatus>().gridX, gambitPlayer.Figurine.GetComponent<FigurineStatus>().gridZ), id);

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


