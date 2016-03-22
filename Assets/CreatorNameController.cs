using UnityEngine;
using System.Collections.Generic;

public class CreatorNameController : MonoBehaviour {

	int itemCounter;
	List<string> nameList;

	// Use this for initialization
	void Start () {
		itemCounter = 0;
		nameList = new List<string> ();
	}
	
	public string CreateUniqueName (string pmGeneratedName){

		string lvResultName = pmGeneratedName;

		if (nameList.Contains (pmGeneratedName)) {
			lvResultName += itemCounter;
			itemCounter++;
		}

		nameList.Add (lvResultName);

		return lvResultName;
	}
}
