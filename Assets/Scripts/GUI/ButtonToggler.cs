using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonToggler{

	public static void ToggleButtonOn(string pmButtonName)
	{
		GetButtonByObjectName (pmButtonName).interactable = true;
	}

	public static void ToggleButtonOff(string pmButtonName)
	{
		GetButtonByObjectName (pmButtonName).interactable = false;
	}

	public static Button GetButtonByObjectName(string pmButtonName)
	{
		return GameObject.Find (pmButtonName).GetComponent<Button> ();
	}
}
