using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemDisplayController : MonoBehaviour {

	public GameObject itemImage;
	public GameObject itemName;
	public GameObject itemDescription;
	public GameObject useButton;

	private Item mItem;

	public void FillDisplay(Item pmItem)
	{
		itemImage.GetComponent<Image> ().sprite = pmItem.GetSprite();
		itemName.GetComponent<Text> ().text = pmItem.GetName ();
		itemDescription.GetComponent<Text> ().text = pmItem.GetDescription ();

		mItem = pmItem;

		if (pmItem is UsableItem) {
			useButton.GetComponent<Button> ().interactable = true;
		} else {
			useButton.GetComponent<Button> ().interactable = false;
		}
	}

	public void Close()
	{
		this.gameObject.SetActive (false);
	}

	public void Use()
	{
		if (mItem is UsableItem) {
			((UsableItem)mItem).Use (PlayerSpooler.instance.GetSpooledPlayer ());
			Close ();
		}
	}
}
