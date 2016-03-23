using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BattleFieldSaver : MonoBehaviour {

	public void Save()
	{
		BinaryFormatter lvFormater = new BinaryFormatter ();
		FileStream lvFile = File.Open (Application.persistentDataPath + "/testsave.dat", FileMode.OpenOrCreate);

		GridData lvData = new GridData ();
		lvData.x = GridDrawer.instance.gridWidth;
		lvData.z = GridDrawer.instance.gridHeight;

		lvFormater.Serialize (lvFile, lvData);
		lvFile.Close ();
	}


}

[Serializable]
class GridData{
	public int x;
	public int z;
}

