using System;
using System.Collections.Generic;
using UnityEngine;

public class CreatorSelectFromGrid : AbstractSelectFromGrid
{
	public bool functionalPlaceMode = false;
	public FunctionalStates currentFunctionalState;
	public GameObject creatorObstacle;

	private List<int> constructorFilledSquares;

	public CreatorSelectFromGrid ()
	{
	}

	protected override void RaycastAction (GameObject gridCell)
	{
		CellStatus lvCellStatus = gridCell.GetComponent<CellStatus> ();
		handleMouseOverCell (lvCellStatus);

		Vector3 lvPosition = gridCell.transform.position;
		int lvCellId = GridDrawer.instance.GetGridId ((int)lvPosition.x, (int)lvPosition.z);

		GameObject figurine = creatorObstacle;
		FigurineStatus figurineStatus = null;

		GridDrawer.instance.SetStateToCells (constructorFilledSquares, CellStates.ENABLED);

		if (figurine != null)
			figurineStatus = figurine.GetComponent<FigurineStatus> ();

	}

	private void handleMouseOverCell (CellStatus pmCellStatus)
	{
		if (!functionalPlaceMode) {
			pmCellStatus.selected = true;
		} else if (functionalPlaceMode) {
			
			switch (currentFunctionalState) {
			case FunctionalStates.PLAYER_SPAWN:
				pmCellStatus.spawnPlayer = true;
				break;
			case FunctionalStates.ENEMY_SPAWN:
				pmCellStatus.spawnEnemy = true;
				break;
			}
		} else if (pmCellStatus.movable) {
			pmCellStatus.selected = true;
		}
		lastStatus = pmCellStatus;
	}

	protected override bool RaycastActionCondition ()
	{
		throw new NotImplementedException ();
	}
}

