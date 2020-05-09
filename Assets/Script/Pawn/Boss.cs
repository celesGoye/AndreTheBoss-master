using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    public void InitializeBoss(MonsterType monsterType, string name,
    int attack, int defense, int HP, int dexterity, int attackRange ,int magicAttack , int magicDefense,int level)
    {
        InitializeMonster(MonsterType.boss, name, attack, defense, HP, dexterity, attackRange , magicAttack , magicDefense,level);
    }

    public override string ToString()
    {
        return "Andre The Boss";
    }
}
