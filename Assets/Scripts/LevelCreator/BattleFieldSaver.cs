using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BattleFieldSaver : MonoBehaviour
{

	public GameObject assetPanel;
	public GameObject assetStatsPanel;
	public GameObject saveNamePanel;

	public GameObject genericYesNoWindowPrefab;
	public GameObject genericAlertWindowPrefab;

	private static string _FileExistsMessage = "File with that name already exists. Do you want to save anyway?";
	private static string _NotEnoughSpawnPointsMessage = "There is not enough spawn points ont the map. Add at least one for each side of conflict";

	public void Save ()
	{
		if (GridDrawer.instance.GetFunctionalFields (FunctionalStates.PLAYER_SPAWN).Capacity == 0 || GridDrawer.instance.GetFunctionalFields (FunctionalStates.ENEMY_SPAWN).Capacity == 0) {
			GameObject lvWindow = GameObject.Instantiate (genericAlertWindowPrefab);
			lvWindow.GetComponent<GenericyesNoPanelControler> ().InitializePanel (_NotEnoughSpawnPointsMessage, null);

			lvWindow.transform.parent = GameObject.Find ("Canvas").transform;
			lvWindow.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		} else {

			GameObject lvObject = saveNamePanel.transform.Find ("SaveFileName").gameObject;
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

	}

	public void SaveMapToFile ()
	{
		GameObject lvObject = saveNamePanel.transform.Find ("SaveFileName").gameObject;
		string lvFileName = lvObject.GetComponent<InputField> ().text;
		SaveMapToFile (lvFileName);
		this.Hide ();
	}

	public void SaveMapToFile (string pmFileName)
	{
		BinaryFormatter lvFormater = new BinaryFormatter ();
		FileStream lvFile = File.Open (Application.persistentDataPath + "/" + pmFileName + ".dat", FileMode.OpenOrCreate);

		GridData lvData = new GridData ();
		lvData.x = GridDrawer.instance.gridWidth;
		lvData.z = GridDrawer.instance.gridHeight;
		lvData.graphicsStyle = FloorCreator.instance.GraphicsStyle;
		lvData.obstacles = CollectObstacleData ();
		lvData.players = CollectPlayersData ();
		lvData.enemies = CollectEnemiesData ();

		lvFormater.Serialize (lvFile, lvData);
		lvFile.Close ();
	}

	public void Show ()
	{
		MenuDisplayer.instance.Hide ();

		assetPanel.SetActive (false);
		assetStatsPanel.SetActive (false);
		saveNamePanel.SetActive (true);
		GameObject lvObject = saveNamePanel.transform.Find ("SaveFileName").gameObject;
		lvObject.GetComponent<InputField> ().text = "NewMap";

		MoveCamera.instance.isMovable = false;
	}

	public void Hide ()
	{
		assetPanel.SetActive (true);
		assetStatsPanel.SetActive (true);
		saveNamePanel.SetActive (false);

		MoveCamera.instance.isMovable = true;
	}

	private ObstacleData[] CollectObstacleData()
	{
		GameObject[] lvCells = GridDrawer.instance.mCells;
		ObstacleData [] lvData = new ObstacleData[lvCells.Length];

		for (int i = 0; i < lvCells.Length; i++) {
			if (lvCells [i].transform.childCount > 0) {
				lvData [i] = lvCells [i].transform.GetChild (0).gameObject.GetComponent<ObstacleStatus> ().getData();
			} else {
				lvData [i] = null;
			}
		}

		return lvData;
	}

	private int [] CollectPlayersData()
	{
		
		GameObject[] lvCells = GridDrawer.instance.mCells;
		int[] lvIds = new int[lvCells.Length];

		for(int i = 0; i < lvCells.Length; i++) {

			CellStatus lvStatus = lvCells[i].GetComponent<CellStatus> ();
			if (lvStatus.functionalState == FunctionalStates.PLAYER_SPAWN) {
				lvIds [i] = lvStatus.FigurineId;
			} else {
				lvIds [i] = 0;
			}

		}



		return lvIds;

	}

	private int [] CollectEnemiesData()
	{

		GameObject[] lvCells = GridDrawer.instance.mCells;
		int[] lvIds = new int[lvCells.Length];

		for(int i = 0; i < lvCells.Length; i++) {

			CellStatus lvStatus = lvCells[i].GetComponent<CellStatus> ();
			if (lvStatus.functionalState == FunctionalStates.ENEMY_SPAWN) {
				lvIds [i] = lvStatus.FigurineId;
			} else {
				lvIds [i] = 0;
			}

		}

		return lvIds;

	}



}



