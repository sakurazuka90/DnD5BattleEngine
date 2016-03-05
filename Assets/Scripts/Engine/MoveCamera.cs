using UnityEngine;
using System.Collections;

/*
 * Simple script for camera movement
 */
public class MoveCamera : MonoBehaviour {

	public float speed;
	public bool isMovable = false;

	public float maxX;
	public float maxZ;

	void Update () {

		if (isMovable) {
			float lvSpeedX = 0, lvSpeedZ = 0;

			float lvSpeed = speed;

			if (Input.GetKey (KeyCode.LeftShift))
				lvSpeed = lvSpeed * 2.0f;

			if (Input.GetKey (KeyCode.D))
				lvSpeedX += lvSpeed;
			if (Input.GetKey (KeyCode.A))
				lvSpeedX -= lvSpeed;
			if (Input.GetKey (KeyCode.W))
				lvSpeedZ += lvSpeed;
			if (Input.GetKey (KeyCode.S))
				lvSpeedZ -= lvSpeed;

			if (transform.position.x + lvSpeedX < 0)
				lvSpeedX -= (transform.position.x + lvSpeedX);
			if (transform.position.z + lvSpeedZ < 0)
				lvSpeedZ -= (transform.position.z + lvSpeedZ);

			if (transform.position.x + lvSpeedX > maxX)
				lvSpeedX -= (transform.position.x + lvSpeedX - maxX);
			if (transform.position.z + lvSpeedZ > maxZ)
				lvSpeedZ -= (transform.position.z + lvSpeedZ - maxZ);

			gameObject.transform.Translate (lvSpeedX, 0.0f, lvSpeedZ);
		}
	}
}
