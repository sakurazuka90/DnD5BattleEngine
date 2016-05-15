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

		_figurineShowcase = Instantiate (lvResourceFig);

		_figurineShowcase.transform.position = new Vector3 (0.0f, 0.0f, -200.0f);

	}

}

