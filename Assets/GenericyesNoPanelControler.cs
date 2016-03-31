using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class GenericyesNoPanelControler : MonoBehaviour {

	public GameObject yesButton;
	public GameObject noButton;
	public GameObject textBox;

	void InitializePanel(string pmMessage, UnityAction pmAction)
	{
		yesButton.GetComponent<Button> ().onClick.AddListener (pmAction);

	}

	public void Hide()
	{
		
	}
}
