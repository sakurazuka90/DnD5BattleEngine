using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryFiller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnEnable()
	{
		populateInventory ();
	}



	private void populateInventory()
	{
		GameObject lvSpoolerObject = GameObject.Find ("PlayerSpooler");
		PlayerSpooler lvSpooler = lvSpoolerObject.GetComponent<PlayerSpooler> ();
		Dictionary<string, Item> lvInventory = lvSpooler.GetSpooledPlayer ().Inventory;

		GameObject [] lvSlots = GameObject.FindGameObjectsWithTag ("InventoryBackpackSlot");

		foreach (GameObject lvItemSlot in lvSlots) {
			if (lvItemSlot.transform.childCount > 0) {
				GameObject lvChild = lvItemSlot.transform.GetChild (0).gameObject;
				GameObject.Destroy (lvChild);
			}

			string lvSlotName = lvItemSlot.name;
			if (lvInventory.ContainsKey (lvSlotName)) {
				GameObject lvItem = lvInventory [lvSlotName].createInventoryObject();
				lvItem.transform.SetParent (lvItemSlot.transform);
			}

		}



	}
}
