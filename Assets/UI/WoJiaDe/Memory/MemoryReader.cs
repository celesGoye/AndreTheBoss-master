using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class MemoryReader
{
	private string path = "/Resources/MemoryData.xml";

    public XmlDocument xmlDoc;
	
	public void ReadFile()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + path);
    }
	
	public string GetNormalMemoryData(int level)
	{
		string data;
		string xpath="/memory/normal/level["+level+"]";
        XmlElement node = (XmlElement)xmlDoc.SelectSingleNode(xpath);
        if(node == null)
        {
            Debug.Log("On MemoryReader: level" + level + " not found");
            return null;
        }
		data=node["content"].InnerXml;
		return data;
	}
	
	public string GetSpecialMemoryData(int kind)//0 start,1 trueend, 2 badend
	{
		string data;
		string xpath="memory";
		switch(kind)
		{
			case 0:
				xpath+="/start";
				break;
			case 1:
				xpath+="/trueend";
				break;
			case 2:
				xpath+="/badend";
				break;
			default:
				return null;
		}
		XmlElement node = (XmlElement)xmlDoc.SelectSingleNode(xpath);
		if(node == null)
        {
            Debug.Log("On MemoryReader: " + kind + " not found");
            return null;
        }
		data=node["content"].InnerXml;
		return data;
	}
	
	public string GetEventMemoryData(int index)
	{
		string data;
		string xpath="/memory/memoryevent/["+index+"]";
        XmlElement node = (XmlElement)xmlDoc.SelectSingleNode(xpath);
        if(node == null)
        {
            Debug.Log("On MemoryReader: index" + index + " not found");
            return null;
        }
		data=node["content"].InnerXml;
		return data;
	}
}
