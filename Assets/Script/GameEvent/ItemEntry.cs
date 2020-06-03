using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntry
{
    public ItemPrimaryType primaryType;
    public ItemType itemType;
    public int number;
    public int posibility;    // XX.OO% -> posibility = XX for each item

    public ItemEntry(ItemPrimaryType primaryType, ItemType itemType, int number, int posibility)
    {
        this.primaryType = primaryType;
        this.itemType = itemType;
        this.number = number;
        this.posibility = posibility;
    }

    public ItemEntry()
    {

    }

    public override string ToString()
    {
        return "PrimaryType: " + primaryType.ToString() + "\nItemType: " +
            itemType.ToString() + "\nnumber: " + number + "\nposibility: " + posibility;
    }
}
