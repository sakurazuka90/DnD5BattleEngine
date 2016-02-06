using UnityEngine;
using System.Collections;

public class AnimationPlayer : MonoBehaviour {

	public bool play = false;
	public GameObject animationPrefab;
	//GameObject _slashPrefab;

	// Use this for initialization
	void Start () {
		//_slashPrefab = Resources.Load<GameObject>("001");
	}
	
	// Update is called once per frame
	void Update () {
		if (play) {
			Instantiate(animationPrefab ,new Vector3(this.gameObject.transform.position.x,1,this.gameObject.transform.position.z), Quaternion.Euler (0, 0, 0));
			Debug.Log ("Slish - slash");
			play = false;
		}
	}
}
