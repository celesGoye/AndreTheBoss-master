using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEntry
{
    public int counter;
    public AttributeType attributeType;
    public int value;

    public BuffEntry(AttributeType type, int counter, int value)
    {
        this.attributeType = type;
        this.counter = counter;
        this.value = value;
    }

    public BuffEntry()
    {

    }

    public override string ToString()
    {
        return "Counter: " + counter + "\nAttibuteType: " + attributeType.ToString() + "\nValue: " + value;
    }
}
