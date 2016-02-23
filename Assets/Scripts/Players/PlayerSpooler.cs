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

	// Use this for initialization
	void Start () {

		mPool = new List<Player> ();

		//mSpool = new Player[2];

		GameObject fig1 = GameObject.Find ("Figurine1");
		Player pl1 = new Player ();
		pl1.Figurine = fig1;
		Sprite spr1 = Resources.Load<Sprite>("001");
		pl1.PlayerSprite = spr1;
		pl1.playerName = "Kurrdar the Mighty";
		pl1.SetAbility (AbilityNames.DEXTERITY, 10);
		pl1.SetAbility (AbilityNames.STRENGTH, 16);
		pl1.SetSpeed (4);
		pl1.UpdateAc ();
		pl1.HpTotal = 22;
		pl1.hp = 22;
		pl1.Proficiency = 2;

		List<EquipementTypes> p1ListEq = new List<EquipementTypes> ();
		p1ListEq.Add (EquipementTypes.MAIN_HAND);
		p1ListEq.Add (EquipementTypes.OFF_HAND);

		List<EquipementTypes> p2ListEq = new List<EquipementTypes> ();
		p2ListEq.Add (EquipementTypes.ARMOR);

		List<EquipementTypes> crossbowListEq = new List<EquipementTypes> ();
		crossbowListEq.Add (EquipementTypes.MAIN_HAND);

		Item lvMagicAxe = new Weapon ("Battleaxe", WeaponType.MELEE, WeaponCategory.MARTIAL, 10, 1, p1ListEq);
		lvMagicAxe.resourceImageName = "Battleaxe";
		lvMagicAxe.inventoryFieldId = "INV4";

		Item lvArmor = new Armor ("Breastplate", p2ListEq,16,0);
		lvArmor.resourceImageName = "ArmorColor";
		lvArmor.inventoryFieldId = "INV1";

		Item lvCrossbow = new Weapon ("Crossbow", WeaponType.RANGED, WeaponCategory.SIMPLE, 10, 1, crossbowListEq,1,2);
		lvCrossbow.resourceImageName = "Crossbow";
		lvCrossbow.inventoryFieldId = "INV7";

		Dictionary<string,Item> inventory1 = new Dictionary<string,Item> ();
		inventory1.Add ("INV4",lvMagicAxe);
		inventory1.Add ("INV1", lvArmor);
		inventory1.Add ("INV7", lvCrossbow);
		pl1.Inventory = inventory1;

		Attack lvAxe = new Attack ("Unarmed", Weapon.unarmed);



		pl1.equippedWeaponAttack = lvAxe;



		GameObject fig2 = GameObject.Find ("Figurine2");
		Player pl2 = new Player ();
		pl2.Figurine = fig2;
		Sprite spr2 = Resources.Load<Sprite>("002");
		pl2.PlayerSprite = spr2;
		pl2.playerName = "Goblin 1";
		pl2.SetAbility (AbilityNames.STRENGTH, 9);
		pl2.SetAbility (AbilityNames.DEXTERITY, 14);
		pl2.SetSpeed (4);
		pl2.UpdateAc ();
		pl2.HpTotal = 9;
		pl2.hp = 9;

		//Weapon lvScimitarWep = new Weapon ("Scimitar", WeaponType.MELEE, WeaponCategory.MARTIAL, 6, 1, p1ListEq);

		Attack lvScimitar = new Attack ("Unarmed",Weapon.unarmed);
		pl2.equippedWeaponAttack = lvScimitar;

		Dictionary<string,Item> inventory2 = new Dictionary<string,Item> ();
		Item lvMagicAxe2 = new Weapon ("Battleaxe", WeaponType.MELEE, WeaponCategory.MARTIAL, 10, 1, p1ListEq);
		lvMagicAxe2.resourceImageName = "Battleaxe";
		lvMagicAxe2.inventoryFieldId = "INV24";

		inventory2.Add ("INV24",lvMagicAxe2);
		pl2.Inventory = inventory2;

		mPool.Add (pl1);
		mPool.Add (pl2);

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

		GameObject lvGridSelectorObject = GameObject.Find("GridSelector");
		SelectFromGrid lvSelector = lvGridSelectorObject.GetComponent<SelectFromGrid> ();

		FigurineStatus lvStatus = mSpooledObject.GetComponent<FigurineStatus> ();
		if (!lvStatus.enemy)
			lvSelector.playersTurn = true;
		else
			lvSelector.playersTurn = false;

		int lvId = GameObject.Find ("GridDrawer").GetComponent<GridDrawer> ().GetGridId (lvStatus.gridX, lvStatus.gridZ);

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
				//Debug.Log ("Initiative " + lvKey + " player " + lvCurrentPlayer.playerName);
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
		if (!GameObject.Find ("GridSelector").GetComponent<SelectFromGrid> ().mMoveMode) {
			int lvMovesLeft = mSpool [mSpooledId].movesLeft;
			mSpooledObject.GetComponent<FigurineStatus> ().pick (lvMovesLeft);
		} else {
			GameObject.Find ("GridSelector").GetComponent<SelectFromGrid> ().DeactivateWalking ();
		}
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

	public Player GetPlayerOnField(int pmCell)
	{
		GridDrawer lvDrawer = GameObject.Find ("GridDrawer").GetComponent<GridDrawer> ();

		int lvGridX = lvDrawer.getGridX (pmCell);
		int lvGridZ = lvDrawer.getGridZ (pmCell);

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



}
