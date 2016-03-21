using UnityEngine;
using System.Collections;

public class PlaceNewObstacle : MonoBehaviour {

	public GameObject obstacle;

	public void Place()
	{
		GameObject lvObstacle = GameObject.Instantiate (obstacle);
		FigurineStatus lvStatus = lvObstacle.GetComponent<FigurineStatus> ();
		lvStatus.active = true;
		lvStatus.picked = true;

		ObstacleStatus lvObstacleStatus = lvObstacle.GetComponent<ObstacleStatus> ();
		lvObstacleStatus.name = lvObstacle.name;
		AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();
		lvStatusEditor.obstacleStatus = lvObstacleStatus;
		lvStatusEditor.populate ();

		lvObstacle.GetComponent<ShaderSwitcher> ().SwitchOutlineOn();

		GameObject.Find ("GridSelector").GetComponent<SelectFromGrid> ().creatorObstacle = lvObstacle;
	}
}
