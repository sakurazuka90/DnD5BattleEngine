using UnityEngine;
using System.Collections;

public class AbilityPanel : MonoBehaviour {

	public GameObject [] AbilitiesDisplayers;

	public void FillAbilities(Player pmPlayer)
	{

		AbilitiesDisplayers [0].GetComponent<AbilityDisplayerController> ().FillAbilityFields ("STR", pmPlayer.GetAbilityValue (AbilityNames.STRENGTH), pmPlayer.GetAbilityModifier (AbilityNames.STRENGTH));
		AbilitiesDisplayers [1].GetComponent<AbilityDisplayerController> ().FillAbilityFields ("DEX", pmPlayer.GetAbilityValue (AbilityNames.DEXTERITY), pmPlayer.GetAbilityModifier (AbilityNames.DEXTERITY));
		AbilitiesDisplayers [2].GetComponent<AbilityDisplayerController> ().FillAbilityFields ("CON", pmPlayer.GetAbilityValue (AbilityNames.CONSTITUTION), pmPlayer.GetAbilityModifier (AbilityNames.CONSTITUTION));
		AbilitiesDisplayers [3].GetComponent<AbilityDisplayerController> ().FillAbilityFields ("INT", pmPlayer.GetAbilityValue (AbilityNames.INTELLIGENCE), pmPlayer.GetAbilityModifier (AbilityNames.INTELLIGENCE));
		AbilitiesDisplayers [4].GetComponent<AbilityDisplayerController> ().FillAbilityFields ("WIS", pmPlayer.GetAbilityValue (AbilityNames.WISDOM), pmPlayer.GetAbilityModifier (AbilityNames.WISDOM));
		AbilitiesDisplayers [5].GetComponent<AbilityDisplayerController> ().FillAbilityFields ("CHA", pmPlayer.GetAbilityValue (AbilityNames.CHARISMA), pmPlayer.GetAbilityModifier (AbilityNames.CHARISMA));

	}
}
