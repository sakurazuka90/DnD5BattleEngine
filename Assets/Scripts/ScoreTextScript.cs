using UnityEngine;
using System.Collections;

public class ScoreTextScript : MonoBehaviour {
	
	public float fadeTime=1.0f;
	float startTime=0;
	
	void Start () {
		startTime=Time.time;

		MessagePool lvGlobalPool = GameObject.Find ("GlobalMessagePool").GetComponent<MessagePool> ();

		string lvMessage = lvGlobalPool.message;
		this.gameObject.GetComponent<TextMesh> ().text = lvMessage;
		lvGlobalPool.message = "";

		transform.eulerAngles = new Vector3 (68.0f, 0.0f, 0.0f);
	}
	
	void Update () {

		transform.Translate(0,Time.deltaTime*1.0f,0);
		
		float newAlpha=1.0f-(Time.time-startTime)/fadeTime;
		GetComponent<TextMesh>().color=new Color(1,1,1,newAlpha);
		
		if (newAlpha<=0)
		{
			Destroy(gameObject);
		}
	}
}
