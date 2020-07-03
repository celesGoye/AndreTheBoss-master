using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	private GameManager gameManager;
	//private ItemReader itemReader;
	
	public List<ItemType> ItemsGot;
	public Dictionary<ItemType,int> ItemsOwn;
	
    public void OnEnable()
	{
		gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		ItemsGot=new List<ItemType>();
		ItemsOwn=new Dictionary<ItemType,int>();
		InitItems();

		//test
		//for(int i=0;i<(int)ItemType.NUM;i++)
		//{
		//	GetItem((ItemType)i,99);
		//}
		 if(PlayerPrefs.GetInt("IsNewGame") == 1)
			GetItem(ItemType.Soul,6);
    }
	
	//test
	public void Get99Resources()
	{
		for(int i=0;i<(int)ItemType.NUM;i++)
		{
			GetItem((ItemType)i,99);
		}
	}
	
	public void InitItems()
	{
		/*for(int i=0;i<(int)ItemType.NUM;i++)
			GetItem((ItemType)i,0);*/
	}
	
	public void GetItem(ItemType itemType, int num)
	{
		if(ItemsOwn.ContainsKey(itemType))
		{
			ItemsOwn[itemType]+=num;
		}
		else
		{
			ItemsOwn[itemType]=num;
			if(!ItemsGot.Contains(itemType))
			{
				ItemsGot.Add(itemType);
			}
		}
		gameManager.gameInteraction.itemtips.AddTip(itemType,num);
	}
	
	public void ConsumeItem(ItemType itemType, int num)
	{
		if(ItemsOwn.ContainsKey(itemType)&&ItemsOwn[itemType]>=num)
		{
			ItemsOwn[itemType]-=num;
			if(ItemsOwn[itemType]==0)
				ItemsOwn.Remove(itemType);
			
			gameManager.gameInteraction.itemtips.AddTip(itemType,-num);
		}
		else
			Debug.Log("consumeitem error");
	}
	
	public bool IsHaveEnoughItem(ItemType itemType, int num)
	{
		return ItemsOwn.ContainsKey(itemType)&&ItemsOwn[itemType]>=num;
	}
	
	public int GetItemNum(ItemType type)
	{
		return ItemsOwn.ContainsKey(type)?ItemsOwn[type]:0;
	}
}
