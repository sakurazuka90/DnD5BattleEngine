using System;
using System.Collections.Generic;

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
		throw new NotImplementedException ();
	}

	#endregion
}

