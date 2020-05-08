using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    public string eventName;
    public string eventDescription;

    public GameEvent(string eventName, string eventDescription)
    {
        this.eventName = eventName;
        this.eventDescription = eventDescription;
    }
}

public class NormalNonoptionGainbuffEvent : GameEvent
{
    public NormalNonoptionGainbuffEvent(string eventName, string eventDescription)
        : base(eventName, eventDescription)
    {

    }
}

public class NormalNonoptionGainitemsEvent : GameEvent
{
    public NormalNonoptionGainitemsEvent(string eventName, string eventDescription)
        : base(eventName, eventDescription)
    {

    }
}

public class NormalOptionEvent : GameEvent
{
    public NormalOptionEvent(string eventName, string eventDescription)
        : base(eventName, eventDescription)
    {

    }
}

public class MysterypersonGainitemsEvent : GameEvent
{
    public MysterypersonGainitemsEvent(string eventName, string eventDescription)
        : base(eventName, eventDescription)
    {

    }
}

public class MysterypersonGaincharacterEvent : GameEvent
{
    public MysterypersonGaincharacterEvent(string eventName, string eventDescription)
        : base(eventName, eventDescription)
    {

    }
}