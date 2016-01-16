using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FigurineMover : MonoBehaviour {

	public int gridX = 0;
	public int gridZ = 0;

	public string path = "";
	public bool isMoving = false;

	private string [] steps;
	private int currentStep = 0;

	public float moveSpeed;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (!isMoving) {
			Transform lvTransform = this.gameObject.transform;

			float figurineX = lvTransform.position.x;
			float figurineZ = lvTransform.position.z;

			float figurineXSize = lvTransform.localScale.x;
			float figurineZSize = lvTransform.localScale.z;

			Vector3 lvMove = new Vector3 (gridX - figurineX + figurineXSize / 2, 0, gridZ - figurineZ + figurineZSize / 2);

			lvTransform.Translate (lvMove);
		}
		handlePath ();

	}

	private void handlePath(){
		if (isMoving) {
			if (path.Length > 0) {

				// reset to first step of path
				currentStep = 0;
				// set steps array
				steps = path.Split ('_');

				path = "";
			} else if(steps.Length > 0)
			{
				GameObject lvMoveButtonGameObject = GameObject.Find ("MoveButton");
				lvMoveButtonGameObject.GetComponent<Button>().interactable = false;


				string lvCurrentStep = steps[currentStep];

				GameObject lvCanvas = GameObject.Find ("GridDrawer");
				GridDrawer lvDrawer = lvCanvas.GetComponent<GridDrawer> ();

				Vector3 lvTarget = lvDrawer.getCellPosition(int.Parse(lvCurrentStep));

				Vector3 lvPosition = transform.position;

				if(lvTarget != lvPosition)
				{
					transform.position = Vector3.MoveTowards(transform.position, lvTarget, moveSpeed * Time.deltaTime);
				} else {
					if(currentStep != (steps.Length -1))
					{
						FigurineStatus lvStatus = this.gameObject.GetComponent<FigurineStatus>();

						gridX = lvDrawer.getGridX(int.Parse(lvCurrentStep));
						lvStatus.gridX = gridX;

						gridZ = lvDrawer.getGridZ(int.Parse(lvCurrentStep));
						lvStatus.gridZ = gridZ;

						currentStep ++;
						lvTarget = lvDrawer.getCellPosition(int.Parse(lvCurrentStep));
						//transform.LookAt(lvTarget);
						//transform.Rotate(new Vector3(0,180,0));

					} else {
						FigurineStatus lvStatus = this.gameObject.GetComponent<FigurineStatus>();
						
						gridX = lvDrawer.getGridX(int.Parse(lvCurrentStep));
						lvStatus.gridX = gridX;
						
						gridZ = lvDrawer.getGridZ(int.Parse(lvCurrentStep));
						lvStatus.gridZ = gridZ;
						steps = new string[0];
						currentStep = 0;
						isMoving = false;
						lvMoveButtonGameObject.GetComponent<Button>().interactable = true;

						GameObject lvSpoolerObject = GameObject.Find ("PlayerSpooler");
						lvSpoolerObject.GetComponent<PlayerSpooler>().spool();
					}
				}
			}
		}
	}


}