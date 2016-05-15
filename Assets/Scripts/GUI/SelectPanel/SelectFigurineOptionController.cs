using System;

public class SelectFigurineOptionController : AbstractSelectOptionController
{

	private int _numericValue;

	public int NumericValue{
		set{ this._numericValue = value; }
	}

	public SelectFigurineOptionController ()
	{
	}

	protected override void Select ()
	{
		MarkSelectedOption ();

		SelectFigurineController lvController = _controller.GetComponent<SelectFigurineController> ();
		lvController.Select (_numericValue.ToString());
	}


}


