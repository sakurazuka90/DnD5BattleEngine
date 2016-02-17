using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

	public EquipementTypes EquipementType;

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
			

			InventoryObjectStatus lvStatus = DragDropHandler.draggedObject.GetComponent<InventoryObjectStatus> ();

			PlayerSpooler lvSpooler = GameObject.Find ("PlayerSpooler").GetComponent<PlayerSpooler> ();
			Dictionary<string, Item> lvInventory = lvSpooler.GetSpooledPlayer ().Inventory;
			Item lvItem = lvInventory [lvStatus.InventorySlotId];

			if (isEquipementAllowed (lvItem)) {
				DragDropHandler.draggedObject.transform.parent = transform;
				lvInventory.Remove (lvStatus.InventorySlotId);
				lvItem.inventoryFieldId = this.gameObject.name;
				lvInventory.Add (this.gameObject.name, lvItem);
				lvStatus.InventorySlotId = this.gameObject.name;
			}

		}
	}

	private bool isEquipementAllowed(Item pmItem)
	{
		if (EquipementType == EquipementTypes.ANY)
			return true;
		else if (pmItem.EquipementTypes.Contains (EquipementType))
			return true;
		else
			return false;


	}

}
