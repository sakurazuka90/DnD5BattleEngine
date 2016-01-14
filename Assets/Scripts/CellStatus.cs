using UnityEngine;
using System.Collections;

public class CellStatus : MonoBehaviour {

	public bool avaiable;

	public Material lvSelectedMaterial;
	public Material lvDeselectedMaterial;
	public Material lvMovableMaterial;
	public Material lvOpportunityMaterial;

	public bool movable = false;
	public bool lvOportunity = false;
	public bool selected;

	// Use this for initialization
	void Start () {
		lvSelectedMaterial = (Material)Resources.Load("Grid2", typeof(Material));
		lvDeselectedMaterial = (Material)Resources.Load("Grid1", typeof(Material));
		lvMovableMaterial = (Material)Resources.Load("Grid3", typeof(Material));
		lvOpportunityMaterial = (Material)Resources.Load("Grid4", typeof(Material));

	}
	
	// Update is called once per frame
	void Update () {
		MeshRenderer lvRenderer = this.gameObject.GetComponent<MeshRenderer>();
		if (avaiable) {
			if (this.selected)
				lvRenderer.material = lvSelectedMaterial;
			else if (this.lvOportunity)
				lvRenderer.material = lvOpportunityMaterial;
			else if (this.movable)
				lvRenderer.material = lvMovableMaterial;
			else
				lvRenderer.material = lvDeselectedMaterial;
		}
	}
}
