using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class GenericItemButton : MonoBehaviour {

	public void InitializeButton(UnityAction pmAction)
	{
		this.GetComponent<Button> ().onClick.AddListener (pmAction);

	}
}
