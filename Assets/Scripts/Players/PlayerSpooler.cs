using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerSpooler : MonoBehaviour {

	Player [] mSpool;
	public GameObject mSpooledObject;
	private int mSpooledId;

	private GameObject [] mSpoolerPics;

	// Use this for initialization
	void Start () {

		mSpool = new Player[2];

		GameObject fig1 = GameObject.Find ("Figurine1");
		Player pl1 = new Player ();
		pl1.Figurine = fig1;
		Sprite spr1 = Resources.Load<Sprite>("001");
		pl1.PlayerSprite = spr1;
		pl1.playerName = "Kurrdar the Mighty";

		GameObject fig2 = GameObject.Find ("Figurine2");
		Player pl2 = new Player ();
		pl2.Figurine = fig2;
		Sprite spr2 = Resources.Load<Sprite>("002");
		pl2.PlayerSprite = spr2;
		pl2.playerName = "Goblin 1";


		mSpool [0] = pl1;
		mSpool [1] = pl2;

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

		GameObject lvGridSelectorObject = GameObject.Find("GridSelector");
		SelectFromGrid lvSelector = lvGridSelectorObject.GetComponent<SelectFromGrid> ();

		FigurineStatus lvStatus = mSpooledObject.GetComponent<FigurineStatus> ();
		if (!lvStatus.enemy)
			lvSelector.playersTurn = true;
		else
			lvSelector.playersTurn = false;


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void UpdateImage()
	{
		GameObject lvPlayerPicObject = GameObject.Find ("CharImage");
		Image lvImage = lvPlayerPicObject.GetComponent<Image>();

		lvImage.sprite = mSpool [mSpooledId].PlayerSprite;

	}

	private void UpdateName()
	{
		GameObject lvNameObject = GameObject.Find ("NameValue");
		Text lvNameText = lvNameObject.GetComponent<Text> ();
		lvNameText.text = mSpool [mSpooledId].playerName;
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
	}

	public void pick()
	{
		mSpooledObject.GetComponent<FigurineStatus> ().pick ();
	}

	public void spool()
	{
		updateFigurine ();
		setSpooler ();
		UpdateImage ();
		UpdateName ();

		GameObject lvGridSelectorObject = GameObject.Find("GridSelector");
		SelectFromGrid lvSelector = lvGridSelectorObject.GetComponent<SelectFromGrid> ();
		lvSelector.updateFields ();

		FigurineStatus lvStatus = mSpooledObject.GetComponent<FigurineStatus> ();
		if (!lvStatus.enemy)
			lvSelector.playersTurn = true;
		else
			lvSelector.playersTurn = false;

	}

	private void setupSpooler()
	{

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





}
