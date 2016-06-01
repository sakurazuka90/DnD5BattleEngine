using UnityEngine;
using System.Collections.Generic;
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

	public bool isAi = false;

	public int movePoints = 0;

	private Vector3 _target = Vector3.zero;

	public bool abort = false;

	// Use this for initialization
	void Start () {
		_figurineAnimator = this.gameObject.GetComponentInChildren<Animator> ();
	}
	
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
				string [] tempsteps = path.Split ('_');

				steps = new string[tempsteps.Length - 1];

				for (int i = 1; i < tempsteps.Length; i++) {
					steps [i - 1] = tempsteps [i];
				}


				path = "";

				// path containt current cell
				//mStepsMoved = steps.Length - 1;

				//if(mStepsMoved > 0)
				//mStepsMoved += SelectFromGrid.instance.CountDifficultTerrainFieldsInPath (steps);

				//_figurineAnimator.SetBool ("isWalking",true);
				ButtonToggler.ToggleButtonOff ("MoveButton");
				ButtonToggler.ToggleButtonOff ("InventoryButton");

			}

			if(steps.Length > 0)
			{
				string lvCurrentStep = steps[currentStep];

				if (_target == Vector3.zero) {
					

					Vector3 lvTarget = GridDrawer.instance.getCellPosition(int.Parse(lvCurrentStep));

					int moveCost = 1;

					if (GridDrawer.IsCellDifficultTerrain (GridDrawer.instance.mCells [int.Parse (lvCurrentStep)])) {
						moveCost++;
					}

					if (moveCost > movePoints) {
						lvTarget = Vector3.zero;
						//currentStep = steps.Length - 1;
						abort = true;
						lvCurrentStep = steps[(currentStep-1)];
					} else {
						movePoints -= moveCost;
						mStepsMoved += moveCost;
					}

					_target = lvTarget;

				}







				if(_target != transform.position && _target != Vector3.zero)
				{
					transform.position = Vector3.MoveTowards(transform.position, _target, moveSpeed * Time.deltaTime);
				} else {

					FigurineStatus lvStatus = this.gameObject.GetComponent<FigurineStatus>();

					gridX = GridDrawer.instance.getGridX(int.Parse(lvCurrentStep));
					lvStatus.gridX = gridX;

					gridZ = GridDrawer.instance.getGridZ(int.Parse(lvCurrentStep));
					lvStatus.gridZ = gridZ;

					_target = Vector3.zero;

					if(currentStep != (steps.Length -1) && !abort)
					{


						currentStep ++;

						Vector3 lvRotation = GridDrawer.instance.GetFigurineFacingRotation (int.Parse(lvCurrentStep), int.Parse(steps [currentStep]));
						this.gameObject.transform.eulerAngles = lvRotation;

					} else {
						AbortMovement ();

						PlayerSpooler lvSpooler = PlayerSpooler.instance;
						PlayerSpooler.DecreaseMoves (mStepsMoved);

						//_figurineAnimator.SetBool ("isWalking",false);

						if (isAi)
							AIEngine.instance.free = true;
					}
				}
			}
		}
	}

	public void AbortMovement()
	{
		steps = new string[0];
		currentStep = 0;
		isMoving = false;
		ButtonToggler.ToggleButtonOn ("MoveButton");
		ButtonToggler.ToggleButtonOn ("InventoryButton");
		abort = false;
	}

	public bool IsOnSelectedField()
	{
		Vector3 fieldPosition = GridDrawer.instance.getCellPosition (GridDrawer.instance.GetGridId (gridX, gridZ));

		return  new Vector3 (fieldPosition.x, 0.0f, fieldPosition.z) == transform.position;
	}

}
