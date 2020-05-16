using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public static Dictionary<int, int> requireSouls = new Dictionary<int, int> { { 1, 1 }, { 2, 3 }, { 3, 7 } };//(level, souls)
    public static Dictionary<int, int> produceItems = new Dictionary<int, int> { { 1, 1 }, { 2, 3 }, { 3, 5 } };//(level, item number)

    private int currentLevel; // 1,2,3
    private ItemType itemTypeProduced;

    public Building(ItemType itemTypeProduced, int initLevel)
    {
        currentLevel = Mathf.Clamp(initLevel, 1,3);
        this.itemTypeProduced = itemTypeProduced;
    }

    public int LevelUp(int souls) // return souls used
    {
        if (currentLevel == 3)
            return 0;

        int required = requireSouls[currentLevel + 1] - requireSouls[currentLevel];
        if (souls < required)
            return 0;

        currentLevel++;
        return required;
    }

    public int GetCurrentProduceNumber()
    {
        return produceItems[currentLevel];
    }
}
