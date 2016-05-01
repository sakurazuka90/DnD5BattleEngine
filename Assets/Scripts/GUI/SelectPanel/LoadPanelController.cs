using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class LoadPanelController : AbstractPanelController {

	protected override List<string> GetOptions ()
	{
		return BattlefieldStateReader.instance.ListFiles ();
	}

	public override void Load()
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

	public void Delete()
	{
		if (File.Exists (Application.persistentDataPath + "/" + _selected)) {
			File.Delete (Application.persistentDataPath + "/" + _selected);
			DeselectAll ();
			GameObject.Find ("LoadBattlefieldButton").GetComponent<Button>().interactable = false;
			GameObject.Find ("DeleteFileButton").GetComponent<Button>().interactable = false;
			GameObjectUtils.RemoveAllChildren (content);
			this.GenerateContent ();
		}
	}


}
