using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_ConsumePanel : MonoBehaviour
{
    public Upgrade_Item prefabitem;
	public Transform content;			//items' parent
	public UpgradePanel upgradePanel;
	
	public float width;
	public float height;
	public float size;
	
	private Pawn monster;
	private List<Vector2> items;
	private int itemcount;
	
	private CharacterReader characterReader;
	private ItemManager itemManager;
	
	public void OnEnable()
	{
		characterReader = FindObjectOfType<GameManager>().characterReader;
		itemManager = FindObjectOfType<GameManager>().itemManager;
		UpdateConsumePanel();
	}
	
	public bool IsUpgradeOK()
	{
		for(int i=0;i<content.childCount;i++)
		{
			Upgrade_Item item=content.GetChild(i).GetComponent<Upgrade_Item>();
			if(item.num<item.numneed)
				return false;
		}
		return true;
	}
	
	public void ConsumeItem()
	{
		for(int i=0;i<content.childCount;i++)
		{
			Upgrade_Item item=content.GetChild(i).GetComponent<Upgrade_Item>();
			itemManager.ConsumeItem(item.type, item.numneed);
		}
	}
	
	public void UpdateConsumePanel()
	{
		monster=upgradePanel.currentMonster;
		if(monster.GetLevel()>=Pawn.MaxLevel)
		{
			for(int i=0;i<content.childCount;i++)
			{
				Transform child=content.GetChild(i);
				GameObject.Destroy(child.gameObject);
			}
			return;
		}

		int unlocklevel = Mathf.CeilToInt((float) ((Monster)monster).monsterType / 3);

		items=characterReader.GetCharacterUpgrade(unlocklevel,monster.Name,monster.GetLevel()+1);
		itemcount=items.Count;
		if(content.childCount>itemcount)
		{
			for(int i=itemcount;i<content.childCount;i++)
			{
				Transform child=content.GetChild(i);
				GameObject.Destroy(child.gameObject);
			}
		}
		for(int i=0;i<itemcount;i++)
		{
			Upgrade_Item item;
			if(content.childCount<=i)
				item=GenItem(i);
			else
				item=content.GetChild(i).GetComponent<Upgrade_Item>();
			item.type=(ItemType)items[i].x;
			if(itemManager.ItemsOwn.ContainsKey(item.type))
				item.num=itemManager.ItemsOwn[item.type];
			else
				item.num=0;
			item.numneed=(int)items[i].y;
			UpdateItem(item,i);
		}
		
		content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemcount*width*UnityEngine.Screen.height); 
	}
	
	public Upgrade_Item GenItem(int index)
	{
		Upgrade_Item newitem=Instantiate<Upgrade_Item>(prefabitem);
		newitem.transform.SetParent(content);
		UpdateItem(newitem,index);
		return newitem;
	}
	public void UpdateItem(Upgrade_Item item, int index)
	{
		item.index=index;
		item.size=size;
		item.width=width;
	}
}
