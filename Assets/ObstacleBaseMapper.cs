using UnityEngine;
using System.Collections;

public class ObstacleBaseMapper : MonoBehaviour {

	public GameObject block;
	public bool[] mapping = new bool[9];

	// Use this for initialization
	void Start () {
		mapping [4] = true;
		PlaceBlocks ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void PlaceBlocks()
	{
		float zMove = -1.0F;

		int level = 0;

		for (int j = 0; j < 3; j++) {
			float xMove = -1.0F;
			for (int i = 0; i < 3; i++) {
				if (mapping [i + (level * 3)]) {
					GameObject instance = Instantiate (block);
					instance.transform.SetParent (this.gameObject.transform.GetChild(0));
					instance.transform.Translate (new Vector3 (xMove, 1.0f, zMove));
				}
				xMove += 1.0F;
			}
			zMove += 1.0F;
			level++;
		}
	}
}
