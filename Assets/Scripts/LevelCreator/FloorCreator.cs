using UnityEngine;
using System.Collections;

public class FloorCreator : MonoBehaviour {

	public GameObject floorTile;

	public GameObject [] walls;

	private Vector3 lvTileSize;
	private float nextTileX;
	private float nextTileZ;

	public static FloorCreator instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this);
	}

	public void CreateFloor(int x, int y)
	{
		GameObjectUtils.RemoveAllChildren (this.gameObject);

		lvTileSize = floorTile.GetComponent<MeshRenderer> ().bounds.size;
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
		CreateTopWall(x, y);
		CreateSideWalls (x, y);
	}

	private void CreateTopWall(int x, int y)
	{
		nextTileX = -lvTileSize.x / 2;
		nextTileZ = (float)y + lvTileSize.z / 2;
		
		for (float i = -lvTileSize.x; i < (x + lvTileSize.x); i += lvTileSize.x) {
			GameObject lvWall = GameObject.Instantiate (walls [0]);
			lvWall.transform.parent = this.gameObject.transform;
			lvWall.transform.position = new Vector3 (nextTileX,0.0f,nextTileZ);
			lvWall.transform.Rotate (0.0f, 90.0f, 0.0f);
			nextTileX += lvTileSize.x;
		}

	}

	private void CreateSideWalls(int x, int y)
	{
		nextTileX = -lvTileSize.x / 2;
		float lvNextTileXRight = (nextTileX * -1) + x;
		nextTileZ = -lvTileSize.z / 2;

		for (float i = -lvTileSize.z; i < (y + lvTileSize.z); i += lvTileSize.z) {
			GameObject lvWall = GameObject.Instantiate (walls [0]);
			lvWall.transform.parent = this.gameObject.transform;
			lvWall.transform.position = new Vector3 (nextTileX,0.0f,nextTileZ);
			lvWall.transform.Rotate (0.0f, 0.0f, 0.0f);

			GameObject lvWallRight = GameObject.Instantiate (walls [0]);
			lvWallRight.transform.parent = this.gameObject.transform;
			lvWallRight.transform.position = new Vector3 (lvNextTileXRight,0.0f,nextTileZ);
			lvWallRight.transform.Rotate (0.0f, 180.0f, 0.0f);

			nextTileZ += lvTileSize.z;
		}

	}


}
