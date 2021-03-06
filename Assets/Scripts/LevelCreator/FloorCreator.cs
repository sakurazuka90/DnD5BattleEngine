﻿using UnityEngine;
using System.Collections;

public class FloorCreator : MonoBehaviour {

	public GameObject floorTile;

	public GameObject [] walls;

	private Vector3 lvTileSize;
	private float nextTileX;
	private float nextTileZ;

	private int graphicsStyle = 0;

	public static FloorCreator instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this);
	}

	public int GraphicsStyle{
		get{ return this.graphicsStyle; }
	}

	public void CreateFloor(int x, int y, int pmGraphicsStyle)
	{
		GameObjectUtils.RemoveAllChildren (this.gameObject);
		string floorAssetName = "";

		this.graphicsStyle = pmGraphicsStyle;

		switch (pmGraphicsStyle) {
		case 0:
			floorAssetName = "Floor_A";
			break;
		case 1:
			floorAssetName = "Terrain_woodland_1";
			break;
		case 2:
			break;
		}

		floorTile = Resources.Load<GameObject> (floorAssetName);

		MeshRenderer renderer = floorTile.GetComponent<MeshRenderer> ();
		Terrain terrain = floorTile.GetComponent<Terrain> ();

		if (renderer != null)
			lvTileSize = renderer.bounds.size;
		else if (terrain != null)
			lvTileSize = terrain.terrainData.size;


		nextTileX = -lvTileSize.x / 2;
		nextTileZ = -lvTileSize.z / 2;

		// Strange conditions made to make floor bit bigger than grid so creator will have enough place for walls later
		for (float j = -lvTileSize.z; j < (y + lvTileSize.z); j += lvTileSize.z) {
			for (float i = -lvTileSize.x; i < (x + lvTileSize.x); i += lvTileSize.x) {
				GameObject lvTile = GameObject.Instantiate (floorTile);
				lvTile.transform.parent = this.gameObject.transform;
				lvTile.transform.position = new Vector3 (nextTileX,0.0f,nextTileZ);
				nextTileX += lvTileSize.x;
			}
			nextTileX = -lvTileSize.x / 2;
			nextTileZ += lvTileSize.z;

		}
	}
		
	public void CreateWalls(int x, int y)
	{
		string wallAssetName = "";

		switch (graphicsStyle) {
		case 0:
			wallAssetName = "Dungeon";
			break;
		case 1:
			wallAssetName = "Woodland";
			break;
		case 2:
			break;
		}

		walls = Resources.LoadAll<GameObject> ("Walls/" + wallAssetName); 

		CreateTopWall(x, y);
		CreateSideWalls (x, y);
	}

	private void CreateTopWall(int x, int y)
	{
		nextTileX = -lvTileSize.x / 2;
		nextTileZ = (float)y + lvTileSize.z / 2;
		
		for (float i = -lvTileSize.x; i < (x + lvTileSize.x); i += lvTileSize.x) {
			GameObject lvWall = null;

			if(i == -lvTileSize.x || (i + lvTileSize.x) >= (x + lvTileSize.x))
				lvWall = GameObject.Instantiate (walls [0]);
			else
				lvWall = GameObject.Instantiate (walls [Random.Range(0,walls.Length)]);

			lvWall.transform.parent = this.gameObject.transform;
			lvWall.transform.position = new Vector3 (nextTileX,0.0f,nextTileZ);

			if(lvWall.transform.GetChild(0).gameObject.GetComponent<Terrain>() == null)
				lvWall.transform.Rotate (0.0f, 90.0f, 0.0f);
			nextTileX += lvTileSize.x;
		}

	}

	private void CreateSideWalls(int x, int y)
	{
		nextTileX = -(lvTileSize.x / 2) -1;
		float lvNextTileXRight = (nextTileX * -1) + x;
		nextTileZ = -lvTileSize.z / 2;

		for (float i = -lvTileSize.z; i < (y + lvTileSize.z); i += lvTileSize.z) {

			GameObject lvWall = null;
			if(i+lvTileSize.z >= (y + lvTileSize.z))
				lvWall = GameObject.Instantiate (walls [0]);
			else
				lvWall = GameObject.Instantiate (walls [Random.Range(0,walls.Length)]);
			
			lvWall.transform.parent = this.gameObject.transform;
			lvWall.transform.position = new Vector3 (nextTileX,0.0f,nextTileZ);

			if(lvWall.transform.GetChild(0).gameObject.GetComponent<Terrain>() == null)
				lvWall.transform.Rotate (0.0f, 0.0f, 0.0f);
			
			GameObject lvWallRight = null;

			if(i+lvTileSize.z >= (y + lvTileSize.z))
				lvWallRight = GameObject.Instantiate (walls [0]);
			else
				lvWallRight = GameObject.Instantiate (walls [Random.Range(0,walls.Length)]);


			lvWallRight.transform.parent = this.gameObject.transform;
			lvWallRight.transform.position = new Vector3 (lvNextTileXRight,0.0f,nextTileZ);

			if(lvWall.transform.GetChild(0).gameObject.GetComponent<Terrain>() == null)
				lvWallRight.transform.Rotate (0.0f, 180.0f, 0.0f);

			nextTileZ += lvTileSize.z;
		}

	}


}
