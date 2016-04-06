using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadPanelController : MonoBehaviour {

	public GameObject content;
	public GameObject optionPrefab;

	private string _selected;


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

	public void Select(string pmFilename)
	{
		_selected = pmFilename;
		GameObject.Find ("LoadBattlefieldButton").GetComponent<Button>().interactable = true;
	}

	public void DeselectAll()
	{
		foreach (Transform lvOption in content.transform) {
			lvOption.gameObject.GetComponent<SelectOptionController> ().SetBackgroundColor (new Color (255.0F / 255.0F, 255.0F / 255.0F, 255.0F / 255.0F));
			lvOption.gameObject.GetComponent<SelectOptionController> ().SetTextColor (new Color (50.0F / 255.0F, 50.0F / 255.0F, 50.0F / 255.0F));
			this._selected = null;
		}
			
	}

	public void Load()
	{
		BattlefieldConstructor.instance.GenerateGameplay(_selected);

		UiItemLibrary.instance.spoolerPanel.SetActive (true);
		UiItemLibrary.instance.inventoryButton.SetActive (true);
		UiItemLibrary.instance.playerInfoPanel.SetActive (true);
		UiItemLibrary.instance.shortcutPanel.SetActive (true);
		UiItemLibrary.instance.skillsButton.SetActive (true);

		PlayerSpooler.instance.Run ();

		this.gameObject.SetActive (false);
	}


}
