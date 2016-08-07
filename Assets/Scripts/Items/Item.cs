using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Item{

	protected string mName;
	protected float mPrice;
	protected string mDescription;

	public string inventoryFieldId;

	public string resourceImageName;

	public Color resourceImageColor;

	protected List<EquipementTypes> mEquipementTypes;

	public List<EquipementTypes> EquipementTypes{
		get{ return mEquipementTypes; }
	}

	public GameObject createInventoryObject()
	{
		GameObject lvItem = Resources.Load<GameObject>("ItemPref");

		lvItem = GameObject.Instantiate (lvItem);

		Image lvItemImage = lvItem.GetComponent<Image> ();
		Sprite lvItemSprite = this.GetSprite ();

		InventoryObjectStatus lvInvStatus = lvItem.GetComponent<InventoryObjectStatus>();
		lvInvStatus.InventorySlotId = inventoryFieldId;
		lvInvStatus.EquipementTypes = this.mEquipementTypes;

		lvItemImage.sprite = lvItemSprite;
		lvItem.name = mName;

		return lvItem;
	}

	public virtual void Equip(Player pmPlayer, bool pmUpdateUi = true)
	{
	}

	public virtual void UnEquip(Player pmPlayer, bool pmUpdateUi = true)
	{
	}

	public string GetName()
	{
		return mName;
	}

	public string GetDescription()
	{
		return mDescription;
	}

	public Sprite GetSprite()
	{
		return Resources.Load<Sprite>(resourceImageName);
	}

}
