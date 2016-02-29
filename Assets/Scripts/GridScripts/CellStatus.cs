using UnityEngine;
using System.Collections;

public class CellStatus : MonoBehaviour {

	public bool avaiable;

	public Material lvSelectedMaterial;
	public Material lvDeselectedMaterial;
	public Material lvMovableMaterial;
	public Material lvOpportunityMaterial;
	public Material lvTargetMaterial;
	public Material lvCloseRangedMaterial;
	public Material lvFarRangedMaterial;


	public bool movable = false;
	public bool lvOportunity = false;
	public bool selected = false;
	public bool target = false;
	public bool closeRange = false;
	public bool farRange = false;

	private Texture2D cursorTexture;

	// Use this for initialization
	void Start () {
		lvSelectedMaterial = (Material)Resources.Load("Grid2", typeof(Material));
		lvDeselectedMaterial = (Material)Resources.Load("Grid1", typeof(Material));
		lvMovableMaterial = (Material)Resources.Load("Grid3", typeof(Material));
		lvOpportunityMaterial = (Material)Resources.Load("Grid4", typeof(Material));
		lvTargetMaterial = (Material)Resources.Load("Grid5", typeof(Material));
		lvCloseRangedMaterial = (Material)Resources.Load("Grid6", typeof(Material));
		lvFarRangedMaterial = (Material)Resources.Load("Grid7", typeof(Material));

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
			else if (this.farRange)
				lvRenderer.material = lvFarRangedMaterial;
			else if (this.closeRange)
				lvRenderer.material = lvCloseRangedMaterial;
			else if (this.lvOportunity)
				lvRenderer.material = lvOpportunityMaterial;
			else if (this.movable)
				lvRenderer.material = lvMovableMaterial;
			else
				lvRenderer.material = lvDeselectedMaterial;
		}
	}

	public void ClearStatus()
	{
		this.movable = false;
		this.lvOportunity = false;
		this.target = false;
		this.closeRange = false;
		this.farRange = false;
	}

	/*void OnMouseEnter() {
		if(this.target)
			Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
	}
	void OnMouseExit() {
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}*/
}
