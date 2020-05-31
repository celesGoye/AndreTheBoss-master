using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventOption
{
    public string name;
    public List<ItemEntry> items;
    public List<BuffEntry> buffs;

    public List<ItemEntry> costs; // canbe null
    public string result;   // canbe null

    public GameEventOption(string name, List<ItemEntry> items, List<BuffEntry> buffs, List<ItemEntry> costs, string result)
    {
        this.name = name;
        this.items = items;
        this.buffs = buffs;
        this.costs = costs;
        this.result = result;
    }

    public override string ToString()
    {
        string ret = "\nOption " + name;

        if (items != null)
        {
            ret += "\nItems";

            foreach (ItemEntry entry in items)
            {
                ret += "\n" + entry.ToString();
            }
        }
        if (buffs != null)
        {
            ret += "\nBuffs";
            foreach (BuffEntry entry in buffs)
            {
                ret += "\n" + entry.ToString();
            }
        }
        if(costs != null)
        {
            ret += "\nCosts";
            foreach (ItemEntry entry in costs)
            {
                ret += "\n" + entry.ToString();
            }
        }
        if(result != null)
        {
            ret += "\nResult: " + result;
        }

        return ret;
    }
}
