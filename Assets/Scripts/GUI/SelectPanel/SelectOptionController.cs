using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectOptionController : AbstractSelectOptionController {


		
	protected override void Select()
	{

		MarkSelectedOption ();

		AbstractPanelController lvController = _controller.GetComponent<AbstractPanelController> ();
		lvController.Select (this.GetValue());
	}

	public string GetValue()
	{
		return this.gameObject.transform.Find ("Text").GetComponent<Text> ().text;
	}

}
