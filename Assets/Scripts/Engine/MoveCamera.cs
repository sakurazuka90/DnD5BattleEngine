using UnityEngine;
using System.Collections;

/*
 * Simple script for camera movement
 */
public class MoveCamera : MonoBehaviour {

	public float speed;
	public bool isMovable = false;

	void Update () {

		if (isMovable) {
			float lvSpeedX = 0, lvSpeedZ = 0;

			if (Input.GetKey (KeyCode.D))
				lvSpeedX += speed;
			if (Input.GetKey (KeyCode.A))
				lvSpeedX -= speed;
			if (Input.GetKey (KeyCode.W))
				lvSpeedZ += speed;
			if (Input.GetKey (KeyCode.S))
				lvSpeedZ -= speed;

			gameObject.transform.Translate (lvSpeedX, 0.0f, lvSpeedZ);
		}
	}
}
