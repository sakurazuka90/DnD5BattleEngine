using UnityEngine;
using System.Collections;

public class FloorCreator : MonoBehaviour {

	public GameObject floorTile;

	public void CreateFloor(int x, int y)
	{
		Vector3 lvTileSize = floorTile.GetComponent<MeshRenderer> ().bounds.size;
		float nextTileX = -lvTileSize.x / 2;
		float nextTileZ = -lvTileSize.z / 2;

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
}
