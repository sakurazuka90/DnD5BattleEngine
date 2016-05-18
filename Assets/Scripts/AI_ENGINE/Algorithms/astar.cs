using UnityEngine;
using System.Collections.Generic;

public class astar : MonoBehaviour {

	public int id = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void test()
	{
		GetRouteAstar (new bool[GridDrawer.instance.mCells.Length], id);
	}

	public int [] GetRouteAstar(bool [] pmGraph, int pmStartId)
	{
		Queue<int> frontier = new Queue<int>();
		frontier.Enqueue (pmStartId);

		bool[] visited = new bool[pmGraph.Length];


		while (frontier.Count > 0) {
			int current = frontier.Dequeue ();

			List<int> neighbours = SelectFromGrid.instance.GetAdjacentFields (current);

			foreach (int next in neighbours) {
				if (!visited [next]) {
					frontier.Enqueue (next);
					visited [next] = true;
					/////////////////////////////////////////////

					GridDrawer.instance.mCells [next].GetComponent<CellStatus> ().avaiable = false;

					////////////////////////////////////////////

				}
			}

		}

		return new int[0];

	}


}
