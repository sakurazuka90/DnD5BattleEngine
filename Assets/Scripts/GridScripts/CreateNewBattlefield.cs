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
		int graphicStyle = GameObject.Find ("GraphicStyleDropdown").GetComponent<Dropdown> ().value;

		if (lvX.Length > 0 && lvY.Length > 0) {
			BattlefieldConstructor.instance.GenerateGrid (int.Parse(lvX), int.Parse(lvY));
			BattlefieldConstructor.instance.SetupCameraMover (float.Parse (lvX), float.Parse (lvY));
			BattlefieldConstructor.instance.CreateFloor (int.Parse (lvX), int.Parse (lvY), graphicStyle);
			BattlefieldConstructor.instance.CreateWalls (int.Parse (lvX), int.Parse (lvY));

			obstacleWindow.SetActive (true);
			obstacleStatusWindow.SetActive (true);
			MenuDisplayer.instance.isMenuAvaiable = true;

			this.gameObject.SetActive (false);
		}
	}
}
