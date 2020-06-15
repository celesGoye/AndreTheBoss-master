using System.Collections;
using UnityEngine;

public class GameEventHelper
{
    public static AttributeType getAttributeTypeFromString(string attributeType)
    {
        AttributeType ret = AttributeType.NUM;
        if (attributeType == "attack")
            ret = AttributeType.Attack;
        else if (attributeType == "magicattack")
            ret = AttributeType.MagicAttack;
        else if (attributeType == "defense")
            ret = AttributeType.Defense;
        else if (attributeType == "magicdefense")
            ret = AttributeType.MagicDefense;
        else if (attributeType == "dexterity")
            ret = AttributeType.Dexertiry;
        else if (attributeType == "attackrange")
            ret = AttributeType.AttackRange;
        else if (attributeType == "mobility")
            ret = AttributeType.Mobility;
        else if (attributeType == "hp")
            ret = AttributeType.HP;

        return ret;
    }

    public static ItemPrimaryType getItemPrimaryTypeFromString(string itemPrimaryType)
    {
        ItemPrimaryType ret = ItemPrimaryType.NUM;

        if (itemPrimaryType == "soultype")
            ret = ItemPrimaryType.SoulType;
        else if (itemPrimaryType == "farm")
            ret = ItemPrimaryType.Farm;
        else if (itemPrimaryType == "mine")
            ret = ItemPrimaryType.Mine;
        else if (itemPrimaryType == "buff")
            ret = ItemPrimaryType.Buff;

        return ret;
    }

    public static ItemType getItemTypeFromString(ItemPrimaryType primaryType, string itemType)
    {
        ItemType ret = ItemType.NUM;
        switch(primaryType)
        {
            case ItemPrimaryType.Buff:
                if (itemType == "redcandy")
                    ret = ItemType.RedCandy;
                else if (itemType == "bluecandy")
                    ret = ItemType.BlueCandy;
                else if (itemType == "purplecandy")
                    ret = ItemType.PurpleCandy;
                else if (itemType == "blackcandy")
                    ret = ItemType.BlackCandy;
                else if (itemType == "springwater")
                    ret = ItemType.SpringWater;
                break;
            case ItemPrimaryType.Farm:
                if (itemType == "spiderlily")
                    ret = ItemType.Spiderlily;
                else if (itemType == "demonfruit")
                    ret = ItemType.DemonFruit;
                else if (itemType == "goldenapple")
                    ret = ItemType.GoldenApple;
                break;
            case ItemPrimaryType.Mine:
                if (itemType == "sulphur")
                    ret = ItemType.Sulphur;
                else if (itemType == "mercury")
                    ret = ItemType.Mercury;
                else if (itemType == "gold")
                    ret = ItemType.Gold;
                break;
            case ItemPrimaryType.SoulType:
                if (itemType == "soul")
                    ret = ItemType.Soul;
                /*else if (itemType == "herosoul")
                    ret = ItemType.HeroSoul;*/
                else if (itemType == "moonlightstone")
                    ret = ItemType.MoonlightStone;
				
				else if(itemType=="robinhood")
					ret=ItemType.RobinhoodSoul;
				else if(itemType=="tatenoyousya")
					ret=ItemType.TatenoyousyaSoul;
				else if(itemType=="jinjyamiko")
					ret=ItemType.JinjyamikoSoul;
				else if(itemType=="orchestraleader")
					ret=ItemType.OrchestraleaderSoul;
				else if(itemType=="cinderlord")
					ret=ItemType.CinderlordSoul;
                break;
            default:
                break;
        }

        return ret;
    }

    public static MonsterType getMonsterTypeFromString(string type)
    {
        MonsterType ret = MonsterType.NUM;

        for(int i = 1; i < (int)MonsterType.NUM; i++)
        {
            if (((MonsterType)i).ToString().Equals(type))
            {
                ret = (MonsterType)i;
                break;
            }
        }

        return ret;
    }
}
