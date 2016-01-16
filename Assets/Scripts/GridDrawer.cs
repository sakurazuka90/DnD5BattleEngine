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

	public GameObject[] obstacles;

	void Start() {
		_cells = new GameObject[gridHeight * gridWidth];

		for (int z = 0; z < gridHeight; z++) {
			for (int x = 0; x < gridWidth; x++) {
				_cells[z * gridWidth + x] = CreateChild(x,z);
			}
		}
	}
	
	void Update () {
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

			GameObject lvObstacle = Instantiate(obstacles[0]);

			lvObstacle.transform.parent = go.transform;
			lvObstacle.transform.Translate(new Vector3(x + 0.5f,0,z + 0.5f));
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

	public Vector3 getCellPosition(int id)
	{
		float gridX = getGridX (id);
		float gridZ = getGridZ (id);

		return new Vector3(gridX + 0.5f,0.0f,gridZ + 0.5f);
	}
	/*
	 * Czysci statusy wszystkich pol grida 
	 */
	public void ClearGridStatus(){

		for (int i = 0; i < _cells.Length; i++) {
			GameObject lvCell = _cells[i];
			CellStatus lvStatus = lvCell.GetComponent<CellStatus>();
			lvStatus.movable = false;
			lvStatus.lvOportunity = false;
		}
	}


}