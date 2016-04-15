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

	public FunctionalStates functionalState;

	public bool movable = false;
	public bool lvOportunity = false;
	public bool selected = false;
	public bool target = false;
	public bool closeRange = false;
	public bool farRange = false;
	public bool spawnPlayer = false;
	public bool spawnEnemy = false;

	private Texture2D cursorTexture;

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
		functionalState = FunctionalStates.NONE;
	}
	
	// Update is called once per frame
	void Update ()
	{
		MeshRenderer lvRenderer = this.gameObject.GetComponent<MeshRenderer> ();

		if (FunctionalStates.NONE != functionalState) {

			switch (functionalState) {
			case FunctionalStates.PLAYER_SPAWN:
				lvRenderer.material = lvSpawnPlayerMaterial;
				break;
			case FunctionalStates.ENEMY_SPAWN:
				lvRenderer.material = lvSpawnEnemyMaterial;
				break;
				
			}

		} else if (avaiable) {
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
			else if (this.spawnPlayer)
				lvRenderer.material = lvSpawnPlayerMaterial;
			else if (this.spawnEnemy)
				lvRenderer.material = lvSpawnEnemyMaterial;
			else
				lvRenderer.material = lvDeselectedMaterial;
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
}
