using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class AbstractPanelController : MonoBehaviour
{
	public GameObject content;
	public GameObject optionPrefab;

	public GameObject [] buttons;

	protected string _selected;

	// Use this for initialization
	void Start () {
		GenerateContent ();
	}


	public AbstractPanelController ()
	{
	}

	public virtual void GenerateContent()
	{
		List<string> lvNames = GetOptions();

		content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, lvNames.Count * 25);

		foreach (string lvName in lvNames) {
			GameObject lvInstance = Instantiate (optionPrefab);
			lvInstance.transform.parent = content.transform;
			lvInstance.GetComponent<SelectOptionController> ().SetName (lvName);
		}
	}

	protected abstract List<string> GetOptions();

	public virtual void Select(string pmFilename)
	{
		_selected = pmFilename;

		foreach (GameObject button in buttons) {
			button.GetComponent<Button>().interactable = true;
		}
	}

	public virtual void DeselectAll()
	{
		foreach (Transform lvOption in content.transform) {
			lvOption.gameObject.GetComponent<SelectOptionController> ().SetBackgroundColor (new Color (255.0F / 255.0F, 255.0F / 255.0F, 255.0F / 255.0F));
			lvOption.gameObject.GetComponent<SelectOptionController> ().SetTextColor (new Color (50.0F / 255.0F, 50.0F / 255.0F, 50.0F / 255.0F));
			this._selected = null;
		}

	}

	public abstract void Load();


}


