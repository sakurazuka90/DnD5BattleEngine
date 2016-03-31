using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class GenericyesNoPanelControler : MonoBehaviour {

	public GameObject yesButton;
	public GameObject noButton;
	public GameObject textBox;

	public void InitializePanel(string pmMessage, UnityAction pmAction)
	{
		//this.gameObject.SetActive (true);
		yesButton.GetComponent<Button> ().onClick.AddListener (pmAction);
		yesButton.GetComponent<Button> ().onClick.AddListener (this.Hide);

		textBox.GetComponent<Text> ().text = pmMessage;

	}

	public void Hide()
	{
		//this.gameObject.SetActive (false);
		Destroy(this.gameObject);
	}
}
