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

		GetRouteAstar (id, new List<int>(targetId), targetId);

		int current = targetId;

		while (current != id) {
			GridDrawer.instance.mCells [current].GetComponent<CellStatus> ().spawnEnemy = true;
			current = tab [current];
		}
	}

	public AstarResponse GetRouteAstar(int pmStartId, List<int> pmTargets, int heuresticPoint)
	{
		//Queue<int> frontier = new Queue<int>();
		PriorityQueue<int,int> frontier = new PriorityQueue<int, int>();
		frontier.Enqueue (pmStartId, 0);

		Dictionary<int,int> cameFrom = new Dictionary<int, int>(); 
		Dictionary<int,int> costSoFar = new Dictionary<int, int> ();

		costSoFar.Add (pmStartId, 0);

		int foundTarget = 0;


		while (!frontier.Empty()) {
			int current = frontier.Dequeue ();

			if (pmTargets.Contains (current)) {
				foundTarget = current;
				break;
			}

			List<int> neighbours = SelectFromGrid.instance.GetAdjacentNonBlockedFieldsWithDiagonalCheck (current);

			foreach (int next in neighbours) {

				int newCost = costSoFar [current] + this.GetCellMoveCost (next);

				if (!costSoFar.ContainsKey (next) || newCost < costSoFar [next]) {
					costSoFar [next] = newCost + Length (next, heuresticPoint);
					int lvPriority = newCost;

					frontier.Enqueue (next, newCost);

					cameFrom [next] = current;

				}

			}

		}

		Debug.Log ("Reached in " + cameFrom.Count + " moves");

		tab = cameFrom;

		return new AstarResponse (cameFrom, pmStartId, foundTarget);

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
