using UnityEngine;
using System.Collections.Generic;

public class SelectFromGrid : MonoBehaviour {

	private CellStatus lastStatus;

	private static string XPOSITION = "XPOSITION";
	private static string ZPOSITION = "ZPOSITION";
	private static string PARENT_PATH = "PARENT_PATH";
	private static string STRAIGHT_LINE = "STRAIGHT_LINE";

	private bool mMoveMode = false;
	private bool mTargetMode = false;
	private Dictionary<string, string> mPaths;

	private List<int> mPlayersFields;
	private List<int> mMonsterFields;
	private List<int> mOpportunityFields;
	private int mActivePlayerStartField;

	public bool playersTurn;

	private GridDrawer mGridDrawer;

	public bool inventoryOpen = false;


	// Use this for initialization
	void Start () {

		GameObject lvCanvas = GameObject.Find ("GridDrawer");
		mGridDrawer = lvCanvas.GetComponent<GridDrawer> ();
		
		mPaths = new Dictionary<string, string> ();
		updateFields ();

	}
	

	void Update(){
		if (lastStatus != null)
			lastStatus.selected = false;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits = Physics.RaycastAll (ray);

		if (!inventoryOpen) {

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

					MeshFilter lvFilter = hit.collider.gameObject.GetComponent<MeshFilter> ();
					Vector3[] vertices = lvFilter.mesh.vertices;

					if (mMoveMode && lvCellStatus.movable) {
						string lvId = "" + (mGridDrawer.gridWidth * (int)vertices [0].z + (int)vertices [0].x);
						DrawWalkableLine (mPaths [lvId]);
					}

					PlayerSpooler lvSpooler = GameObject.Find ("PlayerSpooler").GetComponent<PlayerSpooler> ();

					GameObject lvFigurine = lvSpooler.mSpooledObject;
					FigurineStatus lvStatus = lvFigurine.GetComponent<FigurineStatus> ();

					int lvCellId = mGridDrawer.GetGridId ((int)vertices [0].x, (int)vertices [0].z);

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
						if (Input.GetMouseButtonDown (0)) {
							Player lvTarget = lvSpooler.GetPlayerOnField (lvCellId);
							Debug.Log ("AAAA" + lvTarget.playerName);
							lvSpooler.ResolveSpooledAttack (lvTarget);
							mTargetMode = false;
						}

					} else if (lvStatus.picked) {
						FigurineMover lvMover = lvFigurine.GetComponent<FigurineMover> ();
						lvMover.gridX = (int)vertices [0].x;
						lvMover.gridZ = (int)vertices [0].z;

						if (Input.GetMouseButtonDown (0)) {
							lvStatus.picked = false;
							lvStatus.gridX = (int)vertices [0].x;
							lvStatus.gridZ = (int)vertices [0].z;
						}
					}
				}
			}
		}
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
		GameObject lvCell = mGridDrawer._cells[mGridDrawer.gridWidth * pmGridZ + pmGridX];

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

				GameObject lvCellUpp = mGridDrawer._cells [mGridDrawer.gridWidth * cellZ + pmGridX];
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
				
				GameObject lvCellBott = mGridDrawer._cells [mGridDrawer.gridWidth * cellZ + pmGridX];
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
				
				GameObject lvCellRight = mGridDrawer._cells [mGridDrawer.gridWidth * pmGridZ + cellX];
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
				
				GameObject lvCellLeft = mGridDrawer._cells [mGridDrawer.gridWidth * pmGridZ + cellX];
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
				
				GameObject lvCellTopRight = mGridDrawer._cells [mGridDrawer.gridWidth * cellZ + cellX];
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
				
				GameObject lvCellTopLeft = mGridDrawer._cells [mGridDrawer.gridWidth * cellZ + cellX];
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
				
				GameObject lvCellBotRight = mGridDrawer._cells [mGridDrawer.gridWidth * cellZ + cellX];
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
				
				GameObject lvCellBotLeft = mGridDrawer._cells [mGridDrawer.gridWidth * cellZ + cellX];
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

	public List<int> GetAdjacentFields(int pmCellId)
	{
		List<int> lvReturnList = new List<int>();

		lvReturnList.Add (findTopFieldId (pmCellId));
		lvReturnList.Add (findTopRightFieldId (pmCellId));
		lvReturnList.Add (findTopLeftFieldId (pmCellId));
		lvReturnList.Add (findRightFieldId (pmCellId));
		lvReturnList.Add (findLeftFieldId (pmCellId));
		lvReturnList.Add (findBottomFieldId (pmCellId));
		lvReturnList.Add (findBottomLeftFieldId (pmCellId));
		lvReturnList.Add (findBottomRightFieldId (pmCellId));

		return lvReturnList;

	}

	public void SetStateToCells(List<int> pmCellsList, CellStates pmStatus)
	{
		foreach (int lvField in pmCellsList) {
			CellStatus lvStatus = mGridDrawer._cells [lvField].GetComponent<CellStatus> ();

			switch (pmStatus) {
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
			default:
				break;
			}

		}
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
		return pmField + 1;
	}
	
	private int findLeftFieldId(int pmField)
	{
		return pmField -1;
	}

	private int findTopRightFieldId(int pmField)
	{
		return pmField + mGridDrawer.gridWidth + 1;
	}
	
	private int findTopLeftFieldId(int pmField)
	{
		return pmField + mGridDrawer.gridWidth - 1;
	}
	
	private int findBottomRightFieldId(int pmField)
	{
		return pmField - mGridDrawer.gridWidth + 1;
	}
	
	private int findBottomLeftFieldId(int pmField)
	{
		return pmField - mGridDrawer.gridWidth -1;
	}

	public void SetActivePlayerStartField(int pmField)
	{
		mActivePlayerStartField = pmField;
	}

}
