﻿using System;

[Serializable]
public class ObstacleData{

	public string obstaclePrefabName;

	public bool isDifficultTerrain;
	public bool isBlockingMovement;
	public bool isBlockingLineOfSight;
	public float providedCover;
	public float rotation;
}