using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameEvent
{
    public string eventName;
    public string eventDescription;
    public int counter;
	
	public GameEventType eventType;
    public int whichEvent;

    public GameEvent(string eventName, string eventDescription, int counter, GameEventType eventType, int whichEvent)
    {
        this.eventName = eventName;
        this.eventDescription = eventDescription;
        this.counter = counter;
        this.eventType = eventType;
        this.whichEvent = whichEvent;
    }
}


public class NormalNonoptionGainbuffEvent : GameEvent
{
    BuffEntry buff;
    private string effectDescription;
    public NormalNonoptionGainbuffEvent(string eventName, string eventDescription, int counter, GameEventType eventType, int whichEvent,
        string effectDescription, BuffEntry buff)
        : base(eventName, eventDescription, counter, eventType, whichEvent)
    {
        this.buff = buff;
        this.effectDescription = effectDescription;
    }
	
	public BuffEntry GetBuff()
	{
		return buff;
	}
	
	public string GetEffectDescription()
	{
		return effectDescription;
	}

    public override string ToString()
    {
        return "name: " + this.eventName + "\ndescription: " + this.eventDescription +
            "\ncounter: " + this.counter +
            "\neffectDescription: " + this.effectDescription + "\n" + buff.ToString();
    }
}

public class NormalNonoptionGainitemsEvent : GameEvent
{
    private List<ItemEntry> items;  // total posibility of all items EQUALS 100
    private string effectDescription;
    public NormalNonoptionGainitemsEvent(string eventName, string eventDescription, int counter, GameEventType eventType, int whichEvent,
        string effectDescription, List<ItemEntry> items)
        : base(eventName, eventDescription, counter, eventType, whichEvent)
    {
        this.effectDescription = effectDescription;
        this.items = items;
    }
	
	public List<ItemEntry> GetItems()
	{
		return items;
	}
	
	public string GetEffectDescription()
	{
		return effectDescription;
	}

    public override string ToString()
    {
        string ret = "name: " + this.eventName + "\ndescription: " + this.eventDescription +
            "\ncounter: " + this.counter+
            "\neffectDescription: "  + this.effectDescription + "\nItems:";
        foreach(ItemEntry entry in items)
        {
            ret += "\n\titem\n" + entry.ToString();
        }
        return ret;
    }
}

public class NormalOptionEvent : GameEvent
{
    List<GameEventOption> options;
    public NormalOptionEvent(string eventName, string eventDescription, int counter, GameEventType eventType, int whichEvent, List<GameEventOption> options)
        : base(eventName, eventDescription, counter, eventType, whichEvent)
    {
        this.options = options;
    }
	
	public List<GameEventOption> GetOptions()
	{
		return options;
	}

    public override string ToString()
    {
        string ret = "name: " + this.eventName + "\ndescription: " + this.eventDescription +
            "\ncounter: " + this.counter + "\nOptions: ";
        foreach (GameEventOption option in options)
            ret += option.ToString();

        return ret;
    }
}

public class MysterypersonGainitemsEvent : GameEvent
{
    List<GameEventOption> options;
    public MysterypersonGainitemsEvent(string eventName, string eventDescription, int counter, GameEventType eventType, int whichEvent,
        List<GameEventOption> options)
        : base(eventName, eventDescription, counter, eventType, whichEvent)
    {
        this.options = options;
    }
	
	public List<GameEventOption> GetOptions()
	{
		return options;
	}
	
    public override string ToString()
    {
        string ret = "name: " + this.eventName + "\ndescription: " + this.eventDescription +
            "\ncounter: " + this.counter + "\nOptions: ";
        foreach (GameEventOption option in options)
            ret += option.ToString();

        return ret;
    }
}

public class MysterypersonGaincharacterEvent : GameEvent
{
    MonsterType monsterType;
    int level;
    List<GameEventOption> options;
    public MysterypersonGaincharacterEvent(string eventName, string eventDescription, int counter, GameEventType eventType, int whichEvent, 
        MonsterType monsterType, int level, List<GameEventOption> options)
        : base(eventName, eventDescription, counter, eventType, whichEvent)
    {
        this.monsterType = monsterType;
        this.level = level;
        this.options = options;
    }
	
	public MonsterType GetMonsterType()
	{
		return monsterType;
	}
	
	public int GetLevel()
	{
		return level;
	}
	
	public List<GameEventOption> GetOptions()
	{
		return options;
	}

    public override string ToString()
    {
        string ret = "name: " + this.eventName + "\ndescription: " + this.eventDescription +
            "\ncounter: " + this.counter + "\nOptions: " + "\nMonster: " + monsterType.ToString() + 
            "\nLevel: " + level;
        foreach (GameEventOption option in options)
            ret += option.ToString();

        return ret;
    }
}