using System;
using System.Collections.Generic;
using UnityEngine;

public class Localizer
{
	public int FindClosestEnemy(bool isPlayer)
	{
		return 0;	
	}

	public List<int> FindAllEnemies()
	{
		List<int> lvEnemies = new List<int>();

		for (int i = 0; i < GridDrawer.instance.mCells.Length; i++){
			if (SelectFromGrid.instance.IsEnemyField (i))
				lvEnemies.Add (i);
		}

		return lvEnemies;
	}



}


