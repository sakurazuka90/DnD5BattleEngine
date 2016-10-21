using System;
using UnityEngine;

public abstract class AbstractSelectFromGrid : MonoBehaviour
{
	protected CellStatus lastStatus;

	public AbstractSelectFromGrid ()
	{
	}

	void Update ()
	{
		DeselectLastStatus ();
		if (RaycastActionCondition ()) {
			GameObject gridCell = FindGridCell ();

			if(gridCell != null)
				RaycastAction (gridCell);
		}
	}

	private GameObject FindGridCell()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit[] hits = Physics.RaycastAll (ray);
		GameObject gridCell = null;

		foreach (RaycastHit hit in hits) {
			if (hit.collider.tag.Equals ("GridCell") && hit.collider.gameObject.GetComponent<CellStatus> ().avaiable) {
				gridCell = hit.collider.gameObject;
				break;
			}
		}

		return gridCell;
	}

	private void DeselectLastStatus()
	{
		if(lastStatus != null)
			lastStatus.Deselect ();
	}

	protected abstract void RaycastAction(GameObject gridCell);
	protected abstract bool RaycastActionCondition ();




}


