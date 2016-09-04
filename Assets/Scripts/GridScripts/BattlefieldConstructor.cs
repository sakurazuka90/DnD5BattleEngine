using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattlefieldConstructor : MonoBehaviour {

	public static BattlefieldConstructor instance;

	public bool isGameplay = false;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this);
	}

	// Use this for initialization
	void Start () {
	}

	public bool GenerateGameplay(string pmFilename)
	{
		bool finished = true;
		BattlefieldStateReader.instance.ParseBattlefieldFile (pmFilename);
		GenerateGridFromFile ();
		SetupCameraMover ((float)BattlefieldStateReader.instance.GridWidth, (float)BattlefieldStateReader.instance.GridHeight);
		CreateFloor (BattlefieldStateReader.instance.GridWidth, BattlefieldStateReader.instance.GridHeight, BattlefieldStateReader.instance.GraphicsStyle);
		CreateWalls (BattlefieldStateReader.instance.GridWidth, BattlefieldStateReader.instance.GridHeight);
		SetupObstacles(BattlefieldStateReader.instance.Obstacles);

		int [] players = BattlefieldStateReader.instance.Players;
		int [] enemies = BattlefieldStateReader.instance.Enemies;
		int dynamicPlayers = 0;
		int dynamicEnemies = 0;

		Queue<int> enemiesQueue = new Queue<int> ();
		Queue<int> playersQueue = new Queue<int> ();

		for (int i = 0; i < players.Length; i++) {

			int j = players [i];

			if (j < 0) {
				dynamicPlayers++;
				playersQueue.Enqueue (i);
			}
		}

		for (int i = 0; i < enemies.Length; i++) {

			int j = enemies [i];

			if (j < 0) {
				dynamicEnemies++;
				enemiesQueue.Enqueue (i);
			}
		}

		if (dynamicEnemies > 0 || dynamicPlayers > 0) {
			finished = false;

			UiItemLibrary.instance.selectFigurinePanel.GetComponent<SelectFigurineController> ().enemies = enemiesQueue;
			UiItemLibrary.instance.selectFigurinePanel.GetComponent<SelectFigurineController> ().players = playersQueue;

			UiItemLibrary.instance.selectFigurinePanel.GetComponent<SelectFigurineController> ();

			UiItemLibrary.instance.selectFigurinePanel.SetActive (true);
		}

		return finished;
	}

	public void GenerateGrid(int pmGridWidth, int pmGridHeight)
	{
		GridDrawer.instance.Create (pmGridWidth, pmGridHeight);
	}

	public void GenerateGridFromFile()
	{
		GenerateGrid (BattlefieldStateReader.instance.GridWidth, BattlefieldStateReader.instance.GridHeight);
	}

	public void SetupCameraMover(float pmX, float pmY)
	{
		MoveCamera.instance.isMovable = true;
		MoveCamera.instance.maxX = pmX;
		MoveCamera.instance.maxZ = pmY;
	}

	public void CreateFloor(int pmGridWidth, int pmGridHeight, int pmGraphicsStyle)
	{
		FloorCreator.instance.CreateFloor (pmGridWidth,pmGridHeight, pmGraphicsStyle);
	}

	public void CreateWalls(int pmGridWidth, int pmGridHeight)
	{
		FloorCreator.instance.CreateWalls (pmGridWidth, pmGridHeight);
	}

	public void SetupObstacles(ObstacleData [] pmObstacleData)
	{
		GameObject[] lvCells = GridDrawer.instance.mCells;

		for (int i = 0; i < pmObstacleData.Length; i++) {
			if (pmObstacleData [i] != null) {
				ObstacleData lvData = pmObstacleData [i];
				GameObject lvPrefab = Resources.Load<GameObject> ("ObstaclePrefabs/"+lvData.obstaclePrefabName);

				GameObject lvInstance = Instantiate (lvPrefab);
				lvInstance.transform.parent = lvCells [i].transform;

				FigurineMover lvMover = lvInstance.GetComponent<FigurineMover> ();
				lvMover.gridX = GridDrawer.instance.getGridX (i);
				lvMover.gridZ = GridDrawer.instance.getGridZ (i);

				ObstacleStatus lvStatus = lvInstance.GetComponent<ObstacleStatus> ();
				lvStatus.isBlockingLoS = lvData.isBlockingLineOfSight;
				lvStatus.isDifficultTerrain = lvData.isDifficultTerrain;
				lvStatus.isBlockingMovement = lvData.isBlockingMovement;
				lvStatus.coverValue = lvData.providedCover;


				lvInstance.GetComponent<ObstacleRotator> ().Rotate (lvData.rotation);

				//lvInstance.transform.eulerAngles = new Vector3 (0.0f, lvData.rotation, 0.0f);

			}
		}
	}

	public void SetupFigurines()
	{
		
	}
	

}
