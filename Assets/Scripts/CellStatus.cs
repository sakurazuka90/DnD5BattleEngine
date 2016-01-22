using UnityEngine;
using System.Collections;

public class CellStatus : MonoBehaviour {

	public bool avaiable;

	public Material lvSelectedMaterial;
	public Material lvDeselectedMaterial;
	public Material lvMovableMaterial;
	public Material lvOpportunityMaterial;
	public Material lvTargetMaterial;

	public bool movable = false;
	public bool lvOportunity = false;
	public bool selected = false;
	public bool target = false;

	private Texture2D cursorTexture;

	// Use this for initialization
	void Start () {
		lvSelectedMaterial = (Material)Resources.Load("Grid2", typeof(Material));
		lvDeselectedMaterial = (Material)Resources.Load("Grid1", typeof(Material));
		lvMovableMaterial = (Material)Resources.Load("Grid3", typeof(Material));
		lvOpportunityMaterial = (Material)Resources.Load("Grid4", typeof(Material));
		lvTargetMaterial = (Material)Resources.Load("Grid5", typeof(Material));

		cursorTexture = (Texture2D)Resources.Load("SWORD", typeof(Texture));

	}
	
	// Update is called once per frame
	void Update () {
		MeshRenderer lvRenderer = this.gameObject.GetComponent<MeshRenderer>();
		if (avaiable) {
			if (this.selected)
				lvRenderer.material = lvSelectedMaterial;
			else if (this.target)
				lvRenderer.material = lvTargetMaterial;
			else if (this.lvOportunity)
				lvRenderer.material = lvOpportunityMaterial;
			else if (this.movable)
				lvRenderer.material = lvMovableMaterial;
			else
				lvRenderer.material = lvDeselectedMaterial;
		}
	}

	/*void OnMouseEnter() {
		if(this.target)
			Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
	}
	void OnMouseExit() {
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}*/
}
