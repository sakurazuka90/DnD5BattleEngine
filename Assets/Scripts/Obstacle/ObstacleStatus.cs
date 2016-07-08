using UnityEngine;
using System.Collections.Generic;

public class ObstacleStatus : MonoBehaviour {

	private List<int> cellMods;

	public string name;
	public string prefabName;
	public bool isDifficultTerrain;
	public bool isBlockingMovement;
	public bool isBlockingLoS;
	public int coverValue;

	public ObstacleData getData()
	{
		ObstacleData lvData = new ObstacleData ();

		lvData.isBlockingLineOfSight = isBlockingLoS;
		lvData.isBlockingMovement = isBlockingMovement;
		lvData.isDifficultTerrain = isDifficultTerrain;
		lvData.providedCover = coverValue;
		lvData.obstaclePrefabName = this.prefabName;
		lvData.rotation = this.gameObject.transform.GetChild (0).eulerAngles.y;

		return lvData;
	}
}
