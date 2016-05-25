using UnityEngine;
using System.Collections.Generic;



public class astar : MonoBehaviour {

	public static astar instance;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
	}

	public int id = 0;
	public int targetId = 10;

	private Dictionary<int,int> tab;

	// Use this for initialization
	void Start () {
		tab = new Dictionary<int,int> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void test()
	{
		GridDrawer.instance.ClearGridStatus ();

		GetRouteAstar (id, targetId);

		int current = targetId;

		while (current != id) {
			GridDrawer.instance.mCells [current].GetComponent<CellStatus> ().spawnEnemy = true;
			current = tab [current];
		}
	}

	public Dictionary<int,int> GetRouteAstar(int pmStartId, int pmTargetId)
	{
		//Queue<int> frontier = new Queue<int>();
		PriorityQueue<int,int> frontier = new PriorityQueue<int, int>();
		frontier.Enqueue (pmStartId, 0);

		Dictionary<int,int> cameFrom = new Dictionary<int, int>(); 
		Dictionary<int,int> costSoFar = new Dictionary<int, int> ();

		costSoFar.Add (pmStartId, 0);


		while (!frontier.Empty()) {
			int current = frontier.Dequeue ();

			if (current == pmTargetId)
				break;

			List<int> neighbours = SelectFromGrid.instance.GetAdjacentNonBlockedFields (current);

			foreach (int next in neighbours) {

				int newCost = costSoFar [current] + this.GetCellMoveCost (next);

				if (!costSoFar.ContainsKey (next) || newCost < costSoFar [next]) {
					costSoFar [next] = newCost + Length (next, pmTargetId);
					int lvPriority = newCost;

					frontier.Enqueue (next, newCost);

					cameFrom [next] = current;

				}

			}

		}

		Debug.Log ("Reached in " + cameFrom.Count + " moves");

		tab = cameFrom;

		return cameFrom;

	}

	public string GetAstarAsMoverSteps(int pmStartId, int pmTargetId){

		Dictionary<int,int> lvDict = GetRouteAstar (pmStartId, pmTargetId);

		int current = pmTargetId;

		List<int> pmSteps = new List<int> ();

		while (current != pmStartId) {
			pmSteps.Add (current);
			current = tab [current];
		}

		pmSteps.Add (pmStartId);

		pmSteps.Reverse ();

		string lvSteps = "";

		for (int i = 0; i < pmSteps.Count; i++) {
			lvSteps += pmSteps [i];

			if (i != (pmSteps.Count - 1)) {
				lvSteps += "_";
			}
		}

		return lvSteps;
	}

	private int GetCellMoveCost(int pmCellId)
	{
		if (GridDrawer.instance.IsCellDifficultTerrain (pmCellId))
			return 2;
		else
			return 1;
	}

	private int Length(int pmStartId, int pmEndId)
	{
		return Mathf.Abs (GridDrawer.instance.getGridX (pmEndId) - GridDrawer.instance.getGridX (pmStartId)) + Mathf.Abs (GridDrawer.instance.getGridZ (pmEndId) - GridDrawer.instance.getGridZ (pmStartId));
	}


}
