using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillPanel : MonoBehaviour {

	public GameObject skillNameText;
	public GameObject abilityShortcutText;
	public GameObject checkbox;

	public void FillSkillPanel(Skill skill)
	{
		skillNameText.GetComponent<Text> ().text = skill.Name;
		abilityShortcutText.GetComponent<Text> ().text = Dictionaries.abilityShortcuts [skill.Ability];
		checkbox.GetComponent<Toggle> ().isOn = skill.IsProficient;
	}
}
