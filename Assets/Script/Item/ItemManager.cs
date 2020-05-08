using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	private GameManager gameManager;
	//private ItemReader itemReader;
	
	public List<ItemType> ItemsGot;
	private Dictionary<ItemType,bool> isItemsGot;
	public Dictionary<ItemType,int> ItemsOwn;
	
    public void OnEnable()
	{
		gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		ItemsGot=new List<ItemType>();
		isItemsGot=new Dictionary<ItemType,bool>();
		ItemsOwn=new Dictionary<ItemType,int>();
		for(int i=0;i<(int)ItemType.NUM;i++)
			isItemsGot[(ItemType)i]=false;
		
		//test
		for(int i=0;i<(int)ItemType.NUM;i++)
			GetItem((ItemType)i,10);

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
			if(isItemsGot.ContainsKey(itemType)&&isItemsGot[itemType]==false)
			{
				isItemsGot[itemType]=true;
				ItemsGot.Add(itemType);
			}
		}
	}
	
	public void ConsumeItem(ItemType itemType, int num)
	{
		if(ItemsOwn.ContainsKey(itemType)&&ItemsOwn[itemType]>=num)
		{
			ItemsOwn[itemType]-=num;
			if(ItemsOwn[itemType]==0)
				ItemsOwn.Remove(itemType);
		}
		else
			Debug.Log("consumeitem error");
	}

}
