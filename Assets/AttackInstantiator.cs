using UnityEngine;
using System.Collections;

public class AttackInstantiator : MonoBehaviour {

	public bool shoot = false;
	public GameObject attack;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (shoot) {
			Instantiate (attack);
			shoot = false;
		}
	}
}
