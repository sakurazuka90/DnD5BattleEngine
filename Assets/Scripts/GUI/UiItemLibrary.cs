using UnityEngine;
using System.Collections;

public class UiItemLibrary : MonoBehaviour {

	public GameObject playerInfoPanel;
	public GameObject spoolerPanel;
	public GameObject inventoryPanel;
	public GameObject shortcutPanel;
	public GameObject abilitiesPanel;
	public GameObject fileLoadPanel;

	public GameObject skillsButton;
	public GameObject inventoryButton;
	public GameObject selectFigurinePanel;

	public static UiItemLibrary instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this);
	}
}
