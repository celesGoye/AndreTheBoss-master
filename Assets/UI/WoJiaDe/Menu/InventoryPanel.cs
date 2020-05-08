using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
	public ItemManager itemManager;
	public InventoryDisplay items;
	
	public Dictionary<ItemType,int> itemsOwn;
	private int itemcount;
	
	public void OnEnable()
	{
		UpdateInventory();
		//？？我把这个类忘了，能不能假装无事发生
	}
	public void UpdateInventory()
	{
		itemsOwn=itemManager.ItemsOwn;
		itemcount=itemsOwn.Count;
	}

}
