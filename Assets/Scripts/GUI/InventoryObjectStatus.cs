using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryObjectStatus : MonoBehaviour {

	public string InventorySlotId;

	public List<EquipementTypes> EquipementTypes;

	public void ShowStatus()
	{
		Debug.Log ("OPEN! SESAMEEE! " + InventorySlotId);

		if (Input.GetMouseButtonDown (1)) {
			Dictionary<string, Item> lvInventory = PlayerSpooler.instance.GetSpooledPlayer ().Inventory;
			Item lvItem = lvInventory [InventorySlotId];

			DisplayWindow.instance.OpenItemDisplay (lvItem);
		}
	}
}
