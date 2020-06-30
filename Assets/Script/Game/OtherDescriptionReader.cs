using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class OtherDescriptionReader
{
    private string pathBuilding = "/Resources/BuildingDescription.xml";
    private string pathTerrain = "/Resources/TerrainDescription.xml";
	
	public XmlDocument xmlDocBuilding;
    public XmlDocument xmlDocTerrain;
	
	public class OtherData
	{
		public string description;
		public string effect;
	};
	
	public OtherDescriptionReader()
    {
        xmlDocBuilding = new XmlDocument();
        xmlDocBuilding.Load(Application.dataPath + pathBuilding);
        xmlDocTerrain = new XmlDocument();
        xmlDocTerrain.Load(Application.dataPath + pathTerrain);
    }
	
	public OtherData GetBuildingData(BuildingType building)
    {
        // TODO: parser here
        OtherData data = new OtherData();

        string xpath = "/building/"+building.ToString();

        XmlElement node = (XmlElement)xmlDocBuilding.SelectSingleNode(xpath);

        if (node == null)
        {
            Debug.Log("On OtherDescriptionReader GetBuildingData: " + building.ToString() + " not found");
            return null;
        }

        data.description = node["description"].InnerXml;
        data.effect = node["effect"].InnerXml;

        return data;
    }
	
		
	public OtherData GetTerrainData(HexType terrain)
    {
        // TODO: parser here
        OtherData data = new OtherData();

        string xpath = "/terrain/"+terrain.ToString();

        XmlElement node = (XmlElement)xmlDocTerrain.SelectSingleNode(xpath);

        if (node == null)
        {
            Debug.Log("On OtherDescriptionReader GetTerrainData: " + terrain.ToString() + " not found");
            return null;
        }

        data.description = node["description"].InnerXml;
        data.effect = node["effect"].InnerXml;

        return data;
    }
}
