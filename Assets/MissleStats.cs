using UnityEngine;
using System.Collections;

public class MissleStats : MonoBehaviour {

	public Vector3 targetLocation;
	public float speed;

	// Use this for initialization
	void Start () {
		transform.forward = Vector3.Normalize (targetLocation);
	}
	
	// Update is called once per frame
	void Update () {
		float lvStep = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, targetLocation, lvStep);
	}

	void OnTriggerEnter(Collider other) {
		//Destroy(this.gameObject);
	}
}
