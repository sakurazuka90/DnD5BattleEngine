using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectUtils : MonoBehaviour {

	public static void RemoveAllChildren(GameObject pmParent)
	{
		List<GameObject> lvChildren = new List<GameObject> ();
		foreach (Transform lvCell in pmParent.transform)
			lvChildren.Add (lvCell.gameObject);

		lvChildren.ForEach (child => Destroy(child));
	}
}
