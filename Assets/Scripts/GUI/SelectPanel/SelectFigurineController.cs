using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectFigurineController : AbstractPanelController
{

	private List<int> _idsList;
	private Dictionary<int,string> _values;

	public GameObject rawImage;

	private GameObject _figurineShowcase;

	public GameObject characterNameText;
	public GameObject sizeText;
	public GameObject typeText;
	public GameObject subtypeText;
	public GameObject abilitiesPanel;
	public GameObject savesPanel;

	private bool _rotate = false;
	private float _direction = 0.0f;

	public void Update()
	{
		if (_rotate) {
			RotateShowcase (_direction);
		}
	}


	public SelectFigurineController ()
	{
	}

	#region implemented abstract members of AbstractPanelController

	protected override List<string> GetOptions ()
	{
		_values = DatabaseController.GetListOfValues ();
		List<string> lvNames = new List<string> ();
		_idsList = new List<int> ();
		foreach (int lvId in _values.Keys) {
			_idsList.Add (lvId);
			lvNames.Add (_values [lvId]);
		}

		return lvNames;
	}

	public override void Load ()
	{
		int lvId = int.Parse (_selected);

		AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();
		lvStatusEditor.SetFunction (_values[lvId]);
		lvStatusEditor.SetFigurineId (lvId);
		this.gameObject.SetActive (false);
	}

	#endregion

	public override void GenerateContent()
	{
		List<string> lvNames = GetOptions();

		content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, lvNames.Count * 25);

		for (int i = 0; i < lvNames.Count; i++) {
			GameObject lvInstance = Instantiate (optionPrefab);
			lvInstance.transform.SetParent(content.transform);
			lvInstance.GetComponent<SelectFigurineOptionController> ().SetName (lvNames[i]);
			lvInstance.GetComponent<SelectFigurineOptionController> ().NumericValue = _idsList [i];
			lvInstance.GetComponent<SelectFigurineOptionController> ().Controller = this.gameObject;
		}

	}

	public override void Select(string pmFilename)
	{
		_selected = pmFilename;


		foreach (GameObject button in buttons) {
			button.GetComponent<Button>().interactable = true;
		}

		if (_figurineShowcase != null)
			Destroy (_figurineShowcase);

		GameObject lvResourceFig = Resources.Load<GameObject> ("Figurines/Models/" + DatabaseController.GetFigurineNameByPlayerID(int.Parse(pmFilename)));

		GameObject lvFigModel = Instantiate (lvResourceFig);

		_figurineShowcase = lvFigModel;

		Destroy (_figurineShowcase.GetComponent<FigurineMover> ());

		_figurineShowcase.transform.position = new Vector3 (0.0f, 0.0f, -200.0f);


	}

	public void FillCharacterData(int pmCharacterId)
	{
		Player lvSelectedPlayer = DatabaseController.GetPlayerByID (pmCharacterId);
		lvSelectedPlayer.Saves = DatabaseController.GetCharacterSavesByCharacterId (pmCharacterId);
		characterNameText.GetComponent<Text> ().text = lvSelectedPlayer.playerName;
		sizeText.GetComponent<Text> ().text = Dictionaries.sizes[lvSelectedPlayer.Size];
		typeText.GetComponent<Text> ().text = Dictionaries.types[lvSelectedPlayer.CharacterType];
		subtypeText.GetComponent<Text> ().text = lvSelectedPlayer.Subtype;
		abilitiesPanel.GetComponent<AbilityPanel> ().FillAbilities (lvSelectedPlayer);
		savesPanel.GetComponent<SavesPanelController> ().FillSaves(lvSelectedPlayer);

	}

	public void RotateShowcaseRight()
	{
		_direction = 1.0f;
		_rotate = true;
	}

	public void RotateShowcaseLeft()
	{
		_direction = -1.0f;
		_rotate = true;
	}

	private void RotateShowcase(float pmDirection)
	{
		_figurineShowcase.transform.Rotate (new Vector3 (0.0f, 5.0f * pmDirection, 0.0f));
	}

	public void StopRotate()
	{
		_direction = 0.0f;
		_rotate = false;
	}

}

