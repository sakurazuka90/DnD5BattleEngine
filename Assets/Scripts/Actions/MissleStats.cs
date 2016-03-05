using UnityEngine;
using System.Collections;

public class MissleStats : MonoBehaviour {

	public float speed;
	public Player lvAttacker;
	public Player lvTarget;
	public bool isCritical;
	public bool isAdvantage;


	// Use this for initialization
	void Start () {
		transform.forward = Vector3.Normalize (lvTarget.Figurine.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		float lvStep = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, lvTarget.Figurine.transform.position, lvStep);
	}

	void OnTriggerEnter(Collider other) {
		if (lvTarget.Figurine.name.Equals (other.gameObject.name)) {
			lvAttacker.equippedWeaponAttack.resolveHit(lvAttacker, lvTarget,true);
			GameObject.Destroy (this.gameObject);
		}
	}
}
