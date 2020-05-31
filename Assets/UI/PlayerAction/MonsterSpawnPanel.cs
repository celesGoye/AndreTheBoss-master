using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSpawnPanel : MonoBehaviour
{
	public MonsterPallete monsterPallete;
    public Upgrade_Item prefabitem;
	public Transform content;
	
	public float width;
	public float height;
	public float size;
	
	private MonsterType monster;
	private List<Vector2> items;
	private List<Vector3> itemAndNumbers;//typeNumNumneed
	private int itemcount;
	
	private CharacterReader characterReader;
	
	public void OnEnable()
	{
		if(characterReader==null)
		characterReader = FindObjectOfType<GameManager>().characterReader;
	}
	
	public bool IsSpawnOK()
	{
		if(monsterPallete.currentType==MonsterType.NUM)
			return false;
		for(int i=0;i<itemcount;i++)
		{
			if(!monsterPallete.gameManager.itemManager.ItemsOwn.ContainsKey((ItemType)items[i].x))
				return false;
			if(monsterPallete.gameManager.itemManager.ItemsOwn[(ItemType)items[i].x]<items[i].y)
				return false;
		}
		return true;
	}
	
	public void ConsumeItem()
	{
		//Debug.Log("Consume.");
		for(int i=0;i<content.childCount;i++)
		{
			Upgrade_Item item=content.GetChild(i).GetComponent<Upgrade_Item>();
			monsterPallete.gameManager.itemManager.ConsumeItem(item.type, item.numneed);
		}
	}
	
	public void ClearSpawnPanel()
	{
		for(int i=0;i<content.childCount;i++)
			{
				GameObject.Destroy(content.GetChild(i).gameObject);
			}
	}
	
	public void UpdateSpawnPanel()
	{

		ClearSpawnPanel();
		if(monsterPallete.currentType==MonsterType.NUM)
			return;

		int unlocklevel = Mathf.CeilToInt((float)monsterPallete.currentType / 3);
		items =characterReader.GetCharacterUpgrade(unlocklevel, monsterPallete.currentType.ToString(), 1);	// items to spawn monster at level 1
		itemcount=items.Count;
		for(int i=0;i<itemcount;i++)
		{
			Upgrade_Item item=GenItem(i);
			item.type=(ItemType)items[i].x;
			if(monsterPallete.gameManager.itemManager.ItemsOwn.ContainsKey((ItemType)items[i].x))
				item.num= monsterPallete.gameManager.itemManager.ItemsOwn[(ItemType)items[i].x];
			else
				item.num=0;
			item.numneed=(int)items[i].y;
			UpdateItem(item,i);
		}
		
		if(IsSpawnOK())
			monsterPallete.monsterSpawnButton.GetComponent<Button>().interactable=true;
		else
			monsterPallete.monsterSpawnButton.GetComponent<Button>().interactable=false;
		
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
