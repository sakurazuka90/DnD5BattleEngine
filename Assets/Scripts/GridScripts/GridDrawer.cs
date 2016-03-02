using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridDrawer : MonoBehaviour {
	public float cellSize = 1;
	public int gridWidth = 10;
	public int gridHeight = 10;
	public float yOffset = 0.5f;
	public Material cellMaterialValid;
	public Material noMaterial;

	public GameObject[] _cells;

	public GameObject obstacleBase;
	public GameObject[] obstacles;

	void Start() {
		Create ();
	}

	public void Create()
	{
		RemoveCells ();

		_cells = new GameObject[gridHeight * gridWidth];

		for (int z = 0; z < gridHeight; z++) {
			for (int x = 0; x < gridWidth; x++) {
				_cells[z * gridWidth + x] = CreateChild(x,z);
			}
		}
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
		GameObject go = new GameObject();
		
		go.name = "Grid Cell";
		go.transform.parent = transform;
		go.transform.localPosition = Vector3.zero;

		go.AddComponent<MeshRenderer> ();

		if (!(x == 3 && z == 2)) {
			go.GetComponent<MeshRenderer> ().material = cellMaterialValid;
			go.AddComponent<CellStatus> ().avaiable = true;
		} else {
			go.GetComponent<MeshRenderer> ().material = noMaterial;
			go.AddComponent<CellStatus> ().avaiable = false;

			GameObject lvBase = Instantiate (obstacleBase);
			GameObject lvObstacle = Instantiate(obstacles[0]);

			lvObstacle.transform.parent = lvBase.transform;
			lvBase.transform.parent = go.transform;
			lvBase.transform.Translate(new Vector3(x + 0.5f,0,z + 0.5f));
		}

		///go.AddComponent<CellStatus> ().avaiable = true;

		Mesh lvMesh = CreateMesh ();
		lvMesh.vertices = new Vector3[] {
			MeshVertex(x, z),
			MeshVertex(x, z + 1),
			MeshVertex(x + 1, z),
			MeshVertex(x + 1, z + 1),
		};

		go.AddComponent<MeshFilter> ().mesh = lvMesh;
		go.tag = "GridCell";
		go.AddComponent<MeshCollider> ();
		
		return go;
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
		for (int i = 0; i < _cells.Length; i++) {
			_cells [i].GetComponent<CellStatus> ().ClearStatus ();
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