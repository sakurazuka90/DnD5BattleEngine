using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EquipementFieldsLighter : MonoBehaviour {

	private Dictionary<EquipementTypes,List<string>> mEquipementFieldsNames;
	public Color cellColor;

	// Use this for initialization
	void Start () {
		InitializeNamesMap ();
	}
	
	public void LightFields(EquipementTypes pmType)
	{
		List<string> lvItems = mEquipementFieldsNames [pmType];

		foreach (string lvItemName in lvItems) {

			GameObject lvSlot = GameObject.Find (lvItemName);
			Image lvImage = lvSlot.GetComponent<Image> ();
			lvImage.color = cellColor;

		}
		
	}

	public void FadeFields()
	{
		foreach (EquipementTypes lvType in mEquipementFieldsNames.Keys) {
			List<string> lvItemList = mEquipementFieldsNames [lvType];
			foreach(string lvName in lvItemList)
			{
				GameObject lvSlot = GameObject.Find (lvName);
				Image lvImage = lvSlot.GetComponent<Image> ();
				lvImage.color = new Color (1.0f, 1.0f, 1.0f, 100.0f / 255.0f);
			}
		}
	}


	private void InitializeNamesMap()
	{
		mEquipementFieldsNames = new Dictionary<EquipementTypes, List<string>> ();

		List<string> lvNamesList = new List<string> ();
		lvNamesList.Add ("INV_GOOGLE_PANEL");
		mEquipementFieldsNames.Add (EquipementTypes.GLASSES, lvNamesList);

		lvNamesList = new List<string> ();
		lvNamesList.Add ("INV_HELMET_PANEL");
		mEquipementFieldsNames.Add (EquipementTypes.HELMET, lvNamesList);

		lvNamesList = new List<string> ();
		lvNamesList.Add ("INV_NECKLACE_PANEL");
		mEquipementFieldsNames.Add (EquipementTypes.NECKLACE, lvNamesList);

		lvNamesList = new List<string> ();
		lvNamesList.Add ("INV_BOOTS_PANEL");
		mEquipementFieldsNames.Add (EquipementTypes.BOOTS, lvNamesList);

		lvNamesList = new List<string> ();
		lvNamesList.Add ("INV_BRACERS_PANEL");
		mEquipementFieldsNames.Add (EquipementTypes.BRACERS, lvNamesList);

		lvNamesList = new List<string> ();
		lvNamesList.Add ("INV_GLOVES");
		mEquipementFieldsNames.Add (EquipementTypes.GLOVES, lvNamesList);

		lvNamesList = new List<string> ();
		lvNamesList.Add ("INV_RING_L_PANEL");
		lvNamesList.Add ("INV_RING_R_PANEL");
		mEquipementFieldsNames.Add (EquipementTypes.RING, lvNamesList);

		lvNamesList = new List<string> ();
		lvNamesList.Add ("INV_CLOAK_PANEL");
		mEquipementFieldsNames.Add (EquipementTypes.CLOAK, lvNamesList);

		lvNamesList = new List<string> ();
		lvNamesList.Add ("INV_BELT_PANEL");
		mEquipementFieldsNames.Add (EquipementTypes.BELT, lvNamesList);

		lvNamesList = new List<string> ();
		lvNamesList.Add ("INV_OH1");
		lvNamesList.Add ("INV_OH2");
		lvNamesList.Add ("INV_OH3");
		lvNamesList.Add ("INV_OH4");
		lvNamesList.Add ("INV_OH5");
		mEquipementFieldsNames.Add (EquipementTypes.OFF_HAND, lvNamesList);

		lvNamesList = new List<string> ();
		lvNamesList.Add ("INV_MH1");
		lvNamesList.Add ("INV_MH2");
		lvNamesList.Add ("INV_MH3");
		lvNamesList.Add ("INV_MH4");
		lvNamesList.Add ("INV_MH5");
		mEquipementFieldsNames.Add (EquipementTypes.MAIN_HAND, lvNamesList);

	}
}
