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

	public bool isAi = false;

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
				steps = path.Split ('_');

				path = "";

				// path containt current cell
				mStepsMoved = steps.Length - 1;

				if(mStepsMoved > 0)
				mStepsMoved += SelectFromGrid.instance.CountDifficultTerrainFieldsInPath (steps);

				_figurineAnimator.SetBool ("isWalking",true);

			} else if(steps.Length > 0)
			{
				ButtonToggler.ToggleButtonOff ("MoveButton");
				ButtonToggler.ToggleButtonOff ("InventoryButton");

				string lvCurrentStep = steps[currentStep];

				Vector3 lvTarget = GridDrawer.instance.getCellPosition(int.Parse(lvCurrentStep));

				Vector3 lvPosition = transform.position;

				if(lvTarget != lvPosition)
				{
					transform.position = Vector3.MoveTowards(transform.position, lvTarget, moveSpeed * Time.deltaTime);
				} else {
					if(currentStep != (steps.Length -1))
					{
						FigurineStatus lvStatus = this.gameObject.GetComponent<FigurineStatus>();

						gridX = GridDrawer.instance.getGridX(int.Parse(lvCurrentStep));
						lvStatus.gridX = gridX;

						gridZ = GridDrawer.instance.getGridZ(int.Parse(lvCurrentStep));
						lvStatus.gridZ = gridZ;

						currentStep ++;

						Vector3 lvRotation = GridDrawer.instance.GetFigurineFacingRotation (int.Parse(lvCurrentStep), int.Parse(steps [currentStep]));
						this.gameObject.transform.eulerAngles = lvRotation;

					} else {
						FigurineStatus lvStatus = this.gameObject.GetComponent<FigurineStatus>();
						
						gridX = GridDrawer.instance.getGridX(int.Parse(lvCurrentStep));
						lvStatus.gridX = gridX;
						
						gridZ = GridDrawer.instance.getGridZ(int.Parse(lvCurrentStep));
						lvStatus.gridZ = gridZ;

						AbortMovement ();

						PlayerSpooler lvSpooler = PlayerSpooler.instance;
						PlayerSpooler.DecreaseMoves (mStepsMoved);

						mStepsMoved = 0;

						_figurineAnimator.SetBool ("isWalking",false);

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
	}

}
