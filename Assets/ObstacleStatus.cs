using UnityEngine;
using System.Collections.Generic;

public class ObstacleStatus : MonoBehaviour {

	private List<int> cellMods;

	public string name;
	public bool isDifficultTerrain;
	public bool isBlockingMovement;
	public bool isBlockingLoS;
}
