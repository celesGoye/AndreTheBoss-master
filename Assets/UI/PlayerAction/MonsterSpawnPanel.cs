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
	private Dictionary<ItemType,int> items;
	private List<Vector3> itemAndNumbers;//typeNumNumneed
	private int itemcount;
	
	private CharacterReader characterReader;
	
	public void OnEnable()
	{
		characterReader=new CharacterReader();
		characterReader.ReadUpgradeFile();
		itemAndNumbers=new List<Vector3>();
	}
	
	public bool IsSpawnOK()
	{
		if(monsterPallete.currentType==MonsterType.NUM)
			return false;
		for(int i=0;i<itemcount;i++)
		{
			if(itemAndNumbers[i].y<itemAndNumbers[i].z)
				return false;
		}
		return true;
	}
	
	public void ConsumeItem()
	{
		Debug.Log("Consume.");
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
			Debug.Log("cleared"+monsterPallete.currentType);
	}
	
	public void UpdateSpawnPanel()
	{

		ClearSpawnPanel();
		if(monsterPallete.currentType==null||monsterPallete.currentType==MonsterType.NUM)
			return;
		
		items=characterReader.GetCharacterUpgrade(monsterPallete.currentType.ToString(),0);
		itemcount=items.Count;
		Dictionary<ItemType,int>.Enumerator en=items.GetEnumerator();
		itemAndNumbers.Clear();
		for(int i=0;i<itemcount;i++)
		{
			if(en.MoveNext())
			{
					itemAndNumbers.Add(new Vector3((int)en.Current.Key,monsterPallete.gameManager.itemManager.ItemsOwn[en.Current.Key],en.Current.Value));
					Upgrade_Item item=GenItem(i);
					item.type=(ItemType)itemAndNumbers[i].x;
					item.num=(int)itemAndNumbers[i].y;
					item.numneed=(int)itemAndNumbers[i].z;
					UpdateItem(item,i);
			}
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
