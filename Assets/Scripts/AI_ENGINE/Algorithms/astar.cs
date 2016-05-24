using UnityEngine;
using System.Collections.Generic;



public class astar : MonoBehaviour {

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

		GetRouteAstar (new bool[GridDrawer.instance.mCells.Length], id, targetId);

		int current = targetId;

		while (current != id) {
			GridDrawer.instance.mCells [current].GetComponent<CellStatus> ().spawnEnemy = true;
			current = tab [current];
		}
	}

	public int [] GetRouteAstar(bool [] pmGraph, int pmStartId, int pmTargetId)
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

		return new int[0];

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
