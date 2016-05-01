using UnityEngine;
using System.Collections.Generic;

public class FigurineFileReader : MonoBehaviour {

	private static string FIGURINE_LIBRARY_NAME = "figurineLibrary.flib";

	public static FigurineFileReader instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	public List<string> GetFigurineList()
	{
		List<string> lvNames = new List<string> ();

		lvNames.Add ("DYNAMIC SPAWN POINT");

		lvNames.Add ("Goblin");
		lvNames.Add ("Dwarf");

		return lvNames;
	}
}
