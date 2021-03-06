﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaceNewObstacle : MonoBehaviour
{

	public GameObject obstacle;
	public GameObject content;
	public GameObject genericButtonPrefab;
	public GameObject dropdownObject;

	public int _selectedOption;
	private string[] _options;

	void Start ()
	{
		_options = new string[3];

		_options [0] = "Dungeon";
		_options [1] = "Woodland";
		_options [2] = "Functional";

		dropdownObject.GetComponent<Dropdown> ().value = _selectedOption;
		GenerateContent (_selectedOption);

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

			lvInstance.GetComponent<Button> ().onClick.AddListener (() => {
				Place (name);
			});

			lvInstance.transform.SetParent(content.transform);
		}
	}

	public void PickObstacle(GameObject obstacle)
	{
		CreatorSelectFromGrid.instance.functionalPlaceMode = false;
		FigurineStatus lvStatus = obstacle.GetComponent<FigurineStatus> ();
		lvStatus.active = true;
		lvStatus.picked = true;

		AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();

		if (lvStatusEditor.obstacleStatus != null && !"".Equals (lvStatusEditor.obstacleStatus.name)) {
			GameObject lvOldObstacle = GameObject.Find (lvStatusEditor.obstacleStatus.name);
			lvOldObstacle.GetComponent<ShaderSwitcher> ().SwitchOutlineOff ();
		}

		ObstacleStatus lvObstacleStatus = obstacle.GetComponent<ObstacleStatus> ();

		lvStatusEditor.obstacleStatus = lvObstacleStatus;
		lvStatusEditor.populate ();

		obstacle.GetComponent<ShaderSwitcher> ().SwitchOutlineOn ();
		CreatorSelectFromGrid.instance.creatorObstacle = obstacle;
	}

	public void Place (string pmPrefabName)
	{
		if (_selectedOption != 2) {
			obstacle = Resources.Load<GameObject> ("ObstaclePrefabs/" + pmPrefabName);

			GameObject lvObstacle = GameObject.Instantiate (obstacle);

			lvObstacle.name = GameObject.Find ("CreatorUniqueController").GetComponent<CreatorNameController> ().CreateUniqueName (lvObstacle.name);

			ObstacleStatus lvObstacleStatus = lvObstacle.GetComponent<ObstacleStatus> ();
			lvObstacleStatus.name = lvObstacle.name;
			lvObstacleStatus.prefabName = obstacle.name;

			PickObstacle (lvObstacle);
		} else {
			CreatorSelectFromGrid.instance.functionalPlaceMode = true;

			switch (pmPrefabName) {
			case "SpawnPlayersSprite":
				CreatorSelectFromGrid.instance.currentFunctionalState = FunctionalStates.PLAYER_SPAWN;
				break;
			case "SpawnPlayersSprite2":
				CreatorSelectFromGrid.instance.currentFunctionalState = FunctionalStates.ENEMY_SPAWN;
				break;
			}
		}
	}
}
