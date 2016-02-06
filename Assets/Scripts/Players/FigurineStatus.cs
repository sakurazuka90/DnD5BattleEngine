using UnityEngine;
using System.Collections;

public class FigurineStatus : MonoBehaviour {

	public bool picked = false;
	public bool enemy = false;
	public bool active = false;


	public int gridX;
	public int gridZ;

	public int movesLeft;

	public void pick(int pmMoves)
	{
		//picked = true;
		GameObject lvSelector = GameObject.Find ("GridSelector");
		SelectFromGrid lvSelect = lvSelector.GetComponent<SelectFromGrid>();
		lvSelect.ShowWalkableDistance (gridX, gridZ, pmMoves);

	}


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
