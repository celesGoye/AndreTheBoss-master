using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster: Pawn
{
    public MonsterType monsterType;
    public void InitializeMonster(MonsterType monsterType, string name,
        int attack, int defense, int HP, int dexterity, int attackRange,int magicAttack , int magicDefense,int level)
    {
        this.monsterType = monsterType;
        Name = monsterType.ToString();
        InitializePawn(PawnType.Monster, name, attack, defense, HP, dexterity, attackRange,magicAttack,magicDefense,level);
    }

    public override string ToString()
    {
        switch(this.monsterType)
        {
            case MonsterType.boss:
                return "Boss";
            case MonsterType.dwarf:
                return "Dwarf";
            case MonsterType.giant:
                return "Giant";
            case MonsterType.sprite:
                return "Sprite";
            case MonsterType.zombie:
                return "Zombie";
            default:
                return "?Monster?";
        }
    }

}
