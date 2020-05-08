using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_Itemend : MonoBehaviour
{
	public ItemType itemType;
	
	public Text name;
	public Text intro;
	public Text use;
	//public Text access;
	public Image image;
	
	private ItemReader reader;
	private Item item;
	public void UpdateItem()
	{
		name.text=itemType.ToString();
		
		reader=new ItemReader();
		reader.ReadFile();
		item=reader.GetItemData(itemType);
		
		image.sprite=item.sprite;
		intro.text="<size=22>"+item.Intro.Trim()+"</size>";
		use.text="<size=22>"+item.Use.Trim()+"</size>";
	}
}
