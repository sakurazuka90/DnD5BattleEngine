using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectOptionController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<Button> ().onClick.AddListener (this.Select);
	}
	
	private void Select()
	{
		this.gameObject.GetComponent<Image>().color = new Color (103.0F / 255.0F, 103.0F / 255.0F, 103.0F / 255.0F);

		this.gameObject.transform.FindChild("Text").GetComponent<Text>().color = new Color (193.0F / 255.0F, 193.0F / 255.0F, 193.0F / 255.0F);
	}
}
