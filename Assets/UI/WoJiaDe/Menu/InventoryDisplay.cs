using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
	public InventoryPanel inventory;
	public ItemDisplay Prefab_item;
	
	public float width;
	public float height;
	public float size;
	
	public Color color;
	public ItemType currentType;
	
	public Text name;
	public Text use;
	public Button usebutton;
	
	private Dictionary<ItemType,int> items;
	private int itemcount;
	
	void Awake()
	{
		usebutton.onClick.AddListener(OnUseBtn);
	}
	
	public void OnEnable()
	{
		UpdateInventory();
	}

	public void UpdateInventory()
	{
		items=inventory.itemsOwn;
		itemcount=items.Count;
		Dictionary<ItemType,int>.Enumerator en=items.GetEnumerator();
		if(this.transform.childCount>itemcount)
		{
			for(int i=itemcount;i<this.transform.childCount;i++)
			{
				Debug.Log("destroying child:----"+i);
				Transform child=this.transform.GetChild(i);
				GameObject.Destroy(child.gameObject);
			}
		}
		for(int i=0;i<itemcount;i++)
		{
			if(en.MoveNext())
			{
				if(this.transform.childCount<=i)
				{
					ItemDisplay item=GenItem(i);
					item.type=en.Current.Key;
					item.num=en.Current.Value;
					Debug.Log(item.type+" ========== "+item.num);
					UpdateItem(item,i);
				}
				else
				{
					ItemDisplay item=this.transform.GetChild(i).GetComponent<ItemDisplay>();
					item.type=en.Current.Key;
					item.num=en.Current.Value;
				}
			}
		}
		
		Vector2 v=new Vector2(0,-(itemcount<=12?0:(itemcount-12)/4+1)*height*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().offsetMin=v;
	}
	
	public ItemDisplay GenItem(int index)
	{
		ItemDisplay newitem=Instantiate<ItemDisplay>(Prefab_item);
		newitem.transform.SetParent(this.transform);
		newitem.inventory=this.GetComponent<InventoryDisplay>();
		UpdateItem(newitem,index);
		return newitem;
	}
	
	public void UpdateItem(ItemDisplay item, int index)
	{
		item.index=index;
		item.size=size;
		item.width=width;
		item.height=height;
	}
	
	public void OnItemBtn()
	{
		for(int i=0;i<itemcount;i++)
		{
			ItemDisplay item=this.transform.GetChild(i).GetComponent<ItemDisplay>();
			item.gameObject.GetComponent<Image>().color=item.type!=currentType?Color.white:color;
		}
		UpdateInventory();
	}
	
	public void OnUseBtn()
	{
		inventory.itemManager.ConsumeItem(currentType, 1);
		UpdateInventory();
	}
}
