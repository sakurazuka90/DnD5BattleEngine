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

	private int mStepsMoved = 0;

	Animator _figurineAnimator;

	// Use this for initialization
	void Start () {
		_figurineAnimator = this.gameObject.GetComponentInChildren<Animator> ();
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

				// path containt current cell
				mStepsMoved = steps.Length - 1;

				_figurineAnimator.SetBool ("isWalking",true);

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

						AbortMovement ();

						GameObject lvSpoolerObject = GameObject.Find ("PlayerSpooler");
						PlayerSpooler lvSpooler = lvSpoolerObject.GetComponent<PlayerSpooler> ();

						lvSpooler.DecreaseMoves (mStepsMoved);
						mStepsMoved = 0;

						_figurineAnimator.SetBool ("isWalking",false);

						if(lvStatus.movesLeft == 0)
							lvSpoolerObject.GetComponent<PlayerSpooler>().spool();
					}
				}
			}
		}
	}

	public void AbortMovement()
	{
		GameObject lvMoveButtonGameObject = GameObject.Find ("MoveButton");
		steps = new string[0];
		currentStep = 0;
		isMoving = false;
		lvMoveButtonGameObject.GetComponent<Button>().interactable = true;
	}


}
