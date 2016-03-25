using UnityEngine;
using System.Collections;

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
		if(isGameplay){
			BattlefieldStateReader.instance.ParseBattlefieldFile ();
			GenerateGridFromFile ();
		}
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
	

}
