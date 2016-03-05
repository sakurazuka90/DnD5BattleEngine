using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateNewBattlefield : MonoBehaviour {

	public void Create()
	{
		string lvX = GameObject.Find ("XInputText").GetComponent<Text> ().text;
		string lvY = GameObject.Find ("YInputText").GetComponent<Text> ().text;

		if (lvX.Length > 0 && lvY.Length > 0) {
			GameObject.Find ("GridDrawer").GetComponent<GridDrawer> ().Create (int.Parse (lvX), int.Parse (lvY));
			GameObject.Find ("CameraMover").GetComponent<MoveCamera> ().isMovable = true;
			this.gameObject.SetActive (false);
		}
	}
}
