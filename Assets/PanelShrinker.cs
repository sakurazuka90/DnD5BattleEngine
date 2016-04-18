using UnityEngine;
using System.Collections.Generic;

public class PanelShrinker : MonoBehaviour {

	List<GameObject> _childrenObjects;
	private bool isShrank = false;

	// Use this for initialization
	void Start () {
		_childrenObjects = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ToggleShrink()
	{
		_childrenObjects = GameObjectUtils.GetAllChildrenList (this.gameObject);
		foreach (GameObject lvItem in _childrenObjects)
			lvItem.SetActive(isShrank);

		isShrank = !isShrank;


	}
}
