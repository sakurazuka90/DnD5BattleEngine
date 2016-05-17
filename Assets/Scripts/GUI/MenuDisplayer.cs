using UnityEngine;
using System.Collections;

public class MenuDisplayer : MonoBehaviour {

	public GameObject menuPanel;

	public static MenuDisplayer instance;

	public bool isMenuAvaiable = false;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape) && isMenuAvaiable)
			menuPanel.SetActive (!menuPanel.activeSelf);

	}

	public void Hide()
	{
		menuPanel.SetActive (false);
	}

	public void Show()
	{
		menuPanel.SetActive (true);
	}
}
