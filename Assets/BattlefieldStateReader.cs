﻿using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BattlefieldStateReader : MonoBehaviour
{

	private int _gridWidth = 0;
	private int _gridHeight = 0;

	public int GridWidth{
		get{return _gridWidth;}
	}

	public int GridHeight{
		get{return _gridHeight;}
	}

	public static BattlefieldStateReader instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this);
	}

	// Use this for initialization
	void Start ()
	{
	
	}

	public void ParseBattlefieldFile ()
	{
		if (File.Exists (Application.persistentDataPath + "/testsave.dat")) {
			BinaryFormatter lvFormater = new BinaryFormatter ();
			FileStream lvFile = File.Open (Application.persistentDataPath + "/testsave.dat", FileMode.Open);
			GridData lvData = (GridData)lvFormater.Deserialize (lvFile);
			_gridWidth = lvData.x;
			_gridHeight = lvData.z;

		}

	}


		
}