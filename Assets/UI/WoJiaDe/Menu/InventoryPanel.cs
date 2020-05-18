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
	}
	public void UpdateInventory()
	{
		itemsOwn=itemManager.ItemsOwn;
		itemcount=itemsOwn.Count;
	}

}
