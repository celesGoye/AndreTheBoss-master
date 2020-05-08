using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class ItemReader
{
	private string path = "/Resources/ItemData.xml";

    public XmlDocument xmlDoc;
	
	public void ReadFile()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + path);
    }
	
	public Item GetItemData(ItemType itemType)
	{
		Item item=new Item();
		string xpath="//"+itemType.ToString();
		xpath="items"+xpath;
		XmlElement node = (XmlElement)xmlDoc.SelectSingleNode(xpath);
        if(node == null)
        {
            Debug.Log("On ItemReader: " + itemType.ToString() + " not found");
            return null;
        }
		item.itemType=itemType;
		item.itemPrimaryType=(ItemPrimaryType)System.Enum.Parse(typeof(ItemPrimaryType),node.ParentNode.Name);
		item.Intro=(node["intro"].InnerXml);
		item.Use=(node["use"].InnerXml);
		item.Access=(node["access"].InnerXml);
		item.sprite=Resources.Load("UI/item/"+itemType.ToString(), typeof(Sprite)) as Sprite;
		
		return item;
	}
}
