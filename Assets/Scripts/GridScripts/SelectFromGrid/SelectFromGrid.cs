using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SelectFromGrid : AbstractSelectFromGrid
{

	private static string XPOSITION = "XPOSITION";
	private static string ZPOSITION = "ZPOSITION";
	private static string PARENT_PATH = "PARENT_PATH";
	private static string STRAIGHT_LINE = "STRAIGHT_LINE";
	private static string IS_DIFFICULT = "DIFFICULT_TERRAIN";

	public bool mMoveMode = false;
	private bool mTargetMode = false;
	public bool mCreatorMode = false;
	public bool functionalPlaceMode = false;

	private Dictionary<string, string> mPaths;

	public GameObject creatorObstacle;

	private List<int> mPlayersFields;
	private List<int> mMonsterFields;
	private List<int> mOpportunityFields;
	private int mActivePlayerStartField;

	public bool playersTurn;

	public bool inventoryOpen = false;

	private List<int> constructorFilledSquares;

	public static SelectFromGrid instance;

	public FunctionalStates currentFunctionalState;

	public string Function {
		get;
		set;
	}

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
	}


	// Use this for initialization
	void Start ()
	{
		mPaths = new Dictionary<string, string> ();
		updateFields ();

		constructorFilledSquares = new List<int> ();

		currentFunctionalState = FunctionalStates.NONE;

	}


	protected override void RaycastAction (GameObject gridCell)
	{

		CellStatus lvCellStatus = gridCell.GetComponent<CellStatus> ();

		handleMouseOverCell (lvCellStatus);

		Vector3 lvPosition = gridCell.transform.position;
		int lvCellId = GridDrawer.instance.GetGridId ((int)lvPosition.x, (int)lvPosition.z);

		if (mMoveMode && lvCellStatus.movable) {
			string lvId = lvCellId.ToString ();
			DrawWalkableLine (mPaths [lvId]);
		}


		GameObject lvFigurine;
		FigurineStatus lvStatus = null;

		lvFigurine = PlayerSpooler.instance.mSpooledObject;

		if (lvFigurine != null)
			lvStatus = lvFigurine.GetComponent<FigurineStatus> ();

		if (mMoveMode) {
			MoveModeAction (lvCellId, lvFigurine);
		} else if (mTargetMode) {
			if (Input.GetMouseButtonDown (0) && lvCellStatus.target) {
				Player lvTarget = PlayerSpooler.instance.GetPlayerOnField (lvCellId);
				PlayerSpooler.instance.ResolveSpooledAttack (lvTarget);
				mTargetMode = false;
			}

		} 

	}

	protected override bool RaycastActionCondition ()
	{
		return !inventoryOpen && !EventSystem.current.IsPointerOverGameObject ();
	}


	private void MoveModeAction (int cellId, GameObject figurine)
	{
		if (Input.GetMouseButtonDown (0)) {
			FigurineMover lvMover = figurine.GetComponent<FigurineMover> ();

			string lvId = cellId.ToString ();
			if (mPaths.ContainsKey (lvId)) {
				lvMover.movePoints = PlayerSpooler.instance.GetSpooledPlayer ().movesLeft;
				lvMover.path = mPaths [lvId];
				lvMover.isMoving = true;

				GridDrawer.instance.ClearGridStatus ();

				mMoveMode = false;
				ClearWalkableLine ();
				mPaths = new Dictionary<string, string> ();
			}
		} else if (Input.GetMouseButtonDown (1)) {
			mMoveMode = false;
			GridDrawer.instance.ClearGridStatus ();

			mMoveMode = false;
			ClearWalkableLine ();
			mPaths = new Dictionary<string, string> ();
			FigurineMover lvMover = figurine.GetComponent<FigurineMover> ();
			lvMover.AbortMovement ();
		}
	}

	private void handleMouseOverCell (CellStatus pmCellStatus)
	{
		if (!mMoveMode && !mTargetMode && !functionalPlaceMode) {
			pmCellStatus.selected = true;
		} else if (mTargetMode) {
			if (pmCellStatus.target)
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

		} else {
			if (pmCellStatus.movable)
				pmCellStatus.selected = true;
		}
		lastStatus = pmCellStatus;
	}

	public void DeactivateWalking ()
	{
		PlayerSpooler lvSpooler = GameObject.Find ("PlayerSpooler").GetComponent<PlayerSpooler> ();

		GameObject lvFigurine = lvSpooler.mSpooledObject;
		mMoveMode = false;
		GridDrawer.instance.ClearGridStatus ();

		mMoveMode = false;
		ClearWalkableLine ();
		mPaths = new Dictionary<string, string> ();
		FigurineMover lvMover = lvFigurine.GetComponent<FigurineMover> ();
		lvMover.AbortMovement ();
	}

	public void DrawWalkableLine (string pmPath)
	{
		string[] lvPathSteps = pmPath.Split ('_');

		GameObject lvLineRendererObject = GameObject.Find ("MovementLineDrawer");
		LineRenderer lvRenderer = lvLineRendererObject.GetComponent<LineRenderer> ();

		if (lvPathSteps.Length > 1) {
			lvRenderer.SetVertexCount (lvPathSteps.Length);

			for (int i = 0; i < lvPathSteps.Length; i++) {
				int lvIndex = int.Parse (lvPathSteps [i]);

				float lvGridX = lvIndex % GridDrawer.instance.gridWidth;
				float lvGridZ = (lvIndex - lvGridX) / GridDrawer.instance.gridWidth;

				lvRenderer.SetPosition (i, new Vector3 (lvGridX + 0.5f, 0.2f, lvGridZ + 0.5f));
			}

		} else {
			lvRenderer.SetVertexCount (0);
		}

	}

	public void ClearWalkableLine ()
	{
		GameObject lvLineRendererObject = GameObject.Find ("MovementLineDrawer");
		LineRenderer lvRenderer = lvLineRendererObject.GetComponent<LineRenderer> ();

		lvRenderer.SetVertexCount (0);

	}

	// Funkcja wyswietla na gridzie zasieg ruchu postaci
	public void ShowWalkableDistance (int gridX, int gridZ, int moveDist)
	{
		mMoveMode = true;

		if (playersTurn)
			mOpportunityFields = findOpportunityFields (mMonsterFields);
		else
			mOpportunityFields = findOpportunityFields (mPlayersFields);

		string lvStartCellId = (GridDrawer.instance.gridWidth * gridZ + gridX).ToString ();

		mPaths.Add (lvStartCellId, lvStartCellId);

		Dictionary<string,string> lvCellData = new Dictionary<string,string> ();
		lvCellData.Add (XPOSITION, gridX.ToString ());
		lvCellData.Add (ZPOSITION, gridZ.ToString ());
		lvCellData.Add (PARENT_PATH, "");
		lvCellData.Add (STRAIGHT_LINE, "Y");


		Dictionary<string,Dictionary<string,string>> lvNextLevel = new Dictionary<string, Dictionary<string, string>> ();

		lvNextLevel.Add (GridDrawer.instance.GetGridId (gridX, gridZ).ToString (), lvCellData);

		Dictionary<string,Dictionary<string,string>> lastHardTerrain = new Dictionary<string, Dictionary<string, string>> ();
		Dictionary<string,Dictionary<string,string>> currentHardTerrain = new Dictionary<string, Dictionary<string, string>> ();

		while (moveDist >= 0) {
			foreach (string lvCurrentHardTerrainKey in currentHardTerrain.Keys) {
				if (!lvNextLevel.ContainsKey (lvCurrentHardTerrainKey))
					lvNextLevel.Add (lvCurrentHardTerrainKey, currentHardTerrain [lvCurrentHardTerrainKey]);
			}

			currentHardTerrain.Clear ();

			currentHardTerrain = lastHardTerrain;

			lastHardTerrain = new Dictionary<string, Dictionary<string, string>> ();
			lvNextLevel = returnLevel (lvNextLevel, lastHardTerrain, mPaths, moveDist);
			moveDist--;
		}

	}



	private Dictionary<string,Dictionary<string,string>> returnLevel (Dictionary<string,Dictionary<string,string>> pmNextLevel, Dictionary<string,Dictionary<string,string>> pmLastDiffTerrain, Dictionary<string,string> pmPaths, int pmMoveDist)
	{

		Dictionary<string,Dictionary<string,string>> lvNextLevel = new Dictionary<string, Dictionary<string, string>> ();
		Dictionary<string,Dictionary<string,string>> lvTempLevel = new Dictionary<string, Dictionary<string, string>> ();

		foreach (string lvKey in pmNextLevel.Keys) {
			Dictionary<string,string> lvValue = pmNextLevel [lvKey];

			lvTempLevel = resolveForCell (pmPaths, int.Parse (lvValue [XPOSITION]), int.Parse (lvValue [ZPOSITION]), lvValue [PARENT_PATH], pmMoveDist);

			foreach (string lvKeyTemp in lvTempLevel.Keys) {
				if (!lvNextLevel.ContainsKey (lvKeyTemp)) {

					Dictionary<string,string> lvData = lvTempLevel [lvKeyTemp];

					if ("Y".Equals (lvData [IS_DIFFICULT]) && !pmLastDiffTerrain.ContainsKey (lvKeyTemp))
						pmLastDiffTerrain.Add (lvKeyTemp, lvData);
					else if ("N".Equals (lvData [IS_DIFFICULT]))
						lvNextLevel.Add (lvKeyTemp, lvData);
				}
			}

		}

		return lvNextLevel;
	}

	/*
	 *  
	 */
	private Dictionary<string,Dictionary<string,string>> resolveForCell (Dictionary<string,string> pmPaths, int pmGridX, int pmGridZ, string pmPath, int pmMoveDist)
	{
		GameObject lvCell = GridDrawer.instance.mCells [GridDrawer.instance.gridWidth * pmGridZ + pmGridX];

		string lvParentId = "" + (GridDrawer.instance.gridWidth * pmGridZ + pmGridX);

		string lvCellPath = "";

		if (pmPath.Length > 0)
			lvCellPath += pmPath + "_" + lvParentId;
		else
			lvCellPath = lvParentId;

		//if (lvCell.transform.childCount > 0) {
		//	ObstacleStatus lvObstacleStatus = lvCell.transform.GetChild (0);
		//}

		
		if (!IsAllyField (int.Parse (lvParentId)) || mActivePlayerStartField == int.Parse (lvParentId)) {

			CellStatus lvStatus = lvCell.GetComponent<CellStatus> ();
			
			// na polu sojusznika nie stajemy. Ale możemy przez nie przejść
			lvStatus.movable = true;

			if (mOpportunityFields.Contains (int.Parse (lvParentId))) {
				lvStatus.lvOportunity = true;
			}

			if (pmPaths.ContainsKey (lvParentId)) {

				string[] lvItemsOn = pmPaths [lvParentId].Split ('_');
				string[] lvItemsNew = lvCellPath.Split ('_');

				if (lvItemsNew.Length < lvItemsOn.Length) {
					pmPaths.Add (lvParentId, lvCellPath);
				}
			} else {
				pmPaths.Add (lvParentId, lvCellPath);
			}
		}

		Dictionary<string,Dictionary<string,string>> lvMainDictionary = new Dictionary<string,Dictionary<string,string>> ();


		if (pmMoveDist > 0) {

			bool lvTopGood = (pmGridZ + 1) < GridDrawer.instance.gridHeight;
			bool lvBottomGood = (pmGridZ - 1) > -1;
			
			bool lvRightGood = (pmGridX + 1) < GridDrawer.instance.gridWidth;
			bool lvLeftGood = (pmGridX - 1) > -1;

			bool lvTopMovable = false;
			bool lvBotMovable = false;
			bool lvRightMovable = false;
			bool lvLeftMovable = false;


			if (lvTopGood) {

				int cellZ = pmGridZ + 1;

				GameObject lvCellUpp = GridDrawer.instance.mCells [GridDrawer.instance.gridWidth * cellZ + pmGridX];
				CellStatus lvStatusUpp = lvCellUpp.GetComponent<CellStatus> ();
				string lvCellId = "" + (GridDrawer.instance.gridWidth * cellZ + pmGridX);

				if (!lvStatusUpp.Blocked && !IsEnemyField (int.Parse (lvCellId))) {
					string cellGridZ = cellZ.ToString ();
					string cellGridX = pmGridX.ToString ();

					Dictionary<string,string> lvCellData = new Dictionary<string,string> ();
					lvCellData.Add (XPOSITION, cellGridX);
					lvCellData.Add (ZPOSITION, cellGridZ);
					lvCellData.Add (PARENT_PATH, lvCellPath);
					lvCellData.Add (STRAIGHT_LINE, "Y");

					if (GridDrawer.IsCellDifficultTerrain (lvCellUpp)) {
						lvCellData.Add (IS_DIFFICULT, "Y");
					} else {
						lvCellData.Add (IS_DIFFICULT, "N");
					}

					lvMainDictionary.Add (lvCellId, lvCellData);

					lvTopMovable = true;
				}
			}
			
			if (lvBottomGood) {
				int cellZ = pmGridZ - 1;
				
				GameObject lvCellBott = GridDrawer.instance.mCells [GridDrawer.instance.gridWidth * cellZ + pmGridX];
				CellStatus lvStatusBott = lvCellBott.GetComponent<CellStatus> ();
				string lvCellId = "" + (GridDrawer.instance.gridWidth * cellZ + pmGridX);
				
				if (!lvStatusBott.Blocked && !IsEnemyField (int.Parse (lvCellId))) {
					string cellGridZ = cellZ.ToString ();
					string cellGridX = pmGridX.ToString ();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string> ();
					lvCellData.Add (XPOSITION, cellGridX);
					lvCellData.Add (ZPOSITION, cellGridZ);
					lvCellData.Add (PARENT_PATH, lvCellPath);
					lvCellData.Add (STRAIGHT_LINE, "Y");

					if (GridDrawer.IsCellDifficultTerrain (lvCellBott)) {
						lvCellData.Add (IS_DIFFICULT, "Y");
					} else {
						lvCellData.Add (IS_DIFFICULT, "N");
					}
					
					lvMainDictionary.Add (lvCellId, lvCellData);
					
					lvBotMovable = true;
				}
			}
			
			if (lvRightGood) {
				int cellX = pmGridX + 1;
				
				GameObject lvCellRight = GridDrawer.instance.mCells [GridDrawer.instance.gridWidth * pmGridZ + cellX];
				CellStatus lvStatusRight = lvCellRight.GetComponent<CellStatus> ();
				string lvCellId = "" + (GridDrawer.instance.gridWidth * pmGridZ + cellX);
				
				if (!lvStatusRight.Blocked && !IsEnemyField (int.Parse (lvCellId))) {
					string cellGridX = cellX.ToString ();
					string cellGridZ = pmGridZ.ToString ();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string> ();
					lvCellData.Add (XPOSITION, cellGridX);
					lvCellData.Add (ZPOSITION, cellGridZ);
					lvCellData.Add (PARENT_PATH, lvCellPath);
					lvCellData.Add (STRAIGHT_LINE, "Y");

					if (GridDrawer.IsCellDifficultTerrain (lvCellRight)) {
						lvCellData.Add (IS_DIFFICULT, "Y");
					} else {
						lvCellData.Add (IS_DIFFICULT, "N");
					}
					
					lvMainDictionary.Add (lvCellId, lvCellData);
					
					lvRightMovable = true;
				}
			}
			
			if (lvLeftGood) {
				int cellX = pmGridX - 1;
				
				GameObject lvCellLeft = GridDrawer.instance.mCells [GridDrawer.instance.gridWidth * pmGridZ + cellX];
				CellStatus lvStatusLeft = lvCellLeft.GetComponent<CellStatus> ();
				string lvCellId = "" + (GridDrawer.instance.gridWidth * pmGridZ + cellX);
				
				if (!lvStatusLeft.Blocked && !IsEnemyField (int.Parse (lvCellId))) {
					string cellGridX = cellX.ToString ();
					string cellGridZ = pmGridZ.ToString ();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string> ();
					lvCellData.Add (XPOSITION, cellGridX);
					lvCellData.Add (ZPOSITION, cellGridZ);
					lvCellData.Add (PARENT_PATH, lvCellPath);
					lvCellData.Add (STRAIGHT_LINE, "Y");

					if (GridDrawer.IsCellDifficultTerrain (lvCellLeft)) {
						lvCellData.Add (IS_DIFFICULT, "Y");
					} else {
						lvCellData.Add (IS_DIFFICULT, "N");
					}
					
					lvMainDictionary.Add (lvCellId, lvCellData);
					
					lvLeftMovable = true;
				}
			}
			
			if (lvTopMovable && lvRightMovable) {
				int cellX = pmGridX + 1;
				int cellZ = pmGridZ + 1;
				
				GameObject lvCellTopRight = GridDrawer.instance.mCells [GridDrawer.instance.gridWidth * cellZ + cellX];
				CellStatus lvStatusTopRight = lvCellTopRight.GetComponent<CellStatus> ();
				string lvCellId = "" + (GridDrawer.instance.gridWidth * cellZ + cellX);
				
				if (!lvStatusTopRight.Blocked && !IsEnemyField (int.Parse (lvCellId))) {
					string cellGridX = cellX.ToString ();
					string cellGridZ = cellZ.ToString ();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string> ();
					lvCellData.Add (XPOSITION, cellGridX);
					lvCellData.Add (ZPOSITION, cellGridZ);
					lvCellData.Add (PARENT_PATH, lvCellPath);
					lvCellData.Add (STRAIGHT_LINE, "N");

					if (GridDrawer.IsCellDifficultTerrain (lvCellTopRight)) {
						lvCellData.Add (IS_DIFFICULT, "Y");
					} else {
						lvCellData.Add (IS_DIFFICULT, "N");
					}
					
					lvMainDictionary.Add (lvCellId, lvCellData);
				}
			}
			
			if (lvTopMovable && lvLeftMovable) {
				int cellX = pmGridX - 1;
				int cellZ = pmGridZ + 1;
				
				GameObject lvCellTopLeft = GridDrawer.instance.mCells [GridDrawer.instance.gridWidth * cellZ + cellX];
				CellStatus lvStatusTopLeft = lvCellTopLeft.GetComponent<CellStatus> ();
				string lvCellId = "" + (GridDrawer.instance.gridWidth * cellZ + cellX);
				
				if (!lvStatusTopLeft.Blocked && !IsEnemyField (int.Parse (lvCellId))) {
					string cellGridX = cellX.ToString ();
					string cellGridZ = cellZ.ToString ();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string> ();
					lvCellData.Add (XPOSITION, cellGridX);
					lvCellData.Add (ZPOSITION, cellGridZ);
					lvCellData.Add (PARENT_PATH, lvCellPath);
					lvCellData.Add (STRAIGHT_LINE, "N");

					if (GridDrawer.IsCellDifficultTerrain (lvCellTopLeft)) {
						lvCellData.Add (IS_DIFFICULT, "Y");
					} else {
						lvCellData.Add (IS_DIFFICULT, "N");
					}
					
					lvMainDictionary.Add (lvCellId, lvCellData);
				}
			}
			
			if (lvBotMovable && lvRightMovable) {
				int cellX = pmGridX + 1;
				int cellZ = pmGridZ - 1;
				
				GameObject lvCellBotRight = GridDrawer.instance.mCells [GridDrawer.instance.gridWidth * cellZ + cellX];
				CellStatus lvStatusBotRight = lvCellBotRight.GetComponent<CellStatus> ();
				string lvCellId = "" + (GridDrawer.instance.gridWidth * cellZ + cellX);
				
				if (!lvStatusBotRight.Blocked && !IsEnemyField (int.Parse (lvCellId))) {
					string cellGridX = cellX.ToString ();
					string cellGridZ = cellZ.ToString ();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string> ();
					lvCellData.Add (XPOSITION, cellGridX);
					lvCellData.Add (ZPOSITION, cellGridZ);
					lvCellData.Add (PARENT_PATH, lvCellPath);
					lvCellData.Add (STRAIGHT_LINE, "N");

					if (GridDrawer.IsCellDifficultTerrain (lvCellBotRight)) {
						lvCellData.Add (IS_DIFFICULT, "Y");
					} else {
						lvCellData.Add (IS_DIFFICULT, "N");
					}
					
					lvMainDictionary.Add (lvCellId, lvCellData);
				}
			}
			
			if (lvBotMovable && lvLeftMovable) {
				int cellX = pmGridX - 1;
				int cellZ = pmGridZ - 1;
				
				GameObject lvCellBotLeft = GridDrawer.instance.mCells [GridDrawer.instance.gridWidth * cellZ + cellX];
				CellStatus lvStatusBotLeft = lvCellBotLeft.GetComponent<CellStatus> ();
				string lvCellId = "" + (GridDrawer.instance.gridWidth * cellZ + cellX);
				
				if (!lvStatusBotLeft.Blocked && !IsEnemyField (int.Parse (lvCellId))) {
					string cellGridX = cellX.ToString ();
					string cellGridZ = cellZ.ToString ();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string> ();
					lvCellData.Add (XPOSITION, cellGridX);
					lvCellData.Add (ZPOSITION, cellGridZ);
					lvCellData.Add (PARENT_PATH, lvCellPath);
					lvCellData.Add (STRAIGHT_LINE, "N");

					if (GridDrawer.IsCellDifficultTerrain (lvCellBotLeft)) {
						lvCellData.Add (IS_DIFFICULT, "Y");
					} else {
						lvCellData.Add (IS_DIFFICULT, "N");
					}
					
					lvMainDictionary.Add (lvCellId, lvCellData);
				}
			}
		}

		return lvMainDictionary;

	}

	public void updateFields ()
	{
		GameObject[] lvFigurines = GameObject.FindGameObjectsWithTag ("Figurine");

		mPlayersFields = new List<int> ();
		mMonsterFields = new List<int> ();

		foreach (GameObject lvFigurine in lvFigurines) {

			FigurineStatus lvStatus = lvFigurine.GetComponent<FigurineStatus> ();

			int lvCellId = GridDrawer.instance.gridWidth * lvStatus.gridZ + lvStatus.gridX;

			if (lvStatus.enemy) {
				mMonsterFields.Add (lvCellId);
			} else {
				mPlayersFields.Add (lvCellId);
			}

		}

	}

	public bool IsEnemyField (int pmFieldId)
	{
		bool lvResult = false;

		if (playersTurn) {

			if (mMonsterFields.Contains (pmFieldId))
				lvResult = true;

		} else {
			if (mPlayersFields.Contains (pmFieldId))
				lvResult = true;
		}

		return lvResult;
	}

	public bool IsAllyField (int pmFieldId)
	{
		bool lvResult = false;

		if (playersTurn) {

			if (mPlayersFields.Contains (pmFieldId))
				lvResult = true;

		} else {
			if (mMonsterFields.Contains (pmFieldId))
				lvResult = true;
		}

		return lvResult;
	}

	private List<int> findOpportunityFields (List<int> pmFields)
	{
		List<int> lvReturnList = new List<int> ();

		foreach (int lvField in pmFields) {
			lvReturnList.AddRange (GetAdjacentFields (lvField));
		}

		return lvReturnList;
	}

	//Gets adjacent fields of one cell
	public List<int> GetAdjacentFields (int pmCellId)
	{
		List<int> lvReturnList = new List<int> ();

		AddAdjacentFieldToListIfGood (findTopFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood (findRightFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood (findLeftFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood (findBottomFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood (findTopRightFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood (findTopLeftFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood (findBottomLeftFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood (findBottomRightFieldId (pmCellId), lvReturnList);

		return lvReturnList;
	}


	public List<int> GetAdjacentNonBlockedFields (int pmCellId)
	{
		List<int> lvFields = GetAdjacentFields (pmCellId);
		List<int> lvResult = new List<int> ();

		foreach (int lvFieldId in lvFields) {

			if (!GridDrawer.instance.IsCellBlockedByObstacle (lvFieldId))
				lvResult.Add (lvFieldId);
		}

		return lvResult;
	}

	public List<int> GetAdjacentNonBlockedFieldsWithDiagonalCheck (int pmCellId)
	{
		List<int> lvFields = GetAdjacentFields (pmCellId);
		List<int> lvResult = new List<int> ();

		bool topMovable = false;
		bool bottomMovable = false;
		bool leftMovable = false;
		bool rightMovable = false;

		for (int i = 0; i < lvFields.Count; i++) {

			if (!GridDrawer.instance.IsCellBlockedByObstacle (lvFields [i])) {

				if (i < 4) {
					lvResult.Add (lvFields [i]);

					switch (i) {
					case 0:
						topMovable = true;
						break;
					case 1:
						rightMovable = true;
						break;
					case 2:
						leftMovable = true;
						break;
					case 3:
						bottomMovable = true;
						break;
					}
				} else {
					switch (i) {
					case 4:
						if (topMovable && rightMovable)
							lvResult.Add (lvFields [i]);
						break;
					case 5:
						if (topMovable && leftMovable)
							lvResult.Add (lvFields [i]);
						break;
					case 6:
						if (bottomMovable && leftMovable)
							lvResult.Add (lvFields [i]);
						break;
					case 7:
						if (bottomMovable && rightMovable)
							lvResult.Add (lvFields [i]);
						break;
					}
				}


			}
		}

		return lvResult;
	}


	private void AddAdjacentFieldToListIfGood (int pmCell, List<int> pmAdjacentList)
	{
		if (IsCellGood (pmCell))
			pmAdjacentList.Add (pmCell);
	}

	public List<int> GetAdjacentFields (List<int> pmCells)
	{
		List<int> lvResultList = new List<int> ();
		List<int> lvTempList;

		foreach (int lvCell in pmCells) {
			lvTempList = GetAdjacentFields (lvCell);

			foreach (int lvAdjacent in lvTempList) {
				if (!lvResultList.Contains (lvAdjacent))
					lvResultList.Add (lvAdjacent);
			}
		}

		return lvResultList;
	}

	/*
	 * Sets selected state to each List from pmCellsList
	 */
	public void SetStateToCells (List<int> pmCellsList, CellStates pmStatus)
	{
		foreach (int lvField in pmCellsList) {
			CellStatus lvStatus = GridDrawer.instance.mCells [lvField].GetComponent<CellStatus> ();
			lvStatus.SetState (pmStatus);

			switch (pmStatus) {
			case CellStates.TARGET:
				mTargetMode = true;
				break;
			case CellStates.CLOSE_RANGE:
				mTargetMode = true;
				break;
			case CellStates.FAR_RANGE:
				mTargetMode = true;
				break;	
			default:
				break;
			}

		}
	}

	public void DisplaySimpleRangeMoore (int pmInitialCell, int pmNormalRange, int pmLongRange)
	{
		List<int> lvLastLevel = new List<int> ();
		List<int> lvNormalRangeCells = new List<int> ();
		List<int> lvLongRangeCells = new List<int> ();
		List<int> lvEnemyCells = new List<int> ();

		lvLastLevel.Add (pmInitialCell);


		for (int i = 0; i < pmNormalRange; i++) {
			List<int> lvAdjacentCells = GetAdjacentFields (lvLastLevel);
			lvNormalRangeCells.AddRange (lvLastLevel);
			lvLastLevel = lvAdjacentCells;
		}

		for (int j = pmNormalRange; j < pmLongRange; j++) {
			List<int> lvAdjacentCells = GetAdjacentFields (lvLastLevel);
			if (pmNormalRange != j)
				lvLongRangeCells.AddRange (lvLastLevel);
			else
				lvNormalRangeCells.AddRange (lvLastLevel);

			lvLastLevel = new List<int> ();

			foreach (int lvLongRangeCell in lvAdjacentCells) {
				if (!lvNormalRangeCells.Contains (lvLongRangeCell))
					lvLastLevel.Add (lvLongRangeCell);
			}
		}

		lvLongRangeCells.AddRange (lvLastLevel);

		lvNormalRangeCells = RemoveCellsWithBlockedLoS (pmInitialCell, lvNormalRangeCells);
		lvLongRangeCells = RemoveCellsWithBlockedLoS (pmInitialCell, lvLongRangeCells);

		foreach (int lvEnemyCell in lvNormalRangeCells) {
			if (IsEnemyField (lvEnemyCell))
				lvEnemyCells.Add (lvEnemyCell);
		}

		foreach (int lvEnemyCell in lvLongRangeCells) {
			if (IsEnemyField (lvEnemyCell))
				lvEnemyCells.Add (lvEnemyCell);
		}

		SetStateToCells (lvNormalRangeCells, CellStates.CLOSE_RANGE);
		SetStateToCells (lvLongRangeCells, CellStates.FAR_RANGE);
		SetStateToCells (lvEnemyCells, CellStates.TARGET);

	}

	public List<int> GetSimpleRangeMoore (int pmInitialCell, int pmNormalRange, int pmLongRange)
	{
		List<int> lvLastLevel = new List<int> ();
		List<int> lvNormalRangeCells = new List<int> ();
		List<int> lvLongRangeCells = new List<int> ();
		List<int> lvEnemyCells = new List<int> ();

		lvLastLevel.Add (pmInitialCell);


		for (int i = 0; i < pmNormalRange; i++) {
			List<int> lvAdjacentCells = GetAdjacentFields (lvLastLevel);
			lvNormalRangeCells.AddRange (lvLastLevel);
			lvLastLevel = lvAdjacentCells;
		}

		for (int j = pmNormalRange; j < pmLongRange; j++) {
			List<int> lvAdjacentCells = GetAdjacentFields (lvLastLevel);
			if (pmNormalRange != j)
				lvLongRangeCells.AddRange (lvLastLevel);
			else
				lvNormalRangeCells.AddRange (lvLastLevel);

			lvLastLevel = new List<int> ();

			foreach (int lvLongRangeCell in lvAdjacentCells) {
				if (!lvNormalRangeCells.Contains (lvLongRangeCell))
					lvLastLevel.Add (lvLongRangeCell);
			}
		}

		lvLongRangeCells.AddRange (lvLastLevel);

		lvNormalRangeCells = RemoveCellsWithBlockedLoS (pmInitialCell, lvNormalRangeCells);
		lvLongRangeCells = RemoveCellsWithBlockedLoS (pmInitialCell, lvLongRangeCells);

		foreach (int lvEnemyCell in lvNormalRangeCells) {
			if (IsEnemyField (lvEnemyCell))
				lvEnemyCells.Add (lvEnemyCell);
		}

		foreach (int lvEnemyCell in lvLongRangeCells) {
			if (IsEnemyField (lvEnemyCell))
				lvEnemyCells.Add (lvEnemyCell);
		}

		List<int> allFields = new List<int> ();
		allFields.AddRange (lvNormalRangeCells);
		allFields.AddRange (lvLongRangeCells);

		return allFields;
	}

	private List<int> RemoveCellsWithBlockedLoS (int pmInitialCell, List<int> pmRangeCells)
	{
		Vector3 lvStartPos = GridDrawer.instance.getCellPositionRaycast (pmInitialCell, 1.0f);
		List<int> lvResult = new List<int> ();

		foreach (int lvCellId in pmRangeCells) {
			Vector3 lvNewVec = GridDrawer.instance.getCellPositionRaycast (lvCellId, 1.0f);
			Vector3 lvDirection = lvNewVec - lvStartPos;

			float lvLength = lvDirection.magnitude;

			RaycastHit[] lvHits = Physics.RaycastAll (lvStartPos, lvDirection, lvLength);

			if (lvHits.Length > 0) {

				bool lvIsAdd = true;

				foreach (RaycastHit lvHit in lvHits) {
					if (lvHit.collider.gameObject.tag.Equals ("Obstacle"))
						lvIsAdd = false;
				}

				if (lvIsAdd)
					lvResult.Add (lvCellId);

			} else {
				lvResult.Add (lvCellId);
			}
		}

		return lvResult;


	}

	private int findTopFieldId (int pmField)
	{
		return pmField + GridDrawer.instance.gridWidth;
	}

	private int findBottomFieldId (int pmField)
	{
		return pmField - GridDrawer.instance.gridWidth;
	}

	private int findRightFieldId (int pmField)
	{
		if ((pmField + 1) % GridDrawer.instance.gridWidth == 0)
			return -1;

		return pmField + 1;
	}

	private int findLeftFieldId (int pmField)
	{
		if (pmField % GridDrawer.instance.gridWidth == 0)
			return -1;

		return pmField - 1;
	}

	private int findTopRightFieldId (int pmField)
	{
		if ((pmField + 1) % GridDrawer.instance.gridWidth == 0)
			return -1;

		return pmField + GridDrawer.instance.gridWidth + 1;
	}

	private int findTopLeftFieldId (int pmField)
	{
		if (pmField % GridDrawer.instance.gridWidth == 0)
			return -1;

		return pmField + GridDrawer.instance.gridWidth - 1;
	}

	private int findBottomRightFieldId (int pmField)
	{
		if ((pmField + 1) % GridDrawer.instance.gridWidth == 0)
			return -1;

		return pmField - GridDrawer.instance.gridWidth + 1;
	}

	private int findBottomLeftFieldId (int pmField)
	{
		if (pmField % GridDrawer.instance.gridWidth == 0)
			return -1;

		return pmField - GridDrawer.instance.gridWidth - 1;
	}

	public void SetActivePlayerStartField (int pmField)
	{
		mActivePlayerStartField = pmField;
	}

	public bool IsTopGood (int pmCellId)
	{
		return (GridDrawer.instance.getGridZ (pmCellId) + 1) < GridDrawer.instance.gridHeight;
	}

	public bool IsBottomGood (int pmCellId)
	{
		return (GridDrawer.instance.getGridZ (pmCellId) - 1) > -1;
	}

	public bool IsRightGood (int pmCellId)
	{
		return (GridDrawer.instance.getGridX (pmCellId) + 1) < GridDrawer.instance.gridWidth;
	}

	public bool IsLeftGood (int pmCellId)
	{
		return (GridDrawer.instance.getGridX (pmCellId) - 1) > -1;
	}

	public bool IsCellGood (int pmCellId)
	{
		return pmCellId >= 0 && pmCellId < (GridDrawer.instance.gridWidth * GridDrawer.instance.gridHeight);
	}

	public void putObstacleOnField (int pmCellId, List<int> pmAdjacentCells)
	{
		this.constructorFilledSquares.Add (pmCellId);
	}

	public int CountDifficultTerrainFieldsInPath (string[] pmPath)
	{
		int count = 0;

		foreach (string id in pmPath) {
			if (GridDrawer.IsCellDifficultTerrain (GridDrawer.instance.mCells [int.Parse (id)]))
				count++;
		}

		return count;
	}


}
