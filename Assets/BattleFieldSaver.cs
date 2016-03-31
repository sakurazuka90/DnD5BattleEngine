using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BattleFieldSaver : MonoBehaviour
{

	public GameObject assetPanel;
	public GameObject assetStatsPanel;
	public GameObject saveButton;
	public GameObject saveNamePanel;

	public GameObject genericYesNoWindowPrefab;

	private static string _FileExistsMessage = "File with that name already exists. Do you want to save anyway?";

	public void Save ()
	{
		GameObject lvObject = saveNamePanel.transform.FindChild ("SaveFileName").gameObject;
		string lvFileName = lvObject.GetComponent<InputField> ().text;

		if (File.Exists (Application.persistentDataPath + "/" + lvFileName + ".dat")) {

			GameObject lvWindow = GameObject.Instantiate (genericYesNoWindowPrefab);
			lvWindow.GetComponent<GenericyesNoPanelControler> ().InitializePanel (_FileExistsMessage, this.SaveMapToFile);

			lvWindow.transform.parent = GameObject.Find ("Canvas").transform;
			lvWindow.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;

		} else {
			SaveMapToFile (lvFileName);
			Hide ();
		}


	}

	public void SaveMapToFile ()
	{
		GameObject lvObject = saveNamePanel.transform.FindChild ("SaveFileName").gameObject;
		string lvFileName = lvObject.GetComponent<InputField> ().text;
		SaveMapToFile (lvFileName);
	}

	public void SaveMapToFile (string pmFileName)
	{
		BinaryFormatter lvFormater = new BinaryFormatter ();
		FileStream lvFile = File.Open (Application.persistentDataPath + "/" + pmFileName + ".dat", FileMode.OpenOrCreate);

		GridData lvData = new GridData ();
		lvData.x = GridDrawer.instance.gridWidth;
		lvData.z = GridDrawer.instance.gridHeight;

		lvFormater.Serialize (lvFile, lvData);
		lvFile.Close ();
	}

	public void Show ()
	{
		saveButton.SetActive (false);
		assetPanel.SetActive (false);
		assetStatsPanel.SetActive (false);
		saveNamePanel.SetActive (true);
		GameObject lvObject = saveNamePanel.transform.FindChild ("SaveFileName").gameObject;
		lvObject.GetComponent<InputField> ().text = "NewMap";
	}

	public void Hide ()
	{
		saveButton.SetActive (true);
		assetPanel.SetActive (true);
		assetStatsPanel.SetActive (true);
		saveNamePanel.SetActive (false);
	}


}



