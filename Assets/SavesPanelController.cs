using UnityEngine;
using System.Collections.Generic;

public class SavesPanelController : MonoBehaviour {

	public GameObject[] savePanels;

	public void FillSaves(Player pmPlayer)
	{
		List<int> values = pmPlayer.Saves;
		ClearSaves ();

		foreach (int i in values) {
			savePanels [i - 1].GetComponent<SavePanel> ().SetCheckboxValue (true);
		}

	}

	public void ClearSaves()
	{
		foreach(GameObject box in savePanels)
		{
			box.GetComponent<SavePanel> ().SetCheckboxValue (false);
		}
	}
}
