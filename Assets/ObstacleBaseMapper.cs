using UnityEngine;
using System.Collections.Generic;

public class ObstacleBaseMapper : MonoBehaviour {

	public GameObject blockPrefab;
	public bool[] mapping = new bool[9];

	private List<GameObject> blocks;

	// Use this for initialization
	void Start () {
		blocks = new List<GameObject> ();
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
					GameObject instance = Instantiate (blockPrefab);
					instance.transform.SetParent (this.gameObject.transform.GetChild(0));
					instance.transform.Translate (new Vector3 (xMove, 1.0f, zMove));
					blocks.Add (instance);
				}
				xMove += 1.0F;
			}
			zMove += 1.0F;
			level++;
		}
	}

	public bool IsObstacleOnLegalFields()
	{
		bool legal = true;

		foreach (GameObject block in blocks) {
			BlockRaycaster raycaster = block.GetComponent<BlockRaycaster> ();

			if (raycaster != null) {
				legal = legal && raycaster.IsFieldFreeRaycast ();
			}
		}

		return legal;
	}

	public List<int> GetObstacleFields()
	{
		List<int> fields = new List<int> ();

		foreach (GameObject block in blocks) {
			BlockRaycaster raycaster = block.GetComponent<BlockRaycaster> ();

			if (raycaster != null) {
				int? field = raycaster.GetRaycastFieldId ();
				if (field != null)
					fields.Add (field.Value);
			}
		}

		return fields;
	}


}
