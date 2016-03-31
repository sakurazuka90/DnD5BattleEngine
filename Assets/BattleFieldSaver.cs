using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BattleFieldSaver : MonoBehaviour {

	public GameObject assetPanel;
	public GameObject assetStatsPanel;
	public GameObject saveButton;
	public GameObject saveNamePanel;

	public void Save()
	{
		GameObject lvObject = saveNamePanel.transform.FindChild ("SaveFileName").gameObject;
		string lvFileName = lvObject.GetComponent<InputField>().text;

		if (File.Exists (Application.persistentDataPath + "/" + lvFileName + ".dat")) {
		} else {
			BinaryFormatter lvFormater = new BinaryFormatter ();
			FileStream lvFile = File.Open (Application.persistentDataPath + "/"+ lvFileName +".dat", FileMode.OpenOrCreate);

			GridData lvData = new GridData ();
			lvData.x = GridDrawer.instance.gridWidth;
			lvData.z = GridDrawer.instance.gridHeight;

			lvFormater.Serialize (lvFile, lvData);
			lvFile.Close ();

			Hide ();
		}


	}

	public void Show()
	{
		saveButton.SetActive (false);
		assetPanel.SetActive (false);
		assetStatsPanel.SetActive (false);
		saveNamePanel.SetActive (true);
		GameObject lvObject = saveNamePanel.transform.FindChild ("SaveFileName").gameObject;
		lvObject.GetComponent<InputField>().text = "NewMap";
	}

	public void Hide()
	{
		saveButton.SetActive (true);
		assetPanel.SetActive (true);
		assetStatsPanel.SetActive (true);
		saveNamePanel.SetActive (false);
	}


}



