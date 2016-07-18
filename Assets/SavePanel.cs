using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SavePanel : MonoBehaviour {

	public GameObject checkbox;

	public void SetCheckboxValue(bool pmValue)
	{
		checkbox.GetComponent<Toggle> ().isOn = pmValue;
	}

}
