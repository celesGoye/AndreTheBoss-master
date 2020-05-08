using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

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

    public GameEventReader()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + docPath);
    }

    public int totalEventsNumber()
    {
        int totalEvents = 0;
        if (xmlDoc != null)
        {
            foreach (string xpath in eventxpath)
            {
                totalEvents += xmlDoc.SelectNodes(xpath).Count;
            }
        }
        Debug.Log("Total events number: " + totalEvents);
        return totalEvents;
    }

}
