using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SelectFromGrid : MonoBehaviour {

	private CellStatus lastStatus;

	private static string XPOSITION = "XPOSITION";
	private static string ZPOSITION = "ZPOSITION";
	private static string PARENT_PATH = "PARENT_PATH";
	private static string STRAIGHT_LINE = "STRAIGHT_LINE";

	public bool mMoveMode = false;
	private bool mTargetMode = false;
	public bool mCreatorMode = false;
	private Dictionary<string, string> mPaths;

	public GameObject creatorObstacle;

	private List<int> mPlayersFields;
	private List<int> mMonsterFields;
	private List<int> mOpportunityFields;
	private int mActivePlayerStartField;

	public bool playersTurn;

	private GridDrawer mGridDrawer;

	public bool inventoryOpen = false;

	private List<int> constructorFilledSquares;

	private static SelectFromGrid mInstance;


	// Use this for initialization
	void Start () {
		Debug.Log ("Start");

		GameObject lvCanvas = GameObject.Find ("GridDrawer");
		mGridDrawer = lvCanvas.GetComponent<GridDrawer> ();
		
		mPaths = new Dictionary<string, string> ();
		updateFields ();

		constructorFilledSquares = new List<int> ();

	}
	

	void Update(){
		if (lastStatus != null)
			lastStatus.selected = false;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits = Physics.RaycastAll (ray);

		if (!inventoryOpen && !EventSystem.current.IsPointerOverGameObject()) {

			foreach (RaycastHit hit in hits) {

				// podswietlanie klatek, przesylanie koordynatow do figurki
				if (hit.collider.tag.Equals ("GridCell") && hit.collider.gameObject.GetComponent<CellStatus> ().avaiable) {
					CellStatus lvCellStatus = hit.collider.gameObject.GetComponent<CellStatus> ();

					//TODO FIX!!!!
					if (!mMoveMode && !mTargetMode) {
						lvCellStatus.selected = true;
					} else if (mTargetMode) {
						if (lvCellStatus.target)
							lvCellStatus.selected = true;
					} else {
						if (lvCellStatus.movable)
							lvCellStatus.selected = true;
					}
					lastStatus = lvCellStatus;

					Vector3 lvPosition = hit.collider.transform.position;
					int lvCellId = mGridDrawer.GetGridId ((int)lvPosition.x, (int)lvPosition.z);

					if (mMoveMode && lvCellStatus.movable) {
						string lvId = lvCellId.ToString ();
						DrawWalkableLine (mPaths [lvId]);
					}


					GameObject lvFigurine;
					PlayerSpooler lvSpooler = null;
					FigurineStatus lvStatus = null;

					if (!mCreatorMode) {
						lvSpooler = GameObject.Find ("PlayerSpooler").GetComponent<PlayerSpooler> ();
						lvFigurine = lvSpooler.mSpooledObject;
					} else {
						lvFigurine = creatorObstacle;
						SetStateToCells (constructorFilledSquares, CellStates.ENABLED);
					}

					if(lvFigurine != null)
						lvStatus = lvFigurine.GetComponent<FigurineStatus> ();
					

					if (mMoveMode) {
						if (Input.GetMouseButtonDown (0)) {
							FigurineMover lvMover = lvFigurine.GetComponent<FigurineMover> ();
								
							string lvId = lvCellId.ToString ();
							if (mPaths.ContainsKey (lvId)) {
								lvMover.path = mPaths [lvId];
								lvMover.isMoving = true;

								mGridDrawer.ClearGridStatus ();

								mMoveMode = false;
								ClearWalkableLine ();
								mPaths = new Dictionary<string, string> ();
							}
						} else if (Input.GetMouseButtonDown (1)) {
							mMoveMode = false;
							mGridDrawer.ClearGridStatus ();

							mMoveMode = false;
							ClearWalkableLine ();
							mPaths = new Dictionary<string, string> ();
							FigurineMover lvMover = lvFigurine.GetComponent<FigurineMover> ();
							lvMover.AbortMovement ();
						}

					} else if (mTargetMode) {
						if (Input.GetMouseButtonDown (0) && lvCellStatus.target) {
							Player lvTarget = lvSpooler.GetPlayerOnField (lvCellId);
							lvSpooler.ResolveSpooledAttack (lvTarget);
							mTargetMode = false;
						}

					} else if (mCreatorMode) {
					
						if (lvStatus != null && lvStatus.picked) {
							SetStateToCells (this.constructorFilledSquares, CellStates.DISABLED);
							FigurineMover lvMover = lvFigurine.GetComponent<FigurineMover> ();
							lvMover.gridX = (int)lvPosition.x;
							lvMover.gridZ = (int)lvPosition.z;

							if (Input.GetMouseButtonDown (0)) {
								lvStatus.picked = false;
								lvStatus.gridX = (int)lvPosition.x;
								lvStatus.gridZ = (int)lvPosition.z;
								putObstacleOnField (lvCellId, new List<int> ());

								lvFigurine.transform.parent = mGridDrawer.mCells [lvCellId].transform;

								//lvFigurine.GetComponent<ShaderSwitcher> ().SwitchOutlineOff ();

								//GameObject lvAssetEditPanel = GameObject.Find ("AssetEditPanel");
								//if (lvAssetEditPanel != null) {
								//	lvAssetEditPanel.GetComponent<AssetStatsEditor> ().clear ();

								//}
							}

							if (Input.GetMouseButtonDown (1)) {
								lvFigurine.GetComponent<ObstacleRotator> ().Rotate (90.0f);
							}
						} else {
							if (Input.GetMouseButtonDown (0)) {
								if (hit.collider.gameObject.transform.childCount > 0) {
									ObstacleStatus lvObstacleStatus = hit.collider.gameObject.transform.GetChild (0).GetComponent<ObstacleStatus> ();
									AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();

									if (lvStatusEditor.obstacleStatus != null && !"".Equals (lvStatusEditor.obstacleStatus.name)) {
										GameObject lvOldObstacle = GameObject.Find (lvStatusEditor.obstacleStatus.name);
										lvOldObstacle.GetComponent<ShaderSwitcher> ().SwitchOutlineOff ();
									}

									lvStatusEditor.obstacleStatus = lvObstacleStatus;
									lvStatusEditor.populate ();

									hit.collider.gameObject.transform.GetChild (0).GetComponent<ShaderSwitcher> ().SwitchOutlineOn ();
								} else {

									AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();

									if (lvStatusEditor.obstacleStatus != null && !"".Equals (lvStatusEditor.obstacleStatus.name)) {
										GameObject lvOldObstacle = GameObject.Find (lvStatusEditor.obstacleStatus.name);
										lvOldObstacle.GetComponent<ShaderSwitcher> ().SwitchOutlineOff ();
									}

									lvStatusEditor.clear ();
								}
							}
						}
					}
				}
			}
		}
	}

	public void DeactivateWalking()
	{
		PlayerSpooler lvSpooler = GameObject.Find ("PlayerSpooler").GetComponent<PlayerSpooler> ();

		GameObject lvFigurine = lvSpooler.mSpooledObject;
		mMoveMode = false;
		mGridDrawer.ClearGridStatus ();

		mMoveMode = false;
		ClearWalkableLine ();
		mPaths = new Dictionary<string, string> ();
		FigurineMover lvMover = lvFigurine.GetComponent<FigurineMover> ();
		lvMover.AbortMovement ();
	}

	public void DrawWalkableLine(string pmPath)
	{
		string [] lvPathSteps = pmPath.Split ('_');

		GameObject lvLineRendererObject = GameObject.Find ("MovementLineDrawer");
		LineRenderer lvRenderer = lvLineRendererObject.GetComponent<LineRenderer> ();

		if (lvPathSteps.Length > 1) {
			lvRenderer.SetVertexCount (lvPathSteps.Length);

			for (int i = 0; i < lvPathSteps.Length; i++) {
				int lvIndex = int.Parse (lvPathSteps [i]);

				float lvGridX = lvIndex % mGridDrawer.gridWidth;
				float lvGridZ = (lvIndex - lvGridX) / mGridDrawer.gridWidth;

				lvRenderer.SetPosition (i, new Vector3 (lvGridX + 0.5f, 0.2f, lvGridZ + 0.5f));
			}

		} else {
			lvRenderer.SetVertexCount(0);
		}

	}

	public void ClearWalkableLine()
	{
		GameObject lvLineRendererObject = GameObject.Find ("MovementLineDrawer");
		LineRenderer lvRenderer = lvLineRendererObject.GetComponent<LineRenderer>();

		lvRenderer.SetVertexCount(0);

	}

	// Funkcja wyswietla na gridzie zasieg ruchu postaci 
	public void ShowWalkableDistance(int gridX, int gridZ, int moveDist)
	{
		mMoveMode = true;

		GameObject lvCanvas = GameObject.Find ("GridDrawer");
		GridDrawer lvDrawer = lvCanvas.GetComponent<GridDrawer> ();

		if(playersTurn)
			mOpportunityFields = findOpportunityFields(mMonsterFields);
		else
			mOpportunityFields = findOpportunityFields(mPlayersFields);

		string lvStartCellId = (lvDrawer.gridWidth * gridZ + gridX).ToString ();

		mPaths.Add (lvStartCellId, lvStartCellId);

		Dictionary<string,Dictionary<string,string>> lvNextLevel = resolveForCell (mPaths, gridX, gridZ, "", moveDist);

		while (moveDist >0)
		{

			lvNextLevel = returnLevel (lvNextLevel,mPaths,moveDist);
			moveDist --;
		}

	}



	private Dictionary<string,Dictionary<string,string>> returnLevel(Dictionary<string,Dictionary<string,string>> pmNextLevel, Dictionary<string,string> pmPaths, int pmMoveDist)
	{

		Dictionary<string,Dictionary<string,string>> lvNextLevel = new Dictionary<string, Dictionary<string, string>> ();
		Dictionary<string,Dictionary<string,string>> lvTempLevel = new Dictionary<string, Dictionary<string, string>> ();

		foreach (string lvKey in pmNextLevel.Keys) {
			Dictionary<string,string> lvValue = pmNextLevel[lvKey];

			lvTempLevel = resolveForCell(pmPaths,int.Parse(lvValue[XPOSITION]),int.Parse(lvValue[ZPOSITION]),lvValue[PARENT_PATH],pmMoveDist);

			foreach(string lvKeyTemp in lvTempLevel.Keys)
			{
				if(!lvNextLevel.ContainsKey(lvKeyTemp))
					lvNextLevel.Add(lvKeyTemp,lvTempLevel[lvKeyTemp]);
			}

		}

		return lvNextLevel;
	}

	/*
	 *  
	 */
	private Dictionary<string,Dictionary<string,string>> resolveForCell(Dictionary<string,string> pmPaths, int pmGridX, int pmGridZ, string pmPath, int pmMoveDist)
	{
		GameObject lvCell = mGridDrawer.mCells[mGridDrawer.gridWidth * pmGridZ + pmGridX];

		string lvParentId = "" + (mGridDrawer.gridWidth * pmGridZ + pmGridX);

		string lvCellPath = "";

		if (pmPath.Length > 0)
			lvCellPath += pmPath + "_" + lvParentId;
		else
			lvCellPath = lvParentId;
		
		if (!IsAllyField (int.Parse (lvParentId)) || mActivePlayerStartField == int.Parse(lvParentId)) {

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

			bool lvTopGood = (pmGridZ + 1) < mGridDrawer.gridHeight;
			bool lvBottomGood = (pmGridZ - 1) > -1;
			
			bool lvRightGood = (pmGridX + 1) < mGridDrawer.gridWidth;
			bool lvLeftGood = (pmGridX - 1) > -1;

			bool lvTopMovable = false;
			bool lvBotMovable = false;
			bool lvRightMovable = false;
			bool lvLeftMovable = false;


			if (lvTopGood) {

				int cellZ = pmGridZ + 1;

				GameObject lvCellUpp = mGridDrawer.mCells [mGridDrawer.gridWidth * cellZ + pmGridX];
				CellStatus lvStatusUpp = lvCellUpp.GetComponent<CellStatus> ();
				string lvCellId = "" + (mGridDrawer.gridWidth * cellZ + pmGridX);

				if(lvStatusUpp.avaiable && !IsEnemyField(int.Parse(lvCellId)))
				{
					string cellGridZ = cellZ.ToString();
					string cellGridX = pmGridX.ToString();

					Dictionary<string,string> lvCellData = new Dictionary<string,string>();
					lvCellData.Add(XPOSITION,cellGridX);
					lvCellData.Add(ZPOSITION,cellGridZ);
					lvCellData.Add(PARENT_PATH,lvCellPath);
					lvCellData.Add(STRAIGHT_LINE,"Y");

					lvMainDictionary.Add(lvCellId,lvCellData);

					lvTopMovable = true;
				}
			}
			
			if (lvBottomGood) {
				int cellZ = pmGridZ -1;
				
				GameObject lvCellBott = mGridDrawer.mCells [mGridDrawer.gridWidth * cellZ + pmGridX];
				CellStatus lvStatusBott = lvCellBott.GetComponent<CellStatus> ();
				string lvCellId = "" + (mGridDrawer.gridWidth * cellZ + pmGridX);
				
				if(lvStatusBott.avaiable && !IsEnemyField(int.Parse(lvCellId)))
				{
					string cellGridZ = cellZ.ToString();
					string cellGridX = pmGridX.ToString();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string>();
					lvCellData.Add(XPOSITION,cellGridX);
					lvCellData.Add(ZPOSITION,cellGridZ);
					lvCellData.Add(PARENT_PATH,lvCellPath);
					lvCellData.Add(STRAIGHT_LINE,"Y");
					
					lvMainDictionary.Add(lvCellId,lvCellData);
					
					lvBotMovable = true;
				}
			}
			
			if (lvRightGood) {
				int cellX = pmGridX +1;
				
				GameObject lvCellRight = mGridDrawer.mCells [mGridDrawer.gridWidth * pmGridZ + cellX];
				CellStatus lvStatusRight = lvCellRight.GetComponent<CellStatus> ();
				string lvCellId = "" + (mGridDrawer.gridWidth * pmGridZ + cellX);
				
				if(lvStatusRight.avaiable && !IsEnemyField(int.Parse(lvCellId)))
				{
					string cellGridX = cellX.ToString();
					string cellGridZ = pmGridZ.ToString();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string>();
					lvCellData.Add(XPOSITION,cellGridX);
					lvCellData.Add(ZPOSITION,cellGridZ);
					lvCellData.Add(PARENT_PATH,lvCellPath);
					lvCellData.Add(STRAIGHT_LINE,"Y");
					
					lvMainDictionary.Add(lvCellId,lvCellData);
					
					lvRightMovable = true;
				}
			}
			
			if (lvLeftGood) {
				int cellX = pmGridX -1;
				
				GameObject lvCellLeft = mGridDrawer.mCells [mGridDrawer.gridWidth * pmGridZ + cellX];
				CellStatus lvStatusLeft = lvCellLeft.GetComponent<CellStatus> ();
				string lvCellId = "" + (mGridDrawer.gridWidth * pmGridZ + cellX);
				
				if(lvStatusLeft.avaiable && !IsEnemyField(int.Parse(lvCellId)))
				{
					string cellGridX = cellX.ToString();
					string cellGridZ = pmGridZ.ToString();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string>();
					lvCellData.Add(XPOSITION,cellGridX);
					lvCellData.Add(ZPOSITION,cellGridZ);
					lvCellData.Add(PARENT_PATH,lvCellPath);
					lvCellData.Add(STRAIGHT_LINE,"Y");
					
					lvMainDictionary.Add(lvCellId,lvCellData);
					
					lvLeftMovable = true;
				}
			}
			
			if (lvTopMovable && lvRightMovable) {
				int cellX = pmGridX + 1;
				int cellZ = pmGridZ + 1;
				
				GameObject lvCellTopRight = mGridDrawer.mCells [mGridDrawer.gridWidth * cellZ + cellX];
				CellStatus lvStatusTopRight = lvCellTopRight.GetComponent<CellStatus> ();
				string lvCellId = "" + (mGridDrawer.gridWidth * cellZ + cellX);
				
				if(lvStatusTopRight.avaiable && !IsEnemyField(int.Parse(lvCellId)))
				{
					string cellGridX = cellX.ToString();
					string cellGridZ = cellZ.ToString();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string>();
					lvCellData.Add(XPOSITION,cellGridX);
					lvCellData.Add(ZPOSITION,cellGridZ);
					lvCellData.Add(PARENT_PATH,lvCellPath);
					lvCellData.Add(STRAIGHT_LINE,"N");
					
					lvMainDictionary.Add(lvCellId,lvCellData);
				}
			}
			
			if (lvTopMovable && lvLeftMovable) {
				int cellX = pmGridX - 1;
				int cellZ = pmGridZ + 1;
				
				GameObject lvCellTopLeft = mGridDrawer.mCells [mGridDrawer.gridWidth * cellZ + cellX];
				CellStatus lvStatusTopLeft = lvCellTopLeft.GetComponent<CellStatus> ();
				string lvCellId = "" + (mGridDrawer.gridWidth * cellZ + cellX);
				
				if(lvStatusTopLeft.avaiable && !IsEnemyField(int.Parse(lvCellId)))
				{
					string cellGridX = cellX.ToString();
					string cellGridZ = cellZ.ToString();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string>();
					lvCellData.Add(XPOSITION,cellGridX);
					lvCellData.Add(ZPOSITION,cellGridZ);
					lvCellData.Add(PARENT_PATH,lvCellPath);
					lvCellData.Add(STRAIGHT_LINE,"N");
					
					lvMainDictionary.Add(lvCellId,lvCellData);
				}
			}
			
			if (lvBotMovable && lvRightMovable) {
				int cellX = pmGridX + 1;
				int cellZ = pmGridZ - 1;
				
				GameObject lvCellBotRight = mGridDrawer.mCells [mGridDrawer.gridWidth * cellZ + cellX];
				CellStatus lvStatusBotRight = lvCellBotRight.GetComponent<CellStatus> ();
				string lvCellId = "" + (mGridDrawer.gridWidth * cellZ + cellX);
				
				if(lvStatusBotRight.avaiable && !IsEnemyField(int.Parse(lvCellId)))
				{
					string cellGridX = cellX.ToString();
					string cellGridZ = cellZ.ToString();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string>();
					lvCellData.Add(XPOSITION,cellGridX);
					lvCellData.Add(ZPOSITION,cellGridZ);
					lvCellData.Add(PARENT_PATH,lvCellPath);
					lvCellData.Add(STRAIGHT_LINE,"N");
					
					lvMainDictionary.Add(lvCellId,lvCellData);
				}
			}
			
			if (lvBotMovable && lvLeftMovable) {
				int cellX = pmGridX - 1;
				int cellZ = pmGridZ - 1;
				
				GameObject lvCellBotLeft = mGridDrawer.mCells [mGridDrawer.gridWidth * cellZ + cellX];
				CellStatus lvStatusBotLeft = lvCellBotLeft.GetComponent<CellStatus> ();
				string lvCellId = "" + (mGridDrawer.gridWidth * cellZ + cellX);
				
				if(lvStatusBotLeft.avaiable && !IsEnemyField(int.Parse(lvCellId)))
				{
					string cellGridX = cellX.ToString();
					string cellGridZ = cellZ.ToString();
					
					Dictionary<string,string> lvCellData = new Dictionary<string,string>();
					lvCellData.Add(XPOSITION,cellGridX);
					lvCellData.Add(ZPOSITION,cellGridZ);
					lvCellData.Add(PARENT_PATH,lvCellPath);
					lvCellData.Add(STRAIGHT_LINE,"N");
					
					lvMainDictionary.Add(lvCellId,lvCellData);
				}
			}
		}

		return lvMainDictionary;

	}

	public void updateFields()
	{
		GameObject [] lvFigurines = GameObject.FindGameObjectsWithTag ("Figurine");

		mPlayersFields = new List<int> ();
		mMonsterFields = new List<int> ();

		foreach (GameObject lvFigurine in lvFigurines) {

			FigurineStatus lvStatus = lvFigurine.GetComponent<FigurineStatus>();

			int lvCellId = mGridDrawer.gridWidth * lvStatus.gridZ + lvStatus.gridX;

			if(lvStatus.enemy)
			{
				mMonsterFields.Add(lvCellId);
			} else {
				mPlayersFields.Add(lvCellId);
			}

		}

	}

	public bool IsEnemyField(int pmFieldId)
	{
		bool lvResult = false;

		if (playersTurn) {

			if(mMonsterFields.Contains(pmFieldId))
				lvResult = true;

		} else {
			if(mPlayersFields.Contains(pmFieldId))
				lvResult = true;
		}

		return lvResult;
	}

	public bool IsAllyField(int pmFieldId)
	{
		bool lvResult = false;

		if (playersTurn) {

			if(mPlayersFields.Contains(pmFieldId))
				lvResult = true;

		} else {
			if(mMonsterFields.Contains(pmFieldId))
				lvResult = true;
		}

		return lvResult;
	}

	private List<int> findOpportunityFields(List<int> pmFields)
	{
		List<int> lvReturnList = new List<int>();

		foreach (int lvField in pmFields) {
			lvReturnList.AddRange (GetAdjacentFields (lvField));
		}

		return lvReturnList;
	}

	//Gets adjacent fields of one cell
	public List<int> GetAdjacentFields(int pmCellId)
	{
		List<int> lvReturnList = new List<int>();

		AddAdjacentFieldToListIfGood(findTopFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood(findTopRightFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood(findTopLeftFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood(findRightFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood(findLeftFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood(findBottomFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood(findBottomLeftFieldId (pmCellId), lvReturnList);
		AddAdjacentFieldToListIfGood(findBottomRightFieldId (pmCellId), lvReturnList);

		return lvReturnList;
	}

	private void AddAdjacentFieldToListIfGood(int pmCell, List<int> pmAdjacentList)
	{
		if (IsCellGood (pmCell))
			pmAdjacentList.Add (pmCell);
	}

	public List<int> GetAdjacentFields(List<int> pmCells)
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
	public void SetStateToCells(List<int> pmCellsList, CellStates pmStatus)
	{
		foreach (int lvField in pmCellsList) {
			CellStatus lvStatus = mGridDrawer.mCells [lvField].GetComponent<CellStatus> ();

			switch (pmStatus) {
			case CellStates.DISABLED:
				lvStatus.avaiable = false;
				break;
			case CellStates.ENABLED:
				lvStatus.avaiable = true;
				break;
			case CellStates.TARGET:
				mTargetMode = true;
				lvStatus.target = true;
				break;
			case CellStates.OPPORTUNITY:
				lvStatus.lvOportunity = true;
				break;
			case CellStates.MOVABLE:
				lvStatus.movable = true;
				break;
			case CellStates.CLOSE_RANGE:
				mTargetMode = true;
				lvStatus.closeRange = true;
				break;
			case CellStates.FAR_RANGE:
				mTargetMode = true;
				lvStatus.farRange = true;
				break;	
			default:
				break;
			}

		}
	}

	public void DisplaySimpleRangeMoore(int pmInitialCell, int pmNormalRange, int pmLongRange)
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

			lvLastLevel = new List<int>();

			foreach (int lvLongRangeCell in lvAdjacentCells) {
				if (!lvNormalRangeCells.Contains (lvLongRangeCell))
					lvLastLevel.Add (lvLongRangeCell);
			}
		}

		lvLongRangeCells.AddRange (lvLastLevel);

		lvNormalRangeCells = RemoveCellsWithBlockedLoS (pmInitialCell, lvNormalRangeCells);
		lvLongRangeCells = RemoveCellsWithBlockedLoS (pmInitialCell, lvLongRangeCells);

		foreach(int lvEnemyCell in lvNormalRangeCells)
		{
			if (IsEnemyField (lvEnemyCell))
				lvEnemyCells.Add (lvEnemyCell);
		}

		foreach(int lvEnemyCell in lvLongRangeCells)
		{
			if (IsEnemyField (lvEnemyCell))
				lvEnemyCells.Add (lvEnemyCell);
		}

		SetStateToCells (lvNormalRangeCells, CellStates.CLOSE_RANGE);
		SetStateToCells (lvLongRangeCells, CellStates.FAR_RANGE);
		SetStateToCells (lvEnemyCells, CellStates.TARGET);

	}

	private List<int> RemoveCellsWithBlockedLoS(int pmInitialCell, List<int> pmRangeCells)
	{
		Vector3 lvStartPos = mGridDrawer.getCellPosition (pmInitialCell,1.0f);
		List<int> lvResult = new List<int> ();

		foreach (int lvCellId in pmRangeCells) {
			Vector3 lvNewVec = mGridDrawer.getCellPosition (lvCellId, 1.0f);
			Vector3 lvDirection = lvNewVec - lvStartPos;

			float lvLength = lvDirection.magnitude;

			RaycastHit[] lvHits = Physics.RaycastAll (lvStartPos, lvDirection, lvLength);

			if (lvHits.Length > 0) {

				bool lvIsAdd = true;

				foreach (RaycastHit lvHit in lvHits) {
					if (lvHit.collider.gameObject.tag.Equals ("Obstacle"))
						lvIsAdd = false;
				}

				if(lvIsAdd)
					lvResult.Add (lvCellId);

			} else {
				lvResult.Add (lvCellId);
			}
		}

		return lvResult;


	}

	private int findTopFieldId(int pmField)
	{
		return pmField + mGridDrawer.gridWidth;
	}

	private int findBottomFieldId(int pmField)
	{
		return pmField - mGridDrawer.gridWidth;
	}

	private int findRightFieldId(int pmField)
	{
		if((pmField+1) % mGridDrawer.gridWidth == 0)
			return -1;

		return pmField + 1;
	}
	
	private int findLeftFieldId(int pmField)
	{
		if (pmField % mGridDrawer.gridWidth == 0)
			return -1;

		return pmField -1;
	}

	private int findTopRightFieldId(int pmField)
	{
		if((pmField+1) % mGridDrawer.gridWidth == 0)
			return -1;

		return pmField + mGridDrawer.gridWidth + 1;
	}
	
	private int findTopLeftFieldId(int pmField)
	{
		if (pmField % mGridDrawer.gridWidth == 0)
			return -1;

		return pmField + mGridDrawer.gridWidth - 1;
	}
	
	private int findBottomRightFieldId(int pmField)
	{
		if((pmField+1) % mGridDrawer.gridWidth == 0)
			return -1;

		return pmField - mGridDrawer.gridWidth + 1;
	}
	
	private int findBottomLeftFieldId(int pmField)
	{
		if (pmField % mGridDrawer.gridWidth == 0)
			return -1;

		return pmField - mGridDrawer.gridWidth -1;
	}

	public void SetActivePlayerStartField(int pmField)
	{
		mActivePlayerStartField = pmField;
	}

	public bool IsTopGood(int pmCellId)
	{
		return (mGridDrawer.getGridZ(pmCellId) + 1) < mGridDrawer.gridHeight;
	}

	public bool IsBottomGood(int pmCellId)
	{
		return (mGridDrawer.getGridZ(pmCellId) - 1) > -1;
	}

	public bool IsRightGood(int pmCellId)
	{
		return (mGridDrawer.getGridX(pmCellId) + 1) < mGridDrawer.gridWidth;
	}

	public bool IsLeftGood(int pmCellId)
	{
		return (mGridDrawer.getGridX(pmCellId) - 1) > -1;
	}

	public bool IsCellGood(int pmCellId)
	{
		return pmCellId >= 0 && pmCellId < (mGridDrawer.gridWidth * mGridDrawer.gridHeight);
	}

	public void putObstacleOnField(int pmCellId, List<int> pmAdjacentCells){
		this.constructorFilledSquares.Add (pmCellId);
	}

}
