using UnityEngine;
using System.Collections;

public class FloorCreator : MonoBehaviour {

	public GameObject floorTile;

	public void CreateFloor(int x, int y)
	{
		Vector3 lvTileSize = floorTile.GetComponent<MeshRenderer> ().bounds.size;
		float nextTileX = lvTileSize.x / 2;
		float nextTileZ = lvTileSize.z / 2;

		for (float j = 0; j < y; j += lvTileSize.z) {
			for (float i = 0; i < x; i += lvTileSize.x) {
				GameObject lvTile = GameObject.Instantiate (floorTile);
				lvTile.transform.position = new Vector3 (nextTileX,0.0f,nextTileZ);
				nextTileX += lvTileSize.x;
			}
			nextTileX = lvTileSize.x / 2;
			nextTileZ += lvTileSize.z;

		}


	}
}
