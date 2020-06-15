using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_Item : MonoBehaviour
{
	public int index;
	public float width;
	public float size;
    public ItemType type;
	public int num;
	public int numneed;
	public Image image;
	public Image bg;
	public Text text;
	public ItemReader itemReader;
	
	private Item item;
	
	public void OnEnable()
	{
		
		itemReader=FindObjectOfType<GameManager>().itemReader;
	}
	public void UpdatePosition()
	{
		Vector2 v=new Vector2(width*index,0);
		this.GetComponent<RectTransform>().anchoredPosition = v*UnityEngine.Screen.height;
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,size*UnityEngine.Screen.height);
		
		/*item=itemReader.GetItemData(type);
		if(item.sprite!=null)
		image.sprite=item.sprite;
		if(num<numneed)
			text.text="<color=red>"+num+"</color>/"+numneed;
		else
			text.text=num+"/"+numneed;*/
	}
	
	public string ToString()
	{
		return ("type:"+type+"/nnum:"+num+"/nnumneed"+numneed);
	}
	
	public void UpdateItemDisplay()
	{
		if(itemReader==null)
			itemReader=FindObjectOfType<GameManager>().itemReader;
		item=itemReader.GetItemData(type);
		if(item.sprite!=null)
		image.sprite=item.sprite;
		if(num<numneed)
			text.text="<color=red>"+num+"</color>/"+numneed;
		else
			text.text=num+"/"+numneed;
		switch(item.itemPrimaryType)
		{
			case ItemPrimaryType.SoulType:
				bg.color=Item.SoulColor;
				break;
			case ItemPrimaryType.Farm:
				bg.color=Item.FarmColor;
				break;
			case ItemPrimaryType.Mine:
				bg.color=Item.MineColor;
				break;
			case ItemPrimaryType.Buff:
				bg.color=Item.BuffColor;
				break;
			default:
				return;
		}
	}
}
