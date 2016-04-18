using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectUtils : MonoBehaviour {

	public static void RemoveAllChildren(GameObject pmParent)
	{
		List<GameObject> lvChildren = GetAllChildrenList (pmParent);
		lvChildren.ForEach (child => Destroy(child));
	}

	public static List<GameObject> GetAllChildrenList(GameObject pmParent)
	{
		List<GameObject> lvChildren = new List<GameObject> ();
		foreach (Transform lvCell in pmParent.transform)
			lvChildren.Add (lvCell.gameObject);

		return lvChildren;
	}
}
