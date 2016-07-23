using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerSpooler : MonoBehaviour {

	static Player [] mSpool;
	private List<Player> mPool;
	public GameObject mSpooledObject;
	private static int mSpooledId;

	private GameObject [] mSpoolerPics;

	public static PlayerSpooler instance;

	public GameObject genericYesNoWindowPrefab;

	private string _convertActionString = "Do you really want to convert 1 Standard Action into 1 Move Action?";

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
	}

	public void Run()
	{
		int[] lvPlayers = BattlefieldStateReader.instance.Players;
		int[] lvEnemies = BattlefieldStateReader.instance.Enemies;

		mPool = new List<Player> ();

		for (int i = 0; i < lvPlayers.Length; i++) {

			int lvPlayerId = lvPlayers [i];

			if (lvPlayerId > 0) {
				
				Player pl1 = DatabaseController.GetPlayerByID (lvPlayerId);
				pl1.Saves = DatabaseController.GetCharacterSavesByCharacterId (lvPlayerId);
				pl1.Skills = DatabaseController.GetCharacterSkillsByCharacterId (lvPlayerId);

				GameObject lvModel = Instantiate(Resources.Load<GameObject> ("Figurines/Models/" + pl1.FigurineModelName));

				FigurineStatus lvStatus = lvModel.GetComponent<FigurineStatus> ();
				lvStatus.enemy = false;
				lvStatus.gridX = GridDrawer.instance.getGridX (i);
				lvStatus.gridZ = GridDrawer.instance.getGridZ (i);

				FigurineMover lvMover = lvModel.GetComponent<FigurineMover> ();
				lvMover.gridX = GridDrawer.instance.getGridX (i);
				lvMover.gridZ = GridDrawer.instance.getGridZ (i);

				FigurinePositionController.instance.AddFigurineMover (lvMover);

				pl1.Figurine = lvModel;
				Dictionary<string,Item> inventory1 = new Dictionary<string,Item> ();

				DatabaseController.AddPlayersWeaponsToInventory (lvPlayerId, inventory1, pl1);
				DatabaseController.AddPlayersArmorsToInventory (lvPlayerId, inventory1, pl1);

				pl1.Inventory = inventory1;

				if (pl1.databaseEqWeaponId == 0) {
					Attack lvAxe = new Attack ("Unarmed", Weapon.unarmed);
					pl1.equippedWeaponAttack = lvAxe;
				}

				pl1.UpdateAc ();

				mPool.Add (pl1);



			}
		}

		for (int j = 0; j < lvEnemies.Length; j++) {

			int lvPlayerId = lvEnemies [j];

			if (lvPlayerId > 0) {
				Player pl1 = DatabaseController.GetPlayerByID (lvPlayerId);
				pl1.Saves = DatabaseController.GetCharacterSavesByCharacterId (lvPlayerId);
				pl1.Skills = DatabaseController.GetCharacterSkillsByCharacterId (lvPlayerId);

				GameObject lvModel = Instantiate(Resources.Load<GameObject> ("Figurines/Models/" + pl1.FigurineModelName));

				FigurineStatus lvStatus = lvModel.GetComponent<FigurineStatus> ();
				lvStatus.enemy = true;
				lvStatus.gridX = GridDrawer.instance.getGridX (j);
				lvStatus.gridZ = GridDrawer.instance.getGridZ (j);

				FigurineMover lvMover = lvModel.GetComponent<FigurineMover> ();
				lvMover.gridX = GridDrawer.instance.getGridX (j);
				lvMover.gridZ = GridDrawer.instance.getGridZ (j);

				pl1.Figurine = lvModel;

				Dictionary<string,Item> inventory1 = new Dictionary<string,Item> ();

				DatabaseController.AddPlayersWeaponsToInventory (lvPlayerId, inventory1, pl1);
				DatabaseController.AddPlayersArmorsToInventory (lvPlayerId, inventory1, pl1);

				pl1.Inventory = inventory1;

				if (pl1.databaseEqWeaponId == 0) {
					Attack lvAxe = new Attack ("Unarmed", Weapon.unarmed);
					pl1.equippedWeaponAttack = lvAxe;
				}

				pl1.UpdateAc ();
				//UpdateWeapon ();

				mPool.Add (pl1);

			}
		}

		FigurinePositionController.instance.isInitialized = true;

		prepareSpool ();

		mSpooledId = 0;
		UpdateFigurine ();

		mSpoolerPics = new GameObject[5];

		mSpoolerPics [0] = GameObject.Find ("Spool1");
		mSpoolerPics [1] = GameObject.Find ("Spool2");
		mSpoolerPics [2] = GameObject.Find ("Spool3");
		mSpoolerPics [3] = GameObject.Find ("Spool4");
		mSpoolerPics [4] = GameObject.Find ("Spool5");

		setSpooler ();
		UpdateImage ();
		UpdateName ();
		UpdateMove ();
		UpdateWeapon ();
		UpdateHP ();
		UpdateAc ();
		UpdateActions ();

		GameObject lvGridSelectorObject = GameObject.Find("GridSelector");
		SelectFromGrid lvSelector = lvGridSelectorObject.GetComponent<SelectFromGrid> ();

		FigurineStatus lvSpooledStatus = mSpooledObject.GetComponent<FigurineStatus> ();
		if (!lvSpooledStatus.enemy)
			lvSelector.playersTurn = true;
		else
			lvSelector.playersTurn = false;

		int lvId = GridDrawer.instance.GetGridId (lvSpooledStatus.gridX, lvSpooledStatus.gridZ);

		lvSelector.SetActivePlayerStartField (lvId);
	}

	private void prepareSpool()
	{
		mSpool = new Player[mPool.Count];

		Dictionary<int,List<Player>> lvInitDict = new Dictionary<int, List<Player>> ();
		foreach (Player lvPlayer in mPool) {
			int lvInit = lvPlayer.RollTest(TestsNames.INITIATIVE);
			if (lvInitDict.ContainsKey (lvInit)) {
				lvInitDict [lvInit].Add (lvPlayer);
			} else {
				List<Player> lvList = new List<Player> ();
				lvList.Add (lvPlayer);
				lvInitDict.Add (lvInit,lvList);
			}
		}

		List<int> lvKeys = lvInitDict.Keys.ToList();
		lvKeys.Sort ();
		lvKeys.Reverse ();

		int lvArrayCount = 0;

		foreach (int lvKey in lvKeys) {
			List<Player> lvCurrentInitPlayers = lvInitDict [lvKey];
			if (lvCurrentInitPlayers.Count == 1) {
				Player lvCurrentPlayer = lvCurrentInitPlayers [0];

				mSpool [lvArrayCount] = lvCurrentPlayer;

				lvArrayCount++;
			} else {
				foreach (Player lvCurrentPlayer in lvCurrentInitPlayers) {
					mSpool [lvArrayCount] = lvCurrentPlayer;
					Debug.Log ("Initiative " + lvKey + " player " + lvCurrentPlayer.playerName);
					lvArrayCount++;
				}
			}
		}

	}

	private void UpdateImage()
	{
		GameObject lvPlayerPicObject = GameObject.Find ("CharImage");
		Image lvImage = lvPlayerPicObject.GetComponent<Image>();

		lvImage.sprite = mSpool [mSpooledId].PlayerSprite;

		if (mSpool [mSpooledId].hp == 0) {
			lvImage.color = new Color (152.0F / 255.0F, 2.0F / 255.0F, 2.0F / 255.0F);
		} else {
			lvImage.color = new Color (1.0F, 1.0F, 1.0F);
		}

	}

	private void UpdateFigurine()
	{
		if (mSpooledObject == null) {
			mSpooledObject = mSpool [0].Figurine;
		} else {
			if(mSpooledId < (mSpool.Length-1))
			{
				mSpooledId ++;
			} else {
				mSpooledId = 0;
			}
			mSpooledObject = mSpool[mSpooledId].Figurine;
		}

		//at start turn reset move of set figurine
		mSpool[mSpooledId].ResetMovesLeft();
	}

	public void pick()
	{
		if (!SelectFromGrid.instance.mMoveMode) {

			int lvMovesLeft = mSpool [mSpooledId].movesLeft;

			if (lvMovesLeft == 0 && mSpool[mSpooledId].mTotalStandardActions > 0) {

				GameObject lvWindow = GameObject.Instantiate (genericYesNoWindowPrefab);
				lvWindow.GetComponent<GenericyesNoPanelControler> ().InitializePanel (_convertActionString, this.ConvertStandardActionToMove);

				lvWindow.transform.parent = GameObject.Find ("Canvas").transform;
				lvWindow.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;

			} else {			
				mSpooledObject.GetComponent<FigurineStatus> ().pick (lvMovesLeft);
			}
		} else {
			SelectFromGrid.instance.DeactivateWalking ();
		}
	}

	public void ConvertStandardActionToMove()
	{
		mSpool [mSpooledId].ConvertStandardActionToMove ();
		UpdateMove ();
		UpdateActions ();
		pick ();
	}

	public void ShowDefaultWeaponTargets()
	{
		mSpool [mSpooledId].equippedWeaponAttack.DisplayTargets (mSpool [mSpooledId]);
	}

	public void HitWithDefaultWeapon(Player pmTarget)
	{
		mSpool [mSpooledId].equippedWeaponAttack.resolveHit (mSpool [mSpooledId], pmTarget);
	}

	public void spool()
	{
		UpdateFigurine ();

		//we do survival check first cause it can restore 1 hp and so second if will also be true
		if(mSpool [mSpooledId].hp == 0 && ! mSpool [mSpooledId].isDead )
			mSpool [mSpooledId].MakeSurvivalCheck ();

		if (mSpool [mSpooledId].hp > 0) {
			setSpooler ();
			UpdateImage ();
			UpdateName ();
			UpdateMove ();
			UpdateWeapon ();
			UpdateHP ();
			UpdateAc ();
			UpdateActions ();

			mSpool [mSpooledId].RefillActionsPool ();

			GameObject lvGridSelectorObject = GameObject.Find ("GridSelector");
			SelectFromGrid lvSelector = lvGridSelectorObject.GetComponent<SelectFromGrid> ();
			lvSelector.updateFields ();

			FigurineStatus lvStatus = mSpooledObject.GetComponent<FigurineStatus> ();
			if (!lvStatus.enemy)
				lvSelector.playersTurn = true;
			else
				lvSelector.playersTurn = false;

			int lvId = GameObject.Find ("GridDrawer").GetComponent<GridDrawer> ().GetGridId (lvStatus.gridX, lvStatus.gridZ);

			lvSelector.SetActivePlayerStartField (lvId);

			AIEngine.instance.InitEngine (mSpool [mSpooledId]);

		} else {

			//if player is dead we move to next one
			spool ();
		}
	}

	private void setSpooler()
	{
		if (mSpool.Length > 0) {

			int imgCounter = 0;
			int playerCounter = 0;

			if(mSpooledId+1 < mSpool.Length)
				playerCounter = mSpooledId+1;

			while(imgCounter < mSpoolerPics.Length)
			{
				Image lvImage = mSpoolerPics[imgCounter].GetComponent<Image>();
				lvImage.sprite = mSpool[playerCounter].PlayerSprite;

				if (mSpool [playerCounter].hp == 0 && !mSpool [playerCounter].isDead) {
					lvImage.color = new Color (152.0F / 255.0F, 2.0F / 255.0F, 2.0F / 255.0F);
				} else if (mSpool [playerCounter].hp == 0 && mSpool [playerCounter].isDead) { 
					lvImage.color = new Color (53.0F / 255.0F, 48.0F / 255.0F, 48.0F / 255.0F);
				}
				else {
					lvImage.color = new Color (1.0F, 1.0F, 1.0F);
				}

				if(playerCounter < mSpool.Length -1)
				{
					playerCounter ++;
				} else {
					playerCounter = 0;
				}
				imgCounter ++;
				
			}

		}
	}

	public static void DecreaseMoves(int pmValue)
	{
		mSpool [mSpooledId].DecreaseMovesLeft (pmValue);
		UpdateMove ();
		mSpool [mSpooledId].UseActionForMovement ();
		UpdateActions ();
	}
		

	private static void UpdateMove()
	{
		UpdateTextField (mSpool [mSpooledId].movesLeft.ToString(),"MovesLeftText");
	}

	private static void UpdateName()
	{
		UpdateTextField (mSpool [mSpooledId].playerName,"NameValue");
	}

	public static void UpdateHP()
	{
		UpdateTextField (mSpool [mSpooledId].hp.ToString(),"HPText");
	}

	public static void UpdateWeapon()
	{
		UpdateTextField (mSpool [mSpooledId].equippedWeaponAttack.Name,"WeaponText");
	}

	public static void UpdateAc()
	{
		UpdateTextField (mSpool [mSpooledId].ac.ToString(),"ACText");
	}

	public static void UpdateTextField(string pmValue, string pmName)
	{
		GameObject.Find (pmName).GetComponent<Text> ().text = pmValue;
	}

	public static void UpdateActions()
	{
		UpdateTextField (mSpool [mSpooledId].mTotalMoveActions.ToString(),"MoveActionsText");
		UpdateTextField (mSpool [mSpooledId].mTotalStandardActions.ToString(),"StandardActionsText");
		UpdateTextField (mSpool [mSpooledId].mTotalBonusActions.ToString(),"BasicActionsText");
	}

	public Player GetPlayerOnField(int pmCell)
	{
		int lvGridX = GridDrawer.instance.getGridX (pmCell);
		int lvGridZ = GridDrawer.instance.getGridZ (pmCell);

		foreach(Player lvPlayer in mPool)
		{
			FigurineStatus lvStatus = lvPlayer.Figurine.GetComponent<FigurineStatus> ();

			if (lvGridX == lvStatus.gridX && lvGridZ == lvStatus.gridZ)
				return lvPlayer;
		}

		return null;
		
	}

	public void ResolveSpooledAttack(Player pmTarget)
	{
		mSpool [mSpooledId].equippedWeaponAttack.resolveHit (mSpool [mSpooledId], pmTarget);
	}

	public Player GetSpooledPlayer()
	{
		return mSpool [mSpooledId];
	}

	private void SpoolUnconciousPlayer()
	{
		
	}

	public List<Player> GetPlayersByPlayerType(PlayerTypes pmType)
	{
		bool isPlayer = false;

		if (pmType == PlayerTypes.PLAYER) {
			isPlayer = true;
		}

		List<Player> result = new List<Player> ();

		foreach (Player pooled in mPool) {
			FigurineStatus status = pooled.Figurine.GetComponent<FigurineStatus> ();

			if (status.enemy == !isPlayer)
				result.Add (pooled);
		}

		return result;
	}



}
