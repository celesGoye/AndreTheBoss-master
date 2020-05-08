using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    public void InitializeBoss(MonsterType monsterType, string name,
    int attack, int defense, int HP, int dexterity, int attackRange ,int magic , int resistance)
    {
        InitializeMonster(MonsterType.boss, name, attack, defense, HP, dexterity, attackRange , magic , resistance);
    }

    public override string ToString()
    {
        return "Andre The Boss";
    }
}
