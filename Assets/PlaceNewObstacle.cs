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

		GameObject.Find ("GridSelector").GetComponent<SelectFromGrid> ().creatorObstacle = lvObstacle;
	}
}
