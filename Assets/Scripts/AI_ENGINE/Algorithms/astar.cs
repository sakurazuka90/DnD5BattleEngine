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
		GetRouteAstar (new bool[GridDrawer.instance.mCells.Length], id, targetId);

		int current = targetId;

		while (current != id) {
			GridDrawer.instance.mCells [current].GetComponent<CellStatus> ().spawnEnemy = true;
			current = tab [current];
		}
	}

	public int [] GetRouteAstar(bool [] pmGraph, int pmStartId, int pmTargetId)
	{
		Queue<int> frontier = new Queue<int>();
		frontier.Enqueue (pmStartId);

		Dictionary<int,int> cameFrom = new Dictionary<int,int>(); 


		while (frontier.Count > 0) {
			int current = frontier.Dequeue ();

			if (current == pmTargetId)
				break;

			List<int> neighbours = SelectFromGrid.instance.GetAdjacentFields (current);

			foreach (int next in neighbours) {
				if (!cameFrom.ContainsKey(next)) {
					frontier.Enqueue (next);
					cameFrom [next] = current;
				}
			}

		}

		tab = cameFrom;

		return new int[0];

	}


}
