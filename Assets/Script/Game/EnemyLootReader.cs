using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class EnemyLootReader
{
    
    XmlDocument xmlDoc;
    static string docPath = "/Resources/EnemyLoot.xml";
	
	public EnemyLootReader()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + docPath);
    }

	public List<ItemEntry> GetItems(EnemyType type,bool isHero,int level)
	{
		string xpath="";
		if(!isHero)
			xpath="/enemy/normal/level[" + level + "]";
		else
			xpath="/enemy/hero/"+type.ToString();
		
        XmlElement node = (XmlElement)xmlDoc.SelectSingleNode(xpath);
		if (node == null)
        {
            Debug.Log("On EnemyLootReader: " + type.ToString() + " not found");
            return null;
        }
		
		XmlNodeList itemnodes = node.SelectNodes("item");
        List<ItemEntry> items = new List<ItemEntry>();
        for (int i = 0; i < itemnodes.Count; i++)
        {
            ItemEntry itemEntry = new ItemEntry();
            XmlElement itemnode = (XmlElement)itemnodes[i];
            itemEntry.primaryType = GameEventHelper.getItemPrimaryTypeFromString(itemnode["type"].InnerXml);
            itemEntry.itemType = GameEventHelper.getItemTypeFromString(itemEntry.primaryType, itemnode["typeEnum"].InnerXml);
            itemEntry.number = int.Parse(itemnode["number"].InnerXml);
            itemEntry.posibility = int.Parse(itemnode["posibility"].InnerXml);
            items.Add(itemEntry);
        }
		return items;
	}
}
