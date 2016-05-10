using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractSelectOptionController : MonoBehaviour 
{
	protected GameObject _controller;

	public GameObject Controller{
		set{this._controller = value;}
	}

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<Button> ().onClick.AddListener (this.Select);
	}

	public void SetName(string pmName)
	{
		this.gameObject.transform.FindChild ("Text").GetComponent<Text> ().text = pmName;
	}



	public AbstractSelectOptionController ()
	{
	}
}

