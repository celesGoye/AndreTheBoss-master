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
	public Text type;
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
		type.text=item.itemPrimaryType.ToString();
		intro.text=item.Intro.Trim()+"\n";
		use.text=item.Use.Trim()+"\n"+item.Access.Trim()+"\n";
	}
}
