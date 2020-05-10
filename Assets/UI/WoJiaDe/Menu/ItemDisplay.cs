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
	public Text text;
	public InventoryDisplay inventory;
	
	public bool useable;
	
	private ItemReader reader;
	private Item item;
	
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
		inventory.usebutton.gameObject.SetActive(item.itemPrimaryType==ItemPrimaryType.Buff?true:false);
		inventory.currentType=type;
		Debug.Log("item type: ------"+type.ToString());
		inventory.OnItemBtn();
		inventory.name.text=type.ToString();
		inventory.use.text="<size=40>"+item.itemPrimaryType.ToString()+"	"+item.Use+"</size>";
	}
	
	public void Update()
	{
		Vector2 v=new Vector2(width*(index%4),-height*(index/4));
		this.GetComponent<RectTransform>().anchoredPosition = v*UnityEngine.Screen.height;
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,size*UnityEngine.Screen.height);
		
		item=reader.GetItemData(type);
		image.sprite=item.sprite;
		text.text=""+num;
	}
	
}
