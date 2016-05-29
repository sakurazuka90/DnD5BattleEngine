using System;
using System.Collections.Generic;

public class AstarResponse
{
	private Dictionary<int,int> path;
	private int foundTarget;
	private int startPoint;

	public AstarResponse (Dictionary<int,int> pmPath, int pmStartPoint, int pmFoundTarget)
	{
		path = pmPath;
		foundTarget = pmFoundTarget;
		startPoint = pmStartPoint;
	}

	public string GetAstarAsMoverSteps(){

		Dictionary<int,int> lvDict = path;

		int current = foundTarget;

		List<int> pmSteps = new List<int> ();

		while (current != startPoint) {
			pmSteps.Add (current);
			current = path [current];
		}

		pmSteps.Add (startPoint);

		pmSteps.Reverse ();

		string lvSteps = "";

		for (int i = 0; i < pmSteps.Count; i++) {
			lvSteps += pmSteps [i];

			if (i != (pmSteps.Count - 1)) {
				lvSteps += "_";
			}
		}

		return lvSteps;
	}
}


