using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AssetStatsEditor : MonoBehaviour {

	public ObstacleStatus obstacleStatus;

	public void populate()
	{
		GameObject.Find ("AssetEditNameInput").GetComponent<InputField> ().text = obstacleStatus.name;
		GameObject.Find ("DifficultTerrainToggle").GetComponent<Toggle> ().isOn = obstacleStatus.isDifficultTerrain;
		GameObject.Find ("BlocksMovementToggle").GetComponent<Toggle> ().isOn = obstacleStatus.isBlockingMovement;
		GameObject.Find ("BlocksLoSToggle").GetComponent<Toggle> ().isOn = obstacleStatus.isBlockingLoS;
		//GameObject.Find ("AssetEditNameInput").GetComponent<InputField> ().text = obstacleStatus.name;
	}

	public void clear()
	{
		obstacleStatus = null;
		GameObject.Find ("AssetEditNameInput").GetComponent<InputField> ().text = "";
		GameObject.Find ("DifficultTerrainToggle").GetComponent<Toggle> ().isOn = false;
		GameObject.Find ("BlocksMovementToggle").GetComponent<Toggle> ().isOn = false;
		GameObject.Find ("BlocksLoSToggle").GetComponent<Toggle> ().isOn = false;
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
}
