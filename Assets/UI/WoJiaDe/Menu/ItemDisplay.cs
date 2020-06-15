using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
	public int index;
	public float width;
	public float height;
	public float size;
    public ItemType type;
	public int num;
	public Image image;
	public Image bg;
	public Text text;
	public InventoryDisplay inventory;
	
	public bool useable;
	
	private ItemReader reader;
	private Item item;
	private fontsizeControl fontsize;
	
	void Awake()
	{
		this.GetComponent<Button>().onClick.AddListener(OnItemBtn);	
	}
	
	public void OnEnable()
	{
		reader=new ItemReader();
		reader.ReadFile();
	}
	
	public void OnItemBtn()
	{
		if(fontsize==null)
			fontsize=inventory.use.gameObject.GetComponent<fontsizeControl>();
		inventory.usebutton.gameObject.SetActive(item.itemPrimaryType==ItemPrimaryType.Buff?true:false);
		inventory.currentType=type;
		Debug.Log("item type: ------"+type.ToString());
		inventory.OnItemBtn();
		inventory.name.text=type.ToString();
		inventory.typename.text=item.itemPrimaryType.ToString()+"\n";
		inventory.use.text=item.Access+"<size="+fontsize.newsize/2+">\n\n</size>"+item.Use+"<size="+fontsize.newsize/2+">\n\n</size>"+item.Intro+"\n";
		//inventory.use.text=item.Access+"\n\n"+item.Use+"\n\n"+item.Intro+"\n";
	}
	
	public void UpdatePosition()
	{
		Vector2 v=new Vector2(width*(index%4),-height*(index/4));
		this.GetComponent<RectTransform>().anchoredPosition = v*UnityEngine.Screen.height;
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,size*UnityEngine.Screen.height);
	}
	
	public void UpdateItemDisplay()
	{
		item=reader.GetItemData(type);
		image.sprite=item.sprite;
		text.text=""+num;
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
