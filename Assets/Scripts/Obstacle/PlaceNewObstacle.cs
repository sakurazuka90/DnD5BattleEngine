﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaceNewObstacle : MonoBehaviour
{

	public GameObject obstacle;
	public GameObject content;
	public GameObject genericButtonPrefab;

	private int _selectedOption;
	private string[] _options;

	void Start ()
	{
		GenerateContent ("Dungeon");
		_selectedOption = 0;
		_options = new string[2];

		_options [0] = "Dungeon";
		_options [1] = "Functional";
	}

	public void GenerateContent (int pmOptionId)
	{
		GenerateContent (_options [pmOptionId]);
		_selectedOption = pmOptionId;
	}

	public void GenerateContent (string pmCategoryName)
	{
		GameObjectUtils.RemoveAllChildren (content);

		Sprite[] lvItems = Resources.LoadAll<Sprite> ("ObstaclesImages/" + pmCategoryName); 

		foreach (Sprite lvImage in lvItems) {
			GameObject lvInstance = Instantiate (genericButtonPrefab);
			lvInstance.GetComponent<Image> ().sprite = lvImage;

			string name = lvImage.name;

			lvInstance.GetComponent<GenericItemButton> ().GetComponent<Button> ().onClick.AddListener (() => {
				Place (name);
			});

			lvInstance.transform.SetParent(content.transform);
		}
	}


	public void Place (string pmPrefabName)
	{
		if (_selectedOption != 1) {
			SelectFromGrid.instance.functionalPlaceMode = false;

			obstacle = Resources.Load<GameObject> ("ObstaclePrefabs/" + pmPrefabName);

			GameObject lvObstacle = GameObject.Instantiate (obstacle);

			lvObstacle.name = GameObject.Find ("CreatorUniqueController").GetComponent<CreatorNameController> ().CreateUniqueName (lvObstacle.name);

			FigurineStatus lvStatus = lvObstacle.GetComponent<FigurineStatus> ();
			lvStatus.active = true;
			lvStatus.picked = true;

			ObstacleStatus lvObstacleStatus = lvObstacle.GetComponent<ObstacleStatus> ();
			lvObstacleStatus.name = lvObstacle.name;
			lvObstacleStatus.prefabName = obstacle.name;
			AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();

			if (lvStatusEditor.obstacleStatus != null && !"".Equals (lvStatusEditor.obstacleStatus.name)) {
				GameObject lvOldObstacle = GameObject.Find (lvStatusEditor.obstacleStatus.name);
				lvOldObstacle.GetComponent<ShaderSwitcher> ().SwitchOutlineOff ();
			}


			lvStatusEditor.obstacleStatus = lvObstacleStatus;
			lvStatusEditor.populate ();

			lvObstacle.GetComponent<ShaderSwitcher> ().SwitchOutlineOn ();

			GameObject.Find ("GridSelector").GetComponent<SelectFromGrid> ().creatorObstacle = lvObstacle;
		} else {
			SelectFromGrid.instance.functionalPlaceMode = true;

			switch (pmPrefabName) {
			case "SpawnPlayersSprite":
				SelectFromGrid.instance.currentFunctionalState = FunctionalStates.PLAYER_SPAWN;
				break;
			case "SpawnPlayersSprite2":
				SelectFromGrid.instance.currentFunctionalState = FunctionalStates.ENEMY_SPAWN;
				break;
			}
		}
	}
}