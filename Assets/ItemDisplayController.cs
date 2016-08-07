using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemDisplayController : MonoBehaviour {

	public GameObject itemImage;
	public GameObject itemName;
	public GameObject itemDescription;

	private Item mItem;

	public void FillDisplay(Item pmItem)
	{
		itemImage.GetComponent<Image> ().sprite = pmItem.GetSprite();
		itemName.GetComponent<Text> ().text = pmItem.GetName ();
		itemDescription.GetComponent<Text> ().text = pmItem.GetDescription ();
	}

	public void Close()
	{
		this.gameObject.SetActive (false);
	}

	public void Use()
	{
		
	}
}
