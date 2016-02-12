using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

	public GameObject Item{
		get { 
			if (transform.childCount > 0)
				return transform.GetChild (0).gameObject;

			return null;
		}
	}

	public void OnDrop (PointerEventData eventData)
	{
		if (!Item) {
			DragDropHandler.draggedObject.transform.parent = transform;

			InventoryObjectStatus lvStatus = DragDropHandler.draggedObject.GetComponent<InventoryObjectStatus> ();

			PlayerSpooler lvSpooler = GameObject.Find ("PlayerSpooler").GetComponent<PlayerSpooler> ();
			Dictionary<string, Item> lvInventory = lvSpooler.GetSpooledPlayer ().Inventory;

			Item lvItem = lvInventory [lvStatus.InventorySlotId];
			lvInventory.Remove (lvStatus.InventorySlotId);
			lvItem.inventoryFieldId = this.gameObject.name;
			lvInventory.Add (this.gameObject.name, lvItem);
			lvStatus.InventorySlotId = this.gameObject.name;

		}
	}

}
