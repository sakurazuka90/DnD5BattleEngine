using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BattlefieldStateReader : MonoBehaviour
{

	private int _gridWidth = 0;
	private int _gridHeight = 0;
	private int _graphicsStyle = 0;
	private ObstacleData[] _obstacleData;
	private int[] _players;
	private int[] _enemies;

	public int GridWidth{
		get{return _gridWidth;}
	}

	public int GridHeight{
		get{return _gridHeight;}
	}

	public int GraphicsStyle{
		get{ return _graphicsStyle; }
	}

	public ObstacleData[] Obstacles{
		get{return _obstacleData; }
	}

	public int [] Players{
		get{return _players; }
	}

	public int [] Enemies{
		get{return _enemies;}
	}

	public static BattlefieldStateReader instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
	}

	// Use this for initialization
	void Start ()
	{
		//ListFiles();
	}

	public void ParseBattlefieldFile (string pmFilename)
	{
		if (File.Exists (Application.persistentDataPath + "/" + pmFilename)) {
			BinaryFormatter lvFormater = new BinaryFormatter ();
			FileStream lvFile = File.Open (Application.persistentDataPath + "/" + pmFilename, FileMode.Open);
			GridData lvData = (GridData)lvFormater.Deserialize (lvFile);
			_gridWidth = lvData.x;
			_gridHeight = lvData.z;
			_obstacleData = lvData.obstacles;
			_players = lvData.players;
			_enemies = lvData.enemies;
			_graphicsStyle = lvData.graphicsStyle;
		}

	}

	public List<string> ListFiles()
	{
		DirectoryInfo lvInfo = new DirectoryInfo(Application.persistentDataPath);
		FileInfo[] lvFiles = lvInfo.GetFiles ();

		List<string> lvFilenames = new List<string> ();

		foreach (FileInfo file in lvFiles) 
		{
			if (".dat".Equals (file.Extension)) {
				lvFilenames.Add (file.Name);
			}
		}

		return lvFilenames;
	}


		
}
