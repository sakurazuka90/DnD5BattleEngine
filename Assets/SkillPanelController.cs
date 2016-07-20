using UnityEngine;
using System.Collections;

public class SkillPanelController : MonoBehaviour {

	public GameObject content;
	public GameObject panelPrefab;

	public void FillSkills(Player pmPlayer)
	{
		GameObjectUtils.RemoveAllChildren (content);

		foreach (Skill skill in pmPlayer.Skills) {
			GameObject instance = Instantiate (panelPrefab);
			instance.GetComponent<SkillPanel> ().FillSkillPanel (skill);
			instance.transform.SetParent(content.transform);
		}
	}

}
