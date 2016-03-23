using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class CreateNewBattlefield : MonoBehaviour {

	public GameObject obstacleWindow;
	public GameObject obstacleStatusWindow;

	public void Create()
	{
		string lvX = GameObject.Find ("XInputText").GetComponent<Text> ().text;
		string lvY = GameObject.Find ("YInputText").GetComponent<Text> ().text;

		if (lvX.Length > 0 && lvY.Length > 0) {
			GridDrawer.instance.Create (int.Parse (lvX), int.Parse (lvY));

			MoveCamera lvMover = GameObject.Find ("CameraMover").GetComponent<MoveCamera> ();
			lvMover.isMovable = true;
			lvMover.maxX = float.Parse (lvX);
			lvMover.maxZ = float.Parse (lvY);
			obstacleWindow.SetActive (true);
			obstacleStatusWindow.SetActive (true);
			this.gameObject.SetActive (false);
		}
	}
}
