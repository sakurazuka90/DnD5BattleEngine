using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridDrawer : MonoBehaviour {
	public float cellSize = 1;
	public int gridWidth = 0;
	public int gridHeight = 0;
	public float yOffset = 0.5f;
	public Material cellMaterialValid;
	public Material noMaterial;

	public GameObject[] mCells;

	public GameObject obstacleBase;
	public GameObject[] obstacles;

	void Start() {
		Create ();
	}

	public void Create()
	{
		RemoveCells ();

		mCells = new GameObject[gridHeight * gridWidth];

		for (int z = 0; z < gridHeight; z++) {
			for (int x = 0; x < gridWidth; x++) {
				mCells[z * gridWidth + x] = CreateChild(x,z);
			}
		}

		GameObject lvCreator = GameObject.Find ("FloorCreator");
		if (lvCreator != null)
			lvCreator.GetComponent<FloorCreator>().CreateFloor (gridWidth, gridHeight);
	}

	private void RemoveCells()
	{
		List<GameObject> lvChildren = new List<GameObject> ();
		foreach (Transform lvCell in transform)
			lvChildren.Add (lvCell.gameObject);

		lvChildren.ForEach (child => Destroy(child));
	}

	public void Create(int pmX, int pmZ)
	{
		gridWidth = pmX;
		gridHeight = pmZ;
		Create ();
	}

	GameObject CreateChild(int x, int z) {
		GameObject lvObject = new GameObject();
		
		lvObject.name = "Grid Cell";
		lvObject.transform.parent = transform;
		lvObject.transform.localPosition = new Vector3((float) x, 0.0f, (float) z);

		lvObject.AddComponent<MeshRenderer> ();

		//if (!(x == 3 && z == 2)) {
			lvObject.GetComponent<MeshRenderer> ().material = cellMaterialValid;
			lvObject.AddComponent<CellStatus> ().avaiable = true;
		//} else {
			/*lvObject.GetComponent<MeshRenderer> ().material = noMaterial;
			lvObject.AddComponent<CellStatus> ().avaiable = false;

			GameObject lvBase = Instantiate (obstacleBase);
			GameObject lvObstacle = Instantiate(obstacles[0]);

			lvObstacle.transform.parent = lvBase.transform;
			lvBase.transform.parent = go.transform;
			lvBase.transform.Translate(new Vector3(x + 0.5f,0,z + 0.5f));*/
		//}

		///go.AddComponent<CellStatus> ().avaiable = true;

		Mesh lvMesh = CreateMesh ();
		lvMesh.vertices = new Vector3[] {
			MeshVertex(0, 0),
			MeshVertex(0, 1),
			MeshVertex(1, 0),
			MeshVertex(1, 1),
		};

		lvObject.AddComponent<MeshFilter> ().mesh = lvMesh;
		lvObject.tag = "GridCell";
		lvObject.AddComponent<MeshCollider> ();
		
		return lvObject;
	}
	

	Mesh CreateMesh() {
		Mesh mesh = new Mesh();
		
		mesh.name = "Grid Cell";
		mesh.vertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
		mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
		mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
		mesh.uv = new Vector2[] { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0) };
		
		return mesh;
	}
	
	Vector3 MeshVertex(int x, int z) {
		return new Vector3(x * cellSize, yOffset, z * cellSize);
	}

	public int getGridX(int pmGridId)
	{
		return pmGridId % gridWidth;
	}

	public int getGridZ(int pmGridId)
	{
		int gridX = getGridX (pmGridId);
		return (pmGridId - gridX) / gridWidth;
	}

	public int GetGridId(int pmGridX, int pmGridZ)
	{
		return (gridWidth * pmGridZ) + pmGridX;
	}

	public Vector3 getCellPosition(int id, float pmY = 0.0f)
	{
		float gridX = getGridX (id);
		float gridZ = getGridZ (id);

		return new Vector3(gridX + 0.5f,pmY,gridZ + 0.5f);
	}
	/*
	 * Czysci statusy wszystkich pol grida 
	 */
	public void ClearGridStatus(){
		for (int i = 0; i < mCells.Length; i++) {
			mCells [i].GetComponent<CellStatus> ().ClearStatus ();
		}
	}

	public Vector3 GetFigurineFacingRotation(int pmFigurineCell, int pmTargetCell)
	{
		float lvYRot = 0.0F;

		if (pmFigurineCell == pmTargetCell)
			return new Vector3 (0.0F, 0.0F, 0.0F);
		
		int lvFigX = getGridX (pmFigurineCell);
		int lvFigZ = getGridZ (pmFigurineCell);

		int lvTargetX = getGridX (pmTargetCell);
		int lvTargetZ = getGridZ (pmTargetCell);

		if (lvFigX == lvTargetX) {
			if (lvFigZ > lvTargetZ)
				lvYRot = 0.0F;
			else
				lvYRot = 180.0F;
		} else if (lvFigZ == lvTargetZ) {
			if (lvFigX > lvTargetX)
				lvYRot = 90.0F;
			else
				lvYRot = 270.0F;
		} else {
			if (lvFigX > lvTargetX) {
				if (lvFigZ > lvTargetZ)
					lvYRot = 45.0F;
				else
					lvYRot = 135.0F;
			} else {
				if (lvFigZ > lvTargetZ)
					lvYRot = 315.0F;
				else
					lvYRot = 225.0F;
			}
		}


		return new Vector3 (0.0F, lvYRot, 0.0F);
	}


}