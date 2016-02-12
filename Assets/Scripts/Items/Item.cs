using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Item{

	protected string mName;
	protected float mPrice;

	public string inventoryFieldId;

	public string resourceImageName;

	public Color resourceImageColor;

	public GameObject createInventoryObject()
	{
		//GameObject lvItem = new GameObject ();
		GameObject lvItem = (GameObject)Resources.Load("ItemPref", typeof(GameObject));

		lvItem = GameObject.Instantiate (lvItem);

		Image lvItemImage = lvItem.GetComponent<Image> ();
		Sprite lvItemSprite = (Sprite)Resources.Load(resourceImageName, typeof(Sprite));

		InventoryObjectStatus lvInvStatus = lvItem.GetComponent<InventoryObjectStatus>();
		lvInvStatus.InventorySlotId = inventoryFieldId;

		lvItemImage.sprite = lvItemSprite;
		lvItem.name = mName;

		return lvItem;
	}

}
