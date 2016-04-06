﻿using UnityEngine;
using System.Collections;

public class BattlefieldConstructor : MonoBehaviour {

	public static BattlefieldConstructor instance;

	public bool isGameplay = false;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this);
	}

	// Use this for initialization
	void Start () {
	}

	public void GenerateGameplay(string pmFilename)
	{
		BattlefieldStateReader.instance.ParseBattlefieldFile (pmFilename);
		GenerateGridFromFile ();
		SetupCameraMover ((float)BattlefieldStateReader.instance.GridWidth, (float)BattlefieldStateReader.instance.GridHeight);
		CreateFloor (BattlefieldStateReader.instance.GridWidth, BattlefieldStateReader.instance.GridHeight);
		CreateWalls (BattlefieldStateReader.instance.GridWidth, BattlefieldStateReader.instance.GridHeight);
		SetupObstacles(BattlefieldStateReader.instance.Obstacles);
	}

	public void GenerateGrid(int pmGridWidth, int pmGridHeight)
	{
		GridDrawer.instance.Create (pmGridWidth, pmGridHeight);
	}

	public void GenerateGridFromFile()
	{
		GenerateGrid (BattlefieldStateReader.instance.GridWidth, BattlefieldStateReader.instance.GridHeight);
	}

	public void SetupCameraMover(float pmX, float pmY)
	{
		MoveCamera.instance.isMovable = true;
		MoveCamera.instance.maxX = pmX;
		MoveCamera.instance.maxZ = pmY;
	}

	public void CreateFloor(int pmGridWidth, int pmGridHeight)
	{
		FloorCreator.instance.CreateFloor (pmGridWidth,pmGridHeight);
	}

	public void CreateWalls(int pmGridWidth, int pmGridHeight)
	{
		FloorCreator.instance.CreateWalls (pmGridWidth, pmGridHeight);
	}

	public void SetupObstacles(ObstacleData [] pmObstacleData)
	{
		GameObject[] lvCells = GridDrawer.instance.mCells;

		for (int i = 0; i < pmObstacleData.Length; i++) {
			if (pmObstacleData [i] != null) {
				ObstacleData lvData = pmObstacleData [i];
				GameObject lvPrefab = (GameObject)Resources.Load(lvData.obstaclePrefabName, typeof(GameObject));

				GameObject lvInstance = Instantiate (lvPrefab);
				lvInstance.transform.parent = lvCells [i].transform;

				FigurineMover lvMover = lvInstance.GetComponent<FigurineMover> ();
				lvMover.gridX = GridDrawer.instance.getGridX (i);
				lvMover.gridZ = GridDrawer.instance.getGridZ (i);

				ObstacleStatus lvStatus = lvInstance.GetComponent<ObstacleStatus> ();
				lvStatus.isBlockingLoS = lvData.isBlockingLineOfSight;
				lvStatus.isDifficultTerrain = lvData.isDifficultTerrain;
				lvStatus.isBlockingMovement = lvData.isBlockingMovement;

				lvInstance.GetComponent<ObstacleRotator> ().Rotate (lvData.rotation);

				//lvInstance.transform.eulerAngles = new Vector3 (0.0f, lvData.rotation, 0.0f);

			}
		}


	}
	

}
