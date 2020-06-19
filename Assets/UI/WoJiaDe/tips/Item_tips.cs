using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_tips : MonoBehaviour
{
    public Item_tip prefab_item;
	public List<bool> isempty;
	public int maxnum;
	public List<Vector2> waitlist; //type, num
	
	public void OnEnable()
	{
		InitList();
	}
	
	public class ItemConsumeInfo
	{
		int num;
		ItemType type;
	}

	private void InitList()
	{
		isempty=new List<bool>();
		for(int i=0;i<maxnum;i++)
		{
			isempty.Add(true);
		}
	}
	
	public bool AddTip(ItemType type,int num)
	{
		if(num==0)
			return false;
		
		if(isempty==null||isempty.Count<=0)
			InitList();
		
		for(int i=0;i<maxnum;i++)
		{
			if(isempty[i]==true)
			{
				Item_tip newitem=GameObject.Instantiate(prefab_item);
				newitem.transform.SetParent(this.transform);
				newitem.index=i;
				newitem.tips=this;
				newitem.type=type;
				newitem.num=num;
				newitem.UpdateItem();
				isempty[i]=false;
				return true;
			}
		}
		waitlist.Add(new Vector2((int)type,num));
		return false;
	}
	
	public void UpdateWaits()
	{
		if(waitlist.Count<=0)
			return;
		AddTip((ItemType)waitlist[0].x,(int)waitlist[0].y);
		waitlist.Remove(waitlist[0]);
	}
}
