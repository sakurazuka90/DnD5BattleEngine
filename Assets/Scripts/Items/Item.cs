using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Item{

	protected string mName;
	protected float mPrice;

	public string inventoryFieldId;

	public string resourceImageName;

	public Color resourceImageColor;

	protected List<EquipementTypes> mEquipementTypes;

	public List<EquipementTypes> EquipementTypes{
		get{ return mEquipementTypes; }
	}

	public GameObject createInventoryObject()
	{
		//GameObject lvItem = new GameObject ();
		GameObject lvItem = (GameObject)Resources.Load("ItemPref", typeof(GameObject));

		lvItem = GameObject.Instantiate (lvItem);

		Image lvItemImage = lvItem.GetComponent<Image> ();
		Sprite lvItemSprite = (Sprite)Resources.Load(resourceImageName, typeof(Sprite));

		InventoryObjectStatus lvInvStatus = lvItem.GetComponent<InventoryObjectStatus>();
		lvInvStatus.InventorySlotId = inventoryFieldId;
		lvInvStatus.EquipementTypes = this.mEquipementTypes;

		lvItemImage.sprite = lvItemSprite;
		lvItem.name = mName;

		return lvItem;
	}

	public virtual void Equip(Player pmPlayer)
	{
	}

}
