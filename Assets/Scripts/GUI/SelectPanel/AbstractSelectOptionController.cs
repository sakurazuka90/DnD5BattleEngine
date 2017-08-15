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
		this.gameObject.transform.Find ("Text").GetComponent<Text> ().text = pmName;
	}

	protected abstract void Select();

	protected void MarkSelectedOption()
	{
		AbstractPanelController lvController = _controller.GetComponent<AbstractPanelController> ();
		lvController.DeselectAll ();

		this.SetBackgroundColor(new Color (103.0F / 255.0F, 103.0F / 255.0F, 103.0F / 255.0F));
		this.SetTextColor (new Color (193.0F / 255.0F, 193.0F / 255.0F, 193.0F / 255.0F));
	}

	public void SetBackgroundColor(Color pmColor)
	{
		this.gameObject.GetComponent<Image> ().color = pmColor;
	}

	public void SetTextColor(Color pmColor)
	{
		this.gameObject.transform.Find ("Text").GetComponent<Text> ().color = pmColor;
	}

	public AbstractSelectOptionController ()
	{
	}
}

