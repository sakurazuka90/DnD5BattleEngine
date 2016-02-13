using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayWindow : MonoBehaviour {

	public Rect windowRect = new Rect(20, 20, 600, 500);

	public bool displayExpWindow;
	public bool displayEditor;

	public GameObject InventoryPanel;

	public Image lvImage;
	void OnGUI() {
		if(displayExpWindow)
			windowRect = GUI.Window(0, windowRect, DoMyWindow, "Battle Summary");

	}
	void DoMyWindow(int windowID) {
		if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
			print("Got a click");

		GUI.DragWindow(new Rect(0, 0, 10000, 20));



	}

	public void ToggleInventoryPanel()
	{
		InventoryPanel.SetActive (!InventoryPanel.activeSelf);

		SelectFromGrid lvSelector = GameObject.Find ("GridSelector").GetComponent<SelectFromGrid> ();

		lvSelector.inventoryOpen = !lvSelector.inventoryOpen;
		//lvSelector.ClearWalkableLine ();
	}
}
