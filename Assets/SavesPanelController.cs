using UnityEngine;
using System.Collections;

public class SavesPanelController : MonoBehaviour {

	public GameObject[] savePanels;

	public void FillAbilities(Player pmPlayer)
	{

		savePanels [0].GetComponent<AbilityDisplayerController> ().FillAbilityFields ("STR", pmPlayer.GetAbilityValue (AbilityNames.STRENGTH), pmPlayer.GetAbilityModifier (AbilityNames.STRENGTH));
		savePanels [1].GetComponent<AbilityDisplayerController> ().FillAbilityFields ("DEX", pmPlayer.GetAbilityValue (AbilityNames.DEXTERITY), pmPlayer.GetAbilityModifier (AbilityNames.DEXTERITY));
		savePanels [2].GetComponent<AbilityDisplayerController> ().FillAbilityFields ("CON", pmPlayer.GetAbilityValue (AbilityNames.CONSTITUTION), pmPlayer.GetAbilityModifier (AbilityNames.CONSTITUTION));
		savePanels [3].GetComponent<AbilityDisplayerController> ().FillAbilityFields ("INT", pmPlayer.GetAbilityValue (AbilityNames.INTELLIGENCE), pmPlayer.GetAbilityModifier (AbilityNames.INTELLIGENCE));
		savePanels [4].GetComponent<AbilityDisplayerController> ().FillAbilityFields ("WIS", pmPlayer.GetAbilityValue (AbilityNames.WISDOM), pmPlayer.GetAbilityModifier (AbilityNames.WISDOM));
		savePanels [5].GetComponent<AbilityDisplayerController> ().FillAbilityFields ("CHA", pmPlayer.GetAbilityValue (AbilityNames.CHARISMA), pmPlayer.GetAbilityModifier (AbilityNames.CHARISMA));

	}
}
