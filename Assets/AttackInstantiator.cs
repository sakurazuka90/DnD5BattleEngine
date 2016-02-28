using UnityEngine;
using System.Collections;

public class AttackInstantiator : MonoBehaviour {

	public bool shoot = false;
	public GameObject attack;
	public Vector3 targetPosition;
	public Vector3 playerPosition;
	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (shoot) {
			GameObject lvAttack = Instantiate (attack);
			lvAttack.transform.position = playerPosition;
			MissleStats lvStats = lvAttack.GetComponent<MissleStats> ();
			lvStats.targetLocation = targetPosition;
			lvStats.speed = speed;
			shoot = false;
		}
	}
}
