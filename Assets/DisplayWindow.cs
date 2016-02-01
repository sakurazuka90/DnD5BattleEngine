using UnityEngine;
using System.Collections;

public class DisplayWindow : MonoBehaviour {

	public Rect windowRect = new Rect(20, 20, 120, 50);

	public bool displayExpWindow;

	public GUIContent lvGui;

	void OnGUI() {
		if(displayExpWindow)
			windowRect = GUI.Window(0, windowRect, DoMyWindow, "Battle Summary");
	}
	void DoMyWindow(int windowID) {
		if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
			print("Got a click");

		GUI.DragWindow(new Rect(0, 0, 10000, 20));

	}
}
