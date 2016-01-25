using UnityEngine;
using System.Collections;

public class MessageDisplayer : MonoBehaviour {

	MessagePool mGlobalPool;

	public string message = "";
	public GameObject displayedText;

	// Use this for initialization
	void Start () {
		mGlobalPool = GameObject.Find ("GlobalMessagePool").GetComponent<MessagePool> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (message.Length > 0) {
			mGlobalPool.message = message;
			message = "";
			Instantiate (displayedText, this.gameObject.transform.position, Quaternion.Euler (0, 0, 0));
		}
	}
}
