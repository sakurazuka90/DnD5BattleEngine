using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelShrinker : MonoBehaviour {

	List<GameObject> _childrenObjects;
	private bool isShrank = false;
	private float _savedHeight; 
	private RectTransform _transform;

	// Use this for initialization
	void Start () {
		_childrenObjects = new List<GameObject> ();
		_transform = this.gameObject.GetComponent<RectTransform> ();
		_savedHeight = _transform.sizeDelta.y;
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

		float lvHeightTemp;

		if (_transform.sizeDelta.y > 0.0f) {
			lvHeightTemp = 0.0f;// - _transform.sizeDelta.y;
		} else {
			lvHeightTemp = /*_transform.sizeDelta.y - */_savedHeight;
		}

		_transform.sizeDelta = new Vector2 (_transform.sizeDelta.x,lvHeightTemp);
	}
}
