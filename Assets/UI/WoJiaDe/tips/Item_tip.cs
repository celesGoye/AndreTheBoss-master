using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_tip : MonoBehaviour
{
	public int index;
	public Item_tips tips;
	public ItemType type;
	public int num;
	public float size;
	public float width;
	public float height;
	public float timelast;
	
	public Image image;
	public Image bg;
	public Text textnum;
	public Text textremained;
	
	private Item item;
	private ItemReader itemReader;
	private GameManager gameManager;
	
	public void OnEnable()
	{
        if (gameManager == null)
            gameManager = GameObject.FindObjectOfType<GameManager>();
		
		itemReader=new ItemReader();
		itemReader.ReadFile();
	}
	
	public void UpdateItem()
	{
		Vector2 v=new Vector2(0,size*index);
		//this.GetComponent<RectTransform>().anchoredPosition =new Vector2( v.x*UnityEngine.Screen.width,v.y*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().anchoredPosition =new Vector2( 0,v.y*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,width*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,height*UnityEngine.Screen.height);
				
		item=itemReader.GetItemData(type);
		if(item.sprite!=null)
			image.sprite=item.sprite;
		textnum.text=(num>0?"+":"")+num;
		textremained.text=""+gameManager.itemManager.GetItemNum(type);

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
				break;
		}
	}
	
	public void Update()
	{
		timelast-=Time.deltaTime;
		if(timelast<=0)
			GameObject.Destroy(this.gameObject);
	}
	
	public void OnDestroy()
	{
		if(tips.isempty.Count>index)
		{
			tips.isempty[index]=true;
			tips.UpdateWaits();
		}
	}
}
