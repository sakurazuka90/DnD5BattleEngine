using UnityEngine;
using System.Collections.Generic;

public class ObstacleStatus : MonoBehaviour {

	private List<int> cellMods;

	public string name;
	public string prefabName;
	public bool isDifficultTerrain;
	public bool isBlockingMovement;
	public bool isBlockingLoS;

	public ObstacleData getData()
	{
		ObstacleData lvData = new ObstacleData ();

		lvData.isBlockingLineOfSight = isBlockingLoS;
		lvData.isBlockingMovement = isBlockingMovement;
		lvData.isDifficultTerrain = isDifficultTerrain;
		lvData.providedCover = 0.0f;
		lvData.obstaclePrefabName = this.prefabName;
		lvData.rotation = this.gameObject.transform.rotation.y;

		return lvData;
	}
}
