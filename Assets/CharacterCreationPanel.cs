﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterCreationPanel : AbstractPanelController {

	public GameObject generalPanel;
	public GameObject racePanel;

	private Player newPlayer;
	int step = 0;

	public GameObject characterImage;
	private int currentImage = 0;

	private Sprite[] characterImages;
	private GameObject[] figurinePrefabs;

	private GameObject _figurineShowcase;

	public GameObject characterNameTextField;
	public GameObject playerNameTextField;
	public GameObject image;

	void OnEnable()
	{
		newPlayer = new Player ();
		step = 0;
		UpdateCharacterImage ();
	}

	// Use this for initialization
	void Start () {
		characterImages = Resources.LoadAll<Sprite> ("CharacterImages");
		figurinePrefabs = Resources.LoadAll<GameObject> ("Figurines/Models");
		GenerateContent ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public List<string> ValidateStep0()
	{
		return null;
	}

	public void GatherDataStep0()
	{
		newPlayer.playerName = characterNameTextField.GetComponent<InputField> ().text;
		newPlayer.gamerName = playerNameTextField.GetComponent<InputField> ().text;
		newPlayer.Figurine = _figurineShowcase;
		newPlayer.PlayerSprite = image.GetComponent<Image> ().sprite;

		generalPanel.SetActive (false);
		racePanel.SetActive (true);

		//PlayerDatabaseSaver.Save (newPlayer, _selected);

	}

	private void UpdateCharacterImage()
	{
		Debug.Log (characterImages [currentImage].name); 
		characterImage.GetComponent<Image> ().sprite = characterImages [currentImage];
	}

	public void NextImage()
	{
		if (currentImage < characterImages.Length - 1) {
			currentImage++;
		} else {
			currentImage = 0;
		}

		UpdateCharacterImage ();
	}

	public void PreviousImage()
	{
		if (currentImage > 0) {
			currentImage--;
		} else {
			currentImage = characterImages.Length - 1;
		}

		UpdateCharacterImage ();
	}


	#region implemented abstract members of AbstractPanelController
	protected override List<string> GetOptions ()
	{
		List<string> names = new List<string> ();

		foreach (GameObject model in figurinePrefabs) {
			names.Add (model.name);
		}

		return names;
	}
	public override void Load ()
	{
		
	}
	#endregion

	public override void Select(string pmFilename)
	{
		_selected = pmFilename;


		foreach (GameObject button in buttons) {
			button.GetComponent<Button>().interactable = true;
		}

		if (_figurineShowcase != null)
			Destroy (_figurineShowcase);

		GameObject lvResourceFig = Resources.Load<GameObject> ("Figurines/Models/" + pmFilename);

		GameObject lvFigModel = Instantiate (lvResourceFig);

		_figurineShowcase = lvFigModel;

		Destroy (_figurineShowcase.GetComponent<FigurineMover> ());

		_figurineShowcase.transform.position = new Vector3 (0.0f, 0.0f, -200.0f);


	}
}
