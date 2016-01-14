using UnityEngine;
using System.Collections;

public class Hider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		GameObject lvCanvas = GameObject.Find ("Canvas");
		
		GameObject lvDropdown = lvCanvas.transform.GetChild(0).gameObject;

		if (Input.GetMouseButton(0) && lvDropdown.activeSelf && 
		    !RectTransformUtility.RectangleContainsScreenPoint(
			lvDropdown.GetComponent<RectTransform>(), 
			Input.mousePosition, 
			Camera.main)) {
			lvDropdown.SetActive(false);
		}

	}
}
