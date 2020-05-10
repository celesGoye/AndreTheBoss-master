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
	private Dictionary<ItemType,int> items;
	private int itemcount;
	
	private CharacterReader characterReader;
	
	public void OnEnable()
	{
		characterReader=new CharacterReader();
		characterReader.ReadUpgradeFile();
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
			upgradePanel.itemManager.ConsumeItem(item.type, item.numneed);
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
		items=characterReader.GetCharacterUpgrade(monster.Name,monster.GetLevel());
		itemcount=items.Count;
		Dictionary<ItemType,int>.Enumerator en=items.GetEnumerator();
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
			if(en.MoveNext())
			{
				if(content.childCount<=i)
				{
					Upgrade_Item item=GenItem(i);
					item.type=en.Current.Key;
					
					item.num=upgradePanel.itemManager.ItemsOwn[item.type];
					item.numneed=en.Current.Value;
					UpdateItem(item,i);
				}
				else
				{
					Upgrade_Item item=content.GetChild(i).GetComponent<Upgrade_Item>();
					item.type=en.Current.Key;
					item.num=upgradePanel.itemManager.ItemsOwn[item.type];
					item.numneed=en.Current.Value;
				}
			}
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
