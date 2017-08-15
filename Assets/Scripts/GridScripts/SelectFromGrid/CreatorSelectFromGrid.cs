using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatorSelectFromGrid : AbstractSelectFromGrid
{
	public static CreatorSelectFromGrid instance;

	public bool functionalPlaceMode = false;
	public FunctionalStates currentFunctionalState;
	public GameObject creatorObstacle;

	private List<int> constructorFilledSquares;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
	}

	void Start()
	{
		constructorFilledSquares = new List<int> ();
		currentFunctionalState = FunctionalStates.NONE;
	}

	protected override void RaycastAction (GameObject gridCell)
	{
		CellStatus lvCellStatus = gridCell.GetComponent<CellStatus> ();
		handleMouseOverCell (lvCellStatus);

		Vector3 lvPosition = gridCell.transform.position;
		int lvCellId = GridDrawer.instance.GetGridId ((int)lvPosition.x, (int)lvPosition.z);

		GameObject figurine = creatorObstacle;
		FigurineStatus figurineStatus = null;

		GridDrawer.instance.SetStateToCells (constructorFilledSquares, CellStates.ENABLED);

		if (figurine != null)
			figurineStatus = figurine.GetComponent<FigurineStatus> ();

		if (functionalPlaceMode) {
			FunctionalPlaceModeAction(lvCellStatus);
		} else if (figurineStatus != null && figurineStatus.picked) {
			PlaceObstacleAction (figurine, gridCell);
		} else {
			SelectField (gridCell);
		}

	}

	private void SelectField(GameObject gridCell)
	{
		CellStatus lvCellStatus = gridCell.GetComponent<CellStatus> ();
		if (Input.GetMouseButtonDown (0)) {
			if (gridCell.transform.childCount > 0) {
				ObstacleStatus lvObstacleStatus = gridCell.transform.GetChild (0).GetComponent<ObstacleStatus> ();
				AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();

				if (lvStatusEditor.obstacleStatus != null && !"".Equals (lvStatusEditor.obstacleStatus.name)) {
					GameObject lvOldObstacle = GameObject.Find (lvStatusEditor.obstacleStatus.name);
					lvOldObstacle.GetComponent<ShaderSwitcher> ().SwitchOutlineOff ();
				}

				lvStatusEditor.obstacleStatus = lvObstacleStatus;
				lvStatusEditor.populate ();

				gridCell.transform.GetChild (0).GetComponent<ShaderSwitcher> ().SwitchOutlineOn ();
			} else {

				AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();

				if (lvStatusEditor.obstacleStatus != null && !"".Equals (lvStatusEditor.obstacleStatus.name)) {
					GameObject lvOldObstacle = GameObject.Find (lvStatusEditor.obstacleStatus.name);
					lvOldObstacle.GetComponent<ShaderSwitcher> ().SwitchOutlineOff ();
				}

				lvStatusEditor.clear ();
			}


			FunctionalStates lvState = lvCellStatus.functionalState;

			if (lvState != FunctionalStates.NONE) {

				string lvFunctionalName = "";
				string lvFunctionalButtonName = "";

				switch (lvState) {
				case FunctionalStates.PLAYER_SPAWN:
					lvFunctionalName = "Player Spawn Point";
					lvFunctionalButtonName = "SpawnPlayersSprite";
					break;
				case FunctionalStates.ENEMY_SPAWN:
					lvFunctionalName = "Enemy Spawn Point";
					lvFunctionalButtonName = "SpawnPlayersSprite2";
					break;
				}

				AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();
				lvStatusEditor.clearFunctional ();
				lvStatusEditor.populateFunctional (lvFunctionalName, lvFunctionalButtonName, lvCellStatus, lvCellStatus.Function);

			} else {
				AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();
				lvStatusEditor.clearFunctional ();

			}
		}
	}

	private void handleMouseOverCell (CellStatus pmCellStatus)
	{
		if (!functionalPlaceMode) {
			pmCellStatus.selected = true;
		} else if (functionalPlaceMode) {
			
			switch (currentFunctionalState) {
			case FunctionalStates.PLAYER_SPAWN:
				pmCellStatus.spawnPlayer = true;
				break;
			case FunctionalStates.ENEMY_SPAWN:
				pmCellStatus.spawnEnemy = true;
				break;
			}
		} else if (pmCellStatus.movable) {
			pmCellStatus.selected = true;
		}
		lastStatus = pmCellStatus;
	}

	private void FunctionalPlaceModeAction(CellStatus cellStatus)
	{
		if (Input.GetMouseButtonDown (0)) {
			cellStatus.functionalState = currentFunctionalState;
			functionalPlaceMode = false;
			currentFunctionalState = FunctionalStates.NONE;

			cellStatus.ClearTemporaryFunctionalStates ();

			AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();
			lvStatusEditor.clearFunctional ();

			string lvFunctionalName = "";
			string lvFunctionalButtonName = "";

			switch (cellStatus.functionalState) {
			case FunctionalStates.PLAYER_SPAWN:
				lvFunctionalName = "Player Spawn Point";
				lvFunctionalButtonName = "SpawnPlayersSprite";
				break;
			case FunctionalStates.ENEMY_SPAWN:
				lvFunctionalName = "Enemy Spawn Point";
				lvFunctionalButtonName = "SpawnPlayersSprite2";
				break;
			}

			lvStatusEditor.populateFunctional (lvFunctionalName, lvFunctionalButtonName, cellStatus);
		}
	}

	private void PlaceObstacleAction(GameObject figurine, GameObject gridCell)
	{
		Vector3 lvPosition = gridCell.transform.position;
		int lvCellId = GridDrawer.instance.GetGridId ((int)lvPosition.x, (int)lvPosition.z);
		FigurineStatus status = figurine.GetComponent<FigurineStatus> ();
		GridDrawer.instance.SetStateToCells(this.constructorFilledSquares, CellStates.DISABLED);
		FigurineMover lvMover = figurine.GetComponent<FigurineMover> ();
		lvMover.gridX = (int)lvPosition.x;
		lvMover.gridZ = (int)lvPosition.z;


		if (Input.GetMouseButtonDown (0) && figurine.GetComponent<ObstacleBaseMapper> ().IsObstacleOnLegalFields ()) {
			
			status.picked = false;
			status.gridX = (int)lvPosition.x;
			status.gridZ = (int)lvPosition.z;
			List<int> fields = figurine.GetComponent<ObstacleBaseMapper> ().GetObstacleFields ();
			PutObstacleOnField (fields);

			ObstacleStatus obstacleStatus = figurine.GetComponent<ObstacleStatus> ();
			obstacleStatus.fieldsUsed = fields;
			figurine.transform.parent = GridDrawer.instance.mCells [lvCellId].transform;

		}

		if (Input.GetMouseButtonDown (1)) {
			figurine.GetComponent<ObstacleRotator> ().Rotate (90.0f);
		}
	}

	public void PutObstacleOnField (List<int> pmCells)
	{
		this.constructorFilledSquares.AddRange (pmCells);
	}

	public void RemoveObstacleFromField(List<int> pmCellId)
	{
		foreach (int id in pmCellId) {
			if(this.constructorFilledSquares.Contains(id))
				this.constructorFilledSquares.Remove (id);
		}
	}

	protected override bool RaycastActionCondition ()
	{
		bool uiChecker = EventSystem.current.IsPointerOverGameObject ();
		return !uiChecker;
	}
}

