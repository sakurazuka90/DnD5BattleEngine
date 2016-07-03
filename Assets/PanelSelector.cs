using UnityEngine;
using System.Collections;

public class PanelSelector : MonoBehaviour {

	public GameObject[] panels;

	public void ShowPanel(int pmIndex)
	{
		foreach (GameObject panel in panels) {
			panel.SetActive (false);
		}

		panels [pmIndex].SetActive (true);
	}
}
