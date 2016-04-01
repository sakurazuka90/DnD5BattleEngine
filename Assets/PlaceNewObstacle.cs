using UnityEngine;
using System.Collections;

public class PlaceNewObstacle : MonoBehaviour {

	public GameObject obstacle;

	public void Place()
	{
		GameObject lvObstacle = GameObject.Instantiate (obstacle);

		lvObstacle.name = GameObject.Find ("CreatorUniqueController").GetComponent<CreatorNameController> ().CreateUniqueName (lvObstacle.name);

		FigurineStatus lvStatus = lvObstacle.GetComponent<FigurineStatus> ();
		lvStatus.active = true;
		lvStatus.picked = true;

		ObstacleStatus lvObstacleStatus = lvObstacle.GetComponent<ObstacleStatus> ();
		lvObstacleStatus.name = lvObstacle.name;
		lvObstacleStatus.prefabName = obstacle.name;
		AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();

		if(lvStatusEditor.obstacleStatus != null && !"".Equals(lvStatusEditor.obstacleStatus.name))
		{
			GameObject lvOldObstacle = GameObject.Find (lvStatusEditor.obstacleStatus.name);
			lvOldObstacle.GetComponent<ShaderSwitcher> ().SwitchOutlineOff();
		}


		lvStatusEditor.obstacleStatus = lvObstacleStatus;
		lvStatusEditor.populate ();

		lvObstacle.GetComponent<ShaderSwitcher> ().SwitchOutlineOn();

		GameObject.Find ("GridSelector").GetComponent<SelectFromGrid> ().creatorObstacle = lvObstacle;
	}
}
