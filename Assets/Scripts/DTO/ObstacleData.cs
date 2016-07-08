using System;

[Serializable]
public class ObstacleData{

	public string obstaclePrefabName;

	public bool isDifficultTerrain;
	public bool isBlockingMovement;
	public bool isBlockingLineOfSight;
	public int providedCover;
	public float rotation;
}
