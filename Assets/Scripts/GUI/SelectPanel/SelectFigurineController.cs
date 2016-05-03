using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectFigurineController : AbstractPanelController
{
	public SelectFigurineController ()
	{
	}

	#region implemented abstract members of AbstractPanelController

	protected override List<string> GetOptions ()
	{
		return FigurineFileReader.instance.GetFigurineList ();
	}

	public override void Load ()
	{
		AssetStatsEditor lvStatusEditor = GameObject.Find ("AssetEditPanel").GetComponent<AssetStatsEditor> ();
		lvStatusEditor.SetFunction (_selected);
		this.gameObject.SetActive (false);
	}

	#endregion
}

