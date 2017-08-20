using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AssetStatsEditor : MonoBehaviour {

	public ObstacleStatus obstacleStatus;
	public GameObject removeButton;
	public GameObject rotateButton;

	public GameObject nameInput;
	public GameObject difficultTerrainToggle;
	public GameObject blocksMovementToggle;
	public GameObject blocksLoSToggle;

	public GameObject functionalNameInput;
	public GameObject functionalImage;
	public GameObject functionImput;
	public GameObject functionalRemoveButton;
	public GameObject functionalSelectButton;
	public CellStatus editedFunctionalCell;

	public GameObject figurineSelectPanel;


	public void populate()
	{
		nameInput.GetComponent<InputField> ().text = obstacleStatus.name;
		difficultTerrainToggle.GetComponent<Toggle> ().isOn = obstacleStatus.isDifficultTerrain;
		blocksMovementToggle.GetComponent<Toggle> ().isOn = obstacleStatus.isBlockingMovement;
		blocksLoSToggle.GetComponent<Toggle> ().isOn = obstacleStatus.isBlockingLoS;

		removeButton.GetComponent<Button> ().interactable = true;
		rotateButton.GetComponent<Button> ().interactable = true;
	}

	public void populateFunctional(string pmFunctionalName, string pmFunctionalButtonName, CellStatus pmStatus)
	{
		functionalNameInput.GetComponent<InputField> ().text = pmFunctionalName;
		Sprite lvButtonSprite = Resources.Load<Sprite> ("ObstaclesImages/Functional/"+pmFunctionalButtonName);

		functionalImage.GetComponent<Image> ().sprite = lvButtonSprite;
		pmStatus.edited = true;
		editedFunctionalCell = pmStatus;

		functionImput.GetComponent<InputField>().text = "DYNAMIC";
		functionalSelectButton.GetComponent<Button> ().interactable = true;
		functionalRemoveButton.GetComponent<Button> ().interactable = true;

		pmStatus.Function = "DYNAMIC";
		pmStatus.FigurineId = -1;
	}

	public void populateFunctional(string pmFunctionalName, string pmFunctionalButtonName, CellStatus pmStatus, string pmFunction)
	{
		populateFunctional (pmFunctionalName, pmFunctionalButtonName, pmStatus);
		functionImput.GetComponent<InputField> ().text = pmFunction;
	}

	public void RemoveFunctional()
	{
		editedFunctionalCell.functionalState = FunctionalStates.NONE;
		editedFunctionalCell.Function = "";
		editedFunctionalCell.FigurineId = 0;

		clearFunctional ();
	}

	public void clearFunctional()
	{
		functionalNameInput.GetComponent<InputField> ().text = "";
		functionImput.GetComponent<InputField> ().text = "";
		functionalImage.GetComponent<Image> ().sprite = null;


		if(editedFunctionalCell != null)
			editedFunctionalCell.edited = false;

		functionalSelectButton.GetComponent<Button> ().interactable = false;
		functionalRemoveButton.GetComponent<Button> ().interactable = false;
	}

	public void clear()
	{
		obstacleStatus = null;
		nameInput.GetComponent<InputField> ().text = "";
		difficultTerrainToggle.GetComponent<Toggle> ().isOn = false;
		blocksMovementToggle.GetComponent<Toggle> ().isOn = false;
		blocksLoSToggle.GetComponent<Toggle> ().isOn = false;

		removeButton.GetComponent<Button> ().interactable = false;
		rotateButton.GetComponent<Button> ().interactable = false;

		//GameObject.Find ("AssetEditNameInput").GetComponent<InputField> ().text = obstacleStatus.name;
	}

	public void setDifficultTerrain(bool pmState)
	{
		if(obstacleStatus != null)
		obstacleStatus.isDifficultTerrain = pmState;
	}

	public void setBlocksMovement(bool pmState)
	{
		if(obstacleStatus != null)
		obstacleStatus.isBlockingMovement = pmState;
	}

	public void setBlocksLoS(bool pmState)
	{
		if(obstacleStatus != null)
		obstacleStatus.isBlockingLoS = pmState;
	}

	public void rotateObstacle()
	{
		GameObject obstacle = GameObject.Find (obstacleStatus.name);

		GameObject.Find ("AssetPanel").GetComponent<PlaceNewObstacle> ().PickObstacle (obstacle);
		List<int> fields = obstacle.GetComponent<ObstacleStatus> ().fieldsUsed;
		CreatorSelectFromGrid.instance.RemoveObstacleFromField (fields);
	}

	public void removeObstacle()
	{
		GameObject obstacle = GameObject.Find (obstacleStatus.name);
		List<int> fields = obstacle.GetComponent<ObstacleStatus> ().fieldsUsed;
		foreach (int id in fields) {
			GridDrawer.instance.mCells [id].GetComponent<CellStatus> ().obstacle = null;
		}
		CreatorSelectFromGrid.instance.RemoveObstacleFromField (fields);
		Destroy (obstacle);
		this.clear ();
	}

	public void SetFunction(string pmFunction)
	{
		editedFunctionalCell.Function = pmFunction;
		functionImput.GetComponent<InputField> ().text = pmFunction;
	}

	public void SetFigurineId(int pmId)
	{
		editedFunctionalCell.FigurineId = pmId;
	}

	public void ShowFigurineSelectPanel()
	{
		figurineSelectPanel.SetActive (true);
	}

	public void SetCoverValue(int pmValue)
	{
		if (obstacleStatus != null)
			obstacleStatus.coverValue = pmValue;
	}
}
