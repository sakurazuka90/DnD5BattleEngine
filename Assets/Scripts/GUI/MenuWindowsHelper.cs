using UnityEngine;
using System.Collections;

public class MenuWindowsHelper : MonoBehaviour {

	public GameObject EditFigurineSubmenu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowEditFigurineSubmenu()
	{
		EditFigurineSubmenu.SetActive (true);
	}

	public void HideEditFigurineSubmenu()
	{
		EditFigurineSubmenu.SetActive (false);
	}
}
