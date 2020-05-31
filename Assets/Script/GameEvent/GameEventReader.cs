using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using UnityEditor.Experimental.GraphView;

public class GameEventReader
{
    XmlDocument xmlDoc;
    static string docPath = "/Resources/EventsData.xml";
    static string[] eventxpath =
    {
        "/events/normalEvents/nonoptionsEvents/gainBuffEvents/event",
        "/events/normalEvents/nonoptionsEvents/gainItemsEvents/event",
        "/events/normalEvents/optionsEvents/event",
        "/events/mysteryPersonEvents/gainItemsEvents/event",
        "/events/mysteryPersonEvents/gainCharacterEvents/event",
    };

    private int totalEventNum;
    private int[] eventNums;

    public GameEventReader()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + docPath);
        if (xmlDoc != null)
            calculateEventNumber();
        else
            Debug.Log("On EventReader: Cannot read in events");
    }

    private void calculateEventNumber()
    {
        totalEventNum = 0;
        if (xmlDoc != null)
        {
            eventNums = new int[5];
            int i = 0;
            foreach (string xpath in eventxpath)
            {
                eventNums[i] = xmlDoc.SelectNodes(xpath).Count;
                totalEventNum += eventNums[i++];
            }
        }
    }

    public int getTotalEventsNumber()
    {
        return totalEventNum;
    }

    public int getTotalEventNumOfType(int type)    // [0, totalEventType)
    {
        if (type < 0 || type >= (int)GameEventType.GameEventNum)
            return 0;

        return eventNums[type];
    }

    public void TestGameEvent()
    {
        GameEvent ge = null;
        for(int i = 0; i < eventNums.Length; i++)
        {
            for (int j = 0; j < eventNums[i]; j++)
            {
                ge = getNewGameEvent((GameEventType)i, j);
            }
        }
    }

    public GameEvent getNewGameEvent(GameEventType type, int which)
    {
        if (which < 0 || which >= eventNums[(int)type])
            return null;

        GameEvent gameEvent = null;
        string eventName = "";
        string eventDescription = "";
        int eventCounter = 0;

        if (xmlDoc != null)
        {
            string xpath = eventxpath[(int)type];
            XmlElement node = (XmlElement)xmlDoc.SelectNodes(xpath)[which];
            eventName = node["name"].InnerXml;
            eventDescription = node["description"].InnerXml;
            eventCounter = int.Parse(node["counter"].InnerXml);
        }
        else
        {
            Debug.Log("EventReader not load correctly");
            return null;
        }

        switch(type)
        {
            case GameEventType.NormalNonoptionGainbuffEvent:
                try
                {
                    XmlElement node = (XmlElement)xmlDoc.SelectNodes(eventxpath[(int)type])[which];

                    XmlElement buffnode = (XmlElement)node.SelectSingleNode("buff");
                    int counter = int.Parse(buffnode["counter"].InnerXml);
                    int value = int.Parse(buffnode["value"].InnerXml);

                    string attributeType = buffnode["attribute"].InnerXml;
                    

                    string effectDescription = node["effectDescription"].InnerXml;
                    BuffEntry entry = new BuffEntry(GameEventHelper.getAttributeTypeFromString(attributeType),
                        counter, value);
                    gameEvent = new NormalNonoptionGainbuffEvent(eventName, eventDescription, eventCounter, effectDescription,
                        entry);
                    //Debug.Log(gameEvent.ToString());
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.StackTrace);
                }
                break;
            case GameEventType.NormalNonoptionGainitemsEvent:
                try
                {
                    XmlElement node = (XmlElement)xmlDoc.SelectNodes(eventxpath[(int)type])[which];
                    string effectDescription = node["effectDescription"].InnerXml;
                    XmlNodeList itemnodes = node.SelectNodes("item");
                    //Debug.Log("itemnodes: " + itemnodes.Count);
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
                    gameEvent = new NormalNonoptionGainitemsEvent(eventName, eventDescription, eventCounter, effectDescription,
                        items);
                    //Debug.Log(gameEvent.ToString());
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.StackTrace);
                }
                break;
            case GameEventType.NormalOptionEvent:
                try
                {
                    XmlNodeList optionnodes = xmlDoc.SelectNodes(eventxpath[(int)type])[which].SelectNodes("options/option");
                    //Debug.Log(optionnodes.Count);
                    List<GameEventOption> options = new List<GameEventOption>();
                    for(int i = 0; i < optionnodes.Count; i++)
                    {
                        XmlElement node = (XmlElement)optionnodes[i];
                        string name = node["name"].InnerXml;
                        XmlNodeList itemnodes = node.SelectNodes("item");
                        XmlNodeList buffnodes = node.SelectNodes("buff");

                        List<ItemEntry> items = itemnodes.Count>0?new List<ItemEntry>():null;
                        List<BuffEntry> buffs = buffnodes.Count>0?new List<BuffEntry>():null;

                        //Debug.Log("Items: " + itemnodes.Count + "\t\tBuffs: " + buffnodes.Count);
                        
                        for(int j = 0; j < itemnodes.Count; j++)
                        {
                            XmlElement item = (XmlElement)itemnodes[j];
                            ItemEntry itementry = new ItemEntry();
                            itementry.primaryType = GameEventHelper.getItemPrimaryTypeFromString(item["type"].InnerXml);
                            itementry.itemType = GameEventHelper.getItemTypeFromString(itementry.primaryType, item["typeEnum"].InnerXml);
                            itementry.number = int.Parse(item["number"].InnerXml);
                            itementry.posibility = int.Parse(item["posibility"].InnerXml);

                            items.Add(itementry);
                        }

                        for(int j = 0; j < buffnodes.Count; j++)
                        {
                            XmlElement buff = (XmlElement)buffnodes[j];
                            BuffEntry buffentry = new BuffEntry(GameEventHelper.getAttributeTypeFromString(buff["attribute"].InnerXml),
                                int.Parse(buff["counter"].InnerXml), int.Parse(buff["value"].InnerXml));
                            buffs.Add(buffentry);
                        }

                        GameEventOption option = new GameEventOption(name, items, buffs, null, null);
                        options.Add(option);
                    }
                    gameEvent = new NormalOptionEvent(eventName, eventDescription, eventCounter,
                            options);
                    //Debug.Log(gameEvent.ToString());
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.StackTrace);
                }
                
                break;
            case GameEventType.MysterypersonGainitemsEvent:
                try
                {
                    XmlNodeList optionnodes = xmlDoc.SelectNodes(eventxpath[(int)type])[which].SelectNodes("options/option");
                    //Debug.Log(optionnodes.Count);
                    List<GameEventOption> options = new List<GameEventOption>();
                    for(int i = 0; i < optionnodes.Count; i++)
                    {
                        XmlElement node = (XmlElement)optionnodes[i];
                        string name = node["name"].InnerXml;
                        string result = node["result"].InnerXml??null;
                        //Debug.Log("event " + which + " " + name + result);
                        XmlNodeList itemnodes = node.SelectNodes("item");
                        XmlNodeList buffnodes = node.SelectNodes("buff");
                        XmlNodeList costnodes = node.SelectNodes("cost/item");

                        List<ItemEntry> items = itemnodes.Count > 0 ? new List<ItemEntry>() : null;
                        List<BuffEntry> buffs = buffnodes.Count > 0 ? new List<BuffEntry>() : null;
                        List<ItemEntry> costs = costnodes.Count > 0?new List<ItemEntry>():null;

                        //Debug.Log("Items: " + itemnodes.Count + "\t\tBuffs: " + buffnodes.Count);

                        for (int j = 0; j < itemnodes.Count; j++)
                        {
                            XmlElement item = (XmlElement)itemnodes[j];
                            ItemEntry itementry = new ItemEntry();
                            itementry.primaryType = GameEventHelper.getItemPrimaryTypeFromString(item["type"].InnerXml);
                            itementry.itemType = GameEventHelper.getItemTypeFromString(itementry.primaryType, item["typeEnum"].InnerXml);
                            itementry.number = int.Parse(item["number"].InnerXml);
                            itementry.posibility = int.Parse(item["posibility"].InnerXml);

                            items.Add(itementry);
                        }

                        for (int j = 0; j < buffnodes.Count; j++)
                        {
                            XmlElement buff = (XmlElement)buffnodes[j];
                            BuffEntry buffentry = new BuffEntry(GameEventHelper.getAttributeTypeFromString(buff["attribute"].InnerXml),
                                int.Parse(buff["counter"].InnerXml), int.Parse(buff["value"].InnerXml));
                            buffs.Add(buffentry);
                        }

                        for(int j = 0; j < costnodes.Count;j++)
                        {
                            XmlElement cost = (XmlElement)costnodes[j];
                            ItemEntry costentry = new ItemEntry();
                            costentry.primaryType = GameEventHelper.getItemPrimaryTypeFromString(cost["type"].InnerXml);
                            costentry.itemType = GameEventHelper.getItemTypeFromString(costentry.primaryType, cost["typeEnum"].InnerXml);
                            costentry.number = int.Parse(cost["number"].InnerXml);
                            costentry.posibility = 100;

                            costs.Add(costentry);
                        }

                        GameEventOption option = new GameEventOption(name, items, buffs, costs, result);
                        options.Add(option);
                    }
                    gameEvent = new MysterypersonGainitemsEvent(eventName, eventDescription, eventCounter, options);
                    //Debug.Log(gameEvent.ToString());
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.StackTrace);
                }
                break;
            case GameEventType.MysterypersonGaincharacterEvent:
                try
                {
                    XmlNode eventnode = xmlDoc.SelectNodes(eventxpath[(int)type])[which];
                    XmlNodeList optionnodes = eventnode.SelectNodes("options/option");

                    MonsterType monsterType = GameEventHelper.getMonsterTypeFromString(eventnode["character"].InnerXml);
                    int level = int.Parse(eventnode["level"].InnerXml);

                    List<GameEventOption> options = new List<GameEventOption>();

                    for (int i = 0; i < optionnodes.Count; i++)
                    {
                        XmlElement node = (XmlElement)optionnodes[i];
                        string name = node["name"].InnerXml;
                        string result = node["result"].InnerXml ?? null;
                        //Debug.Log("event " + which + " " + name + result);
                        XmlNodeList itemnodes = node.SelectNodes("item");
                        XmlNodeList buffnodes = node.SelectNodes("buff");
                        XmlNodeList costnodes = node.SelectNodes("cost/item");

                        List<ItemEntry> items = itemnodes.Count > 0 ? new List<ItemEntry>() : null;
                        List<BuffEntry> buffs = buffnodes.Count > 0 ? new List<BuffEntry>() : null;
                        List<ItemEntry> costs = costnodes.Count > 0 ? new List<ItemEntry>() : null;

                        //Debug.Log("Items: " + itemnodes.Count + "\t\tBuffs: " + buffnodes.Count);

                        for (int j = 0; j < itemnodes.Count; j++)
                        {
                            XmlElement item = (XmlElement)itemnodes[j];
                            ItemEntry itementry = new ItemEntry();
                            itementry.primaryType = GameEventHelper.getItemPrimaryTypeFromString(item["type"].InnerXml);
                            itementry.itemType = GameEventHelper.getItemTypeFromString(itementry.primaryType, item["typeEnum"].InnerXml);
                            itementry.number = int.Parse(item["number"].InnerXml);
                            itementry.posibility = int.Parse(item["posibility"].InnerXml);

                            items.Add(itementry);
                        }

                        for (int j = 0; j < buffnodes.Count; j++)
                        {
                            XmlElement buff = (XmlElement)buffnodes[j];
                            BuffEntry buffentry = new BuffEntry(GameEventHelper.getAttributeTypeFromString(buff["attribute"].InnerXml),
                                int.Parse(buff["counter"].InnerXml), int.Parse(buff["value"].InnerXml));
                            buffs.Add(buffentry);
                        }

                        for (int j = 0; j < costnodes.Count; j++)
                        {
                            XmlElement cost = (XmlElement)costnodes[j];
                            ItemEntry costentry = new ItemEntry();
                            costentry.primaryType = GameEventHelper.getItemPrimaryTypeFromString(cost["type"].InnerXml);
                            costentry.itemType = GameEventHelper.getItemTypeFromString(costentry.primaryType, cost["typeEnum"].InnerXml);
                            costentry.number = int.Parse(cost["number"].InnerXml);
                            costentry.posibility = 100;

                            costs.Add(costentry);
                        }

                        GameEventOption option = new GameEventOption(name, items, buffs, costs, result);
                        options.Add(option);
                    }

                    gameEvent = new MysterypersonGaincharacterEvent(eventName, eventDescription, eventCounter,
                        monsterType, level, options);
                    //Debug.Log(gameEvent.ToString());
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.StackTrace);
                }
                
                break;
            default:
                break;
        }

        return gameEvent;
    }

}
