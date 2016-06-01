using UnityEngine;
using System.Collections.Generic;

public class FigurinePositionController : MonoBehaviour {

	public bool isInitialized = false;

	Queue<FigurineMover> queue = new Queue<FigurineMover>();

	public static FigurinePositionController instance;

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

	public void AddFigurineMover(FigurineMover pmMover)
	{
		queue.Enqueue (pmMover);
	}
	
	// Update is called once per frame
	void Update () {
		if (isInitialized) {
			Queue<FigurineMover> lvNotReady = new Queue<FigurineMover> ();

			while (queue.Count > 0) {
				FigurineMover tested = queue.Dequeue ();

				if (!tested.IsOnSelectedField ())
					lvNotReady.Enqueue (tested);
			}

			if (lvNotReady.Count == 0) {
				AIEngine.instance.InitEngine (PlayerSpooler.instance.GetSpooledPlayer ());
				Destroy (this.gameObject);
			} else {
				queue = lvNotReady;
			}
		}
	}


}
