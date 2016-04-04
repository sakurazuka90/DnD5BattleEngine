using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadPanelController : MonoBehaviour {

	public GameObject content;
	public GameObject optionPrefab;

	private GameObject _selected;


	// Use this for initialization
	void Start () {
		GenerateContent ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void GenerateContent()
	{
		List<string> lvNames = BattlefieldStateReader.instance.ListFiles ();

		content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, lvNames.Capacity * 25);

		foreach (string lvName in lvNames) {
			GameObject lvInstance = Instantiate (optionPrefab);
			lvInstance.transform.parent = content.transform;
			lvInstance.GetComponent<SelectOptionController> ().SetName (lvName);
		}
	}

	public void Select(GameObject pmGameObject)
	{
		_selected = pmGameObject;
	}

	public void DeselectAll()
	{
		foreach (Transform lvOption in content.transform) {
			lvOption.gameObject.GetComponent<SelectOptionController> ().SetBackgroundColor (new Color (255.0F / 255.0F, 255.0F / 255.0F, 255.0F / 255.0F));
			lvOption.gameObject.GetComponent<SelectOptionController> ().SetTextColor (new Color (50.0F / 255.0F, 50.0F / 255.0F, 50.0F / 255.0F));
			this._selected = null;
		}
			
	}


}
