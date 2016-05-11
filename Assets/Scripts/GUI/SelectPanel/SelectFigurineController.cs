using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectFigurineController : AbstractPanelController
{

	private List<int> _idsList;

	public SelectFigurineController ()
	{
	}

	#region implemented abstract members of AbstractPanelController

	protected override List<string> GetOptions ()
	{
		Dictionary<int,string> lvValue = DatabaseController.GetListOfValues ();
		List<string> lvNames = new List<string> ();
		_idsList = new List<int> ();
		foreach (int lvId in lvValue.Keys) {
			_idsList.Add (lvId);
			lvNames.Add (lvValue [lvId]);
		}

		return lvNames;
	}

	public override void Load ()
	{
		AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();
		lvStatusEditor.SetFunction (_selected);
		this.gameObject.SetActive (false);
	}

	#endregion

	public override void GenerateContent()
	{
		List<string> lvNames = GetOptions();

		content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, lvNames.Count * 25);


		foreach (string lvName in lvNames) {
			GameObject lvInstance = Instantiate (optionPrefab);
			lvInstance.transform.SetParent(content.transform);
			lvInstance.GetComponent<SelectOptionController> ().SetName (lvName);
			lvInstance.GetComponent<SelectOptionController> ().Controller = this.gameObject;
		}
	}
}

