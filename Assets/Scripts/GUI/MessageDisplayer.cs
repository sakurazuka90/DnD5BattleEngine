using UnityEngine;
using System.Collections.Generic;


public class MessageDisplayer : MonoBehaviour {

	public GameObject displayedText;

	private double timer = 0;
	public bool isTiming = false;
	private double cooldown = 0;

	private Queue<string> _mQueue = new Queue<string>();

	public string GetMessage()
	{
		if (_mQueue.Count > 0) {
			if (_mQueue.Count == 1) {
				isTiming = false;
				timer = 0;
				cooldown = 0;
			}

			return _mQueue.Dequeue ();
		}
		else
			return "";
	}

	public void SetMessage(string pmMessage)
	{
		isTiming = true;

		_mQueue.Enqueue (pmMessage);
	}


	// Update is called once per frame
	void Update () {
			
		if(isTiming)
			timer += Time.deltaTime;

		if (timer >= cooldown) {
			GameObject lvInstance = (GameObject)Instantiate (displayedText, this.gameObject.transform.position, Quaternion.Euler (0, 0, 0));

			lvInstance.GetComponent<ScoreTextScript> ().text = GetMessage ();

			if (cooldown == 0)
				cooldown = 0.5;

			timer = 0;
		}

	}
}
