using UnityEngine;
using System.Collections;

public class ObstacleRotator : MonoBehaviour {

	public void Rotate(float pmAngle)
	{
		foreach (Transform lvChild in this.transform) {
			if (lvChild.CompareTag ("ObstacleRoot"))
				lvChild.transform.Rotate (new Vector3 (0.0f, pmAngle, 0.0f));
		}
			
	}
}
