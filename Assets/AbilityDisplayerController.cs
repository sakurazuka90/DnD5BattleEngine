using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityDisplayerController : MonoBehaviour {

	public GameObject abilityShortcut;
	public GameObject abilityValue;
	public GameObject abilityModifier;

	public void FillAbilityFields(string pmShortcut, int pmValue, int pmModifier)
	{
		abilityValue.GetComponent<Text> ().text = pmValue.ToString ();
		abilityShortcut.GetComponent<Text> ().text = pmShortcut;
		abilityModifier.GetComponent<Text> ().text = pmModifier.ToString();
	}
}
