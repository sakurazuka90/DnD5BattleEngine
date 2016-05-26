using UnityEngine;
using System.Collections.Generic;

public class AIEngine : MonoBehaviour {

	public bool free = true;

	public Player currentPlayer;

	private Queue<Gambit> gambitQueue;

	public static AIEngine instance;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (currentPlayer != null && currentPlayer.isAi && free) {
			Process ();
		}

	}

	private void Process()
	{
		bool foundGambit = false;

		while (gambitQueue.Count > 0) {
			Gambit gambit = gambitQueue.Dequeue ();

			if (gambit.Evaluate ()) {
				gambit.Process ();
				foundGambit = true;
				break;
			}
		}

		if (foundGambit)
			gambitQueue = GenerateGambitQueue ();
		else
			PlayerSpooler.instance.spool ();


	}

	public void InitEngine(Player pmCurrentPlayer)
	{
		currentPlayer = pmCurrentPlayer;

		gambitQueue = GenerateGambitQueue ();
	}

	private Queue<Gambit> GenerateGambitQueue(){
		Queue<Gambit> queue = new Queue<Gambit> ();

		List<Gambit> list = currentPlayer.GambitList;

		foreach (Gambit lvGambit in list) {
			queue.Enqueue (lvGambit);
		}

		return queue;
	}
}
