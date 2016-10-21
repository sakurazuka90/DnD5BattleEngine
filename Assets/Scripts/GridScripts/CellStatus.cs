using UnityEngine;
using System.Collections;

public class CellStatus : MonoBehaviour
{

	public bool avaiable;

	private Material lvSelectedMaterial;
	private Material lvDeselectedMaterial;
	private Material lvMovableMaterial;
	private Material lvOpportunityMaterial;
	private Material lvTargetMaterial;
	private Material lvCloseRangedMaterial;
	private Material lvFarRangedMaterial;
	private Material lvSpawnPlayerMaterial;
	private Material lvSpawnEnemyMaterial;
	private Material lvSpawnPlayerMaterialSelected;
	private Material lvSpawnEnemyMaterialSelected;

	public FunctionalStates functionalState;
	private string function;
	private int figurineId;


	public bool movable = false;
	public bool lvOportunity = false;
	public bool selected = false;
	public bool target = false;
	public bool closeRange = false;
	public bool farRange = false;
	public bool spawnPlayer = false;
	public bool spawnEnemy = false;

	public bool edited = false;

	private Texture2D cursorTexture;
	private MeshRenderer cellMeshRenderer;

	public string Function{
		get{ return this.function; }
		set{ this.function = value;}
	}

	public int FigurineId{
		get{ return this.figurineId; }
		set{ this.figurineId = value;}
	}

	// Use this for initialization
	void Start ()
	{
		lvSelectedMaterial = Resources.Load<Material> ("Grid2");
		lvDeselectedMaterial = Resources.Load<Material> ("Grid1");
		lvMovableMaterial = Resources.Load<Material> ("Grid3");
		lvOpportunityMaterial = Resources.Load<Material> ("Grid4");
		lvTargetMaterial = Resources.Load<Material> ("Grid5");
		lvCloseRangedMaterial = Resources.Load<Material> ("Grid6");
		lvFarRangedMaterial = Resources.Load<Material> ("Grid7");
		lvSpawnPlayerMaterial = Resources.Load<Material> ("SpawnPlayerMaterial");
		lvSpawnEnemyMaterial = Resources.Load<Material> ("SpawnEnemyMaterial");
		lvSpawnPlayerMaterialSelected = Resources.Load<Material> ("SpawnPlayerMaterialSelected");
		lvSpawnEnemyMaterialSelected = Resources.Load<Material> ("SpawnEnemyMaterialSelected");


		functionalState = FunctionalStates.NONE;
		function = "";

		cellMeshRenderer = this.gameObject.GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		 if (avaiable) {
			if (this.selected)
				renderSelected ();
			else if (this.target)
				cellMeshRenderer.material = lvTargetMaterial;
			else if (this.farRange)
				cellMeshRenderer.material = lvFarRangedMaterial;
			else if (this.closeRange)
				cellMeshRenderer.material = lvCloseRangedMaterial;
			else if (this.lvOportunity)
				cellMeshRenderer.material = lvOpportunityMaterial;
			else if (this.movable)
				cellMeshRenderer.material = lvMovableMaterial;
			else if (this.spawnPlayer)
				cellMeshRenderer.material = lvSpawnPlayerMaterial;
			else if (this.spawnEnemy)
				cellMeshRenderer.material = lvSpawnEnemyMaterial;
			else if (FunctionalStates.NONE != functionalState)
				renderFunctional (edited);
			else
				cellMeshRenderer.material = lvDeselectedMaterial;
		}
	}


	public void ClearStatus ()
	{
		this.movable = false;
		this.lvOportunity = false;
		this.target = false;
		this.closeRange = false;
		this.farRange = false;
		this.functionalState = FunctionalStates.NONE;
		this.function = "";
		ClearTemporaryFunctionalStates ();
	}

	public void ClearTemporaryFunctionalStates()
	{
		this.spawnEnemy = false;
		this.spawnPlayer = false;
	}


	public bool Blocked {
		get {
			if (this.gameObject.transform.childCount > 0) {
				return this.gameObject.transform.GetChild (0).gameObject.GetComponent<ObstacleStatus> ().isBlockingMovement;
			} else
				return false;
		}
	}

	public void Deselect()
	{
		this.selected = false;
		ClearTemporaryFunctionalStates ();
	}

	public void SetState(CellStates state)
	{
		switch (state) {
			case CellStates.DISABLED:
				this.avaiable = false;
				break;
			case CellStates.ENABLED:
				this.avaiable = true;
				break;
			case CellStates.TARGET:
				this.target = true;
				break;
			case CellStates.OPPORTUNITY:
				this.lvOportunity = true;
				break;
			case CellStates.MOVABLE:
				this.movable = true;
				break;
			case CellStates.CLOSE_RANGE:
				this.closeRange = true;
				break;
			case CellStates.FAR_RANGE:
				this.farRange = true;
				break;	
			default:
				this.avaiable = true;
				break;
		}
	}


	private void renderSelected()
	{
		if (FunctionalStates.NONE != functionalState) {
			renderFunctional(true);
		} else {
			cellMeshRenderer.material = lvSelectedMaterial;
		}
	}

	private void renderFunctional(bool pmSelected)
	{
		if (!pmSelected) {
			switch (functionalState) {
			case FunctionalStates.PLAYER_SPAWN:
				cellMeshRenderer.material = lvSpawnPlayerMaterial;
				break;
			case FunctionalStates.ENEMY_SPAWN:
				cellMeshRenderer.material = lvSpawnEnemyMaterial;
				break;
			}
		} else {
			switch (functionalState) {
			case FunctionalStates.PLAYER_SPAWN:
				cellMeshRenderer.material = lvSpawnPlayerMaterialSelected;
				break;
			case FunctionalStates.ENEMY_SPAWN:
				cellMeshRenderer.material = lvSpawnEnemyMaterialSelected;
				break;
			}
		}
	}

}
