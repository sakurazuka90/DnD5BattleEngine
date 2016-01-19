using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerSpooler : MonoBehaviour {

	Player [] mSpool;
	private List<Player> mPool;
	public GameObject mSpooledObject;
	private int mSpooledId;

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
		pl1.setAbility (AbilityNames.DEXTERITY, 10);
		pl1.setSpeed (4);

		GameObject fig2 = GameObject.Find ("Figurine2");
		Player pl2 = new Player ();
		pl2.Figurine = fig2;
		Sprite spr2 = Resources.Load<Sprite>("002");
		pl2.PlayerSprite = spr2;
		pl2.playerName = "Goblin 1";
		pl2.setAbility (AbilityNames.DEXTERITY, 14);
		pl2.setSpeed (4);

		mPool.Add (pl1);
		mPool.Add (pl2);

		prepareSpool ();

		mSpooledId = 0;
		updateFigurine ();

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
	
	// Update is called once per frame
	void Update () {
	
	}

	private void prepareSpool()
	{
		mSpool = new Player[mPool.Count];

		Dictionary<int,List<Player>> lvInitDict = new Dictionary<int, List<Player>> ();
		foreach (Player lvPlayer in mPool) {
			int lvInit = lvPlayer.rollTest(TestsNames.INITIATIVE);
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
				Debug.Log ("Initiative " + lvKey + " player " + lvCurrentPlayer.playerName);
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

	}

	private void updateFigurine()
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
		int lvMovesLeft = mSpool [mSpooledId].movesLeft;
		mSpooledObject.GetComponent<FigurineStatus> ().pick (lvMovesLeft);
	}

	public void spool()
	{
		updateFigurine ();
		setSpooler ();
		UpdateImage ();
		UpdateName ();
		UpdateMove ();

		GameObject lvGridSelectorObject = GameObject.Find("GridSelector");
		SelectFromGrid lvSelector = lvGridSelectorObject.GetComponent<SelectFromGrid> ();
		lvSelector.updateFields ();

		FigurineStatus lvStatus = mSpooledObject.GetComponent<FigurineStatus> ();
		if (!lvStatus.enemy)
			lvSelector.playersTurn = true;
		else
			lvSelector.playersTurn = false;

		int lvId = GameObject.Find ("GridDrawer").GetComponent<GridDrawer> ().GetGridId (lvStatus.gridX, lvStatus.gridZ);

		lvSelector.SetActivePlayerStartField (lvId);
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

	public void DecreaseMoves(int pmValue)
	{
		mSpool [mSpooledId].DecreaseMovesLeft (pmValue);
		UpdateMove ();
	}

	private void UpdateMove()
	{
		UpdateTextField (mSpool [mSpooledId].movesLeft.ToString(),"MovesLeftText");
	}

	private void UpdateName()
	{
		UpdateTextField (mSpool [mSpooledId].playerName,"NameValue");
	}

	public void UpdateHP()
	{
		UpdateTextField (mSpool [mSpooledId].hp.ToString(),"HPText");
	}

	public void UpdateTextField(string pmValue, string pmName)
	{
		GameObject lvObject = GameObject.Find (pmName);
		Text lvText = lvObject.GetComponent<Text> ();
		lvText.text = pmValue;
	}




}
