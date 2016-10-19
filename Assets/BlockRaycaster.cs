using UnityEngine;
using System.Collections;

public class BlockRaycaster : MonoBehaviour {

	private Material lvSelectedMaterial;
	private Material lvDeselectedMaterial;

	MeshRenderer blockRenderer;

	// Use this for initialization
	void Start () {
		lvSelectedMaterial = Resources.Load<Material> ("Grid2");
		lvDeselectedMaterial = Resources.Load<Material> ("Grid1");
		blockRenderer = this.gameObject.GetComponent<MeshRenderer> ();
		blockRenderer.material = lvDeselectedMaterial;
	}

	// Update is called once per frame
	void Update () {
		RaycastHit[] hits;
		bool didhit = false;
		hits = Physics.RaycastAll(transform.position, new Vector3(0.0F, -1.0F, 0.0F), 100.0F);
		foreach(RaycastHit hit in hits)
		{
			GameObject item = hit.collider.gameObject;
			if ("GridCell".Equals (item.tag)) {
				didhit = true;
			}
		}

		if (didhit)
			blockRenderer.material = lvDeselectedMaterial;
		else
			blockRenderer.material = lvSelectedMaterial;
	}
}
