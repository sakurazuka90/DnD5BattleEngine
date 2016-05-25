using System;
using System.Collections.Generic;
using UnityEngine;

public class Localizer : MonoBehaviour
{
	public static Localizer instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
	}


	public int FindClosestEnemy(int pmX1, int pmZ1)
	{
		int result = 0;
		double distance = 10000;

		List<int> enemiesFields = FindAllEnemies ();

		int x1 = pmX1;
		int z1 = pmZ1;

		foreach (int lvFieldId in enemiesFields) {
			int x2 = GridDrawer.instance.getGridX (lvFieldId);
			int z2 = GridDrawer.instance.getGridZ (lvFieldId);

			double calculatedDistance = MathUtils.CalculateDistance (x1, x2, z1, z2);

			if (distance == null || calculatedDistance < distance) {
				result = lvFieldId;
				distance = calculatedDistance;
			}
				
		}

		return result;	
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


