using UnityEngine;
using System.Collections;

public class BlockRaycaster : MonoBehaviour {

	private Material occupiedFieldMaterial;
	private Material freeFieldMaterial;

	MeshRenderer blockRenderer;

	// Use this for initialization
	void Start () {
		occupiedFieldMaterial = Resources.Load<Material> ("BlockRed");
		freeFieldMaterial = Resources.Load<Material> ("BlockGreen");
		blockRenderer = this.gameObject.GetComponent<MeshRenderer> ();
		blockRenderer.material = freeFieldMaterial;
	}

	// Update is called once per frame
	void Update () {
		if (IsFieldFreeRaycast())
			blockRenderer.material = freeFieldMaterial;
		else
			blockRenderer.material = occupiedFieldMaterial;
	}

	public bool IsFieldFreeRaycast()
	{
		RaycastHit[] hits;
		bool isFieldFree = false;
		hits = Physics.RaycastAll(transform.position, new Vector3(0.0F, -1.0F, 0.0F), 100.0F);
		foreach(RaycastHit hit in hits)
		{
			GameObject item = hit.collider.gameObject;
			if ("GridCell".Equals (item.tag)) {
				isFieldFree = true;
			}
		}

		return isFieldFree;
	}
}
