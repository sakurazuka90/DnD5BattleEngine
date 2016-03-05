using UnityEngine;
using System.Collections;

public class AttackInstantiator : MonoBehaviour {

	public bool shoot = false;
	public GameObject projectile;
	public float speed;
	public Player lvAttacker;
	public Player lvTarget;
	public bool isAdvantage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (shoot) {
			GameObject lvAttack = Instantiate (projectile);
			lvAttack.transform.position = lvAttacker.Figurine.transform.position;
			MissleStats lvStats = lvAttack.GetComponent<MissleStats> ();
			lvStats.lvAttacker = lvAttacker;
			lvStats.lvTarget = lvTarget;
			lvStats.isAdvantage = isAdvantage;
			lvStats.speed = speed;
			shoot = false;
		}
	}
}
