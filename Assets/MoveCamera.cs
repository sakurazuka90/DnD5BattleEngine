using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float lvSpeedX = 0, lvSpeedZ = 0;

		if (Input.GetKey (KeyCode.D))
			lvSpeedX += speed;
		if (Input.GetKey (KeyCode.A))
			lvSpeedX -= speed;

		gameObject.transform.Translate (lvSpeedX, 0.0f, lvSpeedZ);
	
	}
}
