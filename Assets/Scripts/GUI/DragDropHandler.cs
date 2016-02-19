using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DragDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject draggedObject;
	private Vector3 startPosition;
	private Transform startParent;

	public void OnBeginDrag (PointerEventData eventData)
	{
		draggedObject = gameObject;
		startPosition = gameObject.transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;

		GameObject lvInventoryPanelObject = GameObject.Find ("InventoryPanel");
		EquipementFieldsLighter lvLighter = lvInventoryPanelObject.GetComponent<EquipementFieldsLighter> ();
		List<EquipementTypes> lvItems =  gameObject.GetComponent<InventoryObjectStatus> ().EquipementTypes;

		foreach (EquipementTypes lvType in lvItems) {
			lvLighter.LightFields (lvType);
		}
	}

	
	void IDragHandler.OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		draggedObject = null;

		GetComponent<CanvasGroup> ().blocksRaycasts = true;

		if(transform.parent == startParent)
			transform.position = startPosition;

		GameObject lvInventoryPanelObject = GameObject.Find ("InventoryPanel");
		EquipementFieldsLighter lvLighter = lvInventoryPanelObject.GetComponent<EquipementFieldsLighter> ();
		lvLighter.FadeFields ();
	}

}
