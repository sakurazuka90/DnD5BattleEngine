using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AccordionPanelAdjuster : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Adjust()
	{
		List<GameObject> lvChildren = GameObjectUtils.GetAllChildrenList (this.gameObject);

		float lvPadding = -5.0f;

		foreach (GameObject lvObject in lvChildren) {
			RectTransform lvTransform = lvObject.GetComponent<RectTransform> ();

			lvPadding -= lvTransform.sizeDelta.y / 2;

			lvTransform.anchoredPosition = new Vector2 (lvTransform.anchoredPosition.x, lvPadding);
			lvPadding -= lvTransform.sizeDelta.y / 2;

			if (lvObject.GetComponent<Button> () == null) {
				lvPadding -= 5.0f;
			}
		}

		RectTransform lvPanelTransform = this.gameObject.GetComponent<RectTransform> (); 

		lvPanelTransform.sizeDelta = new Vector2 (lvPanelTransform.sizeDelta.x, -lvPadding + 5.0f);
		lvPanelTransform.anchoredPosition = new Vector2 (lvPanelTransform.anchoredPosition.x,-(lvPanelTransform.sizeDelta.y / 2) - 5.0f);

	}
}
