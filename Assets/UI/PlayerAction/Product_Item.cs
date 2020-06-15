using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Product_Item : MonoBehaviour
{
    public int index;
	public float width;
	public float height;
	public float size;
    public ItemType type;
	
	public Image image;
	public Image bg;
	public Button button;
	public FacilityPallete facilityPallete;
	
	private Item item;
	private ItemReader itemReader;
	
	public void OnEnable()
	{
		button.onClick.AddListener(OnItemBtn);	
		itemReader=FindObjectOfType<GameManager>().itemReader;
	}
	
	public void OnItemBtn()
	{
		facilityPallete.currentItem=type;
		facilityPallete.OnItemButton();
	}
	
	public void UpdatePosition()
	{
		Vector2 v=new Vector2(width*(index),0);
		this.GetComponent<RectTransform>().anchoredPosition = v*UnityEngine.Screen.height;
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,size*UnityEngine.Screen.height);
	}
	
	public void UpdateItemDisplay()
	{
		Debug.Log("hello");
		item=itemReader.GetItemData(type);
		if(item.sprite!=null)
		image.sprite=item.sprite;
	
		switch(item.itemPrimaryType)
		{
			case ItemPrimaryType.SoulType:
				bg.color=Item.SoulColor;
				break;
			case ItemPrimaryType.Farm:
			Debug.Log("farmmmm");
				bg.color=Item.FarmColor;
				break;
			case ItemPrimaryType.Mine:
				bg.color=Item.MineColor;
				break;
			case ItemPrimaryType.Buff:
				bg.color=Item.BuffColor;
				break;
			default:
				break;
		}
	}
}
