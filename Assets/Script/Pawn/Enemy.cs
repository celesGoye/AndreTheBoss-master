using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Pawn
{
    public EnemyType enemyType;
    public void InitializeEnemy(EnemyType enemyType, string name,
    int attack, int defense, int HP, int dexterity, int attackRange,int magic , int resistance)
    {
        this.enemyType = enemyType;
        Name = enemyType.ToString();
        InitializePawn(PawnType.Enemy, name, attack, defense, HP, dexterity, attackRange,magic,resistance);
    }

    public override string ToString()
    {
        switch(this.enemyType)
        {
            case EnemyType.magicApprentice:
                return "Magic\nApprentice";
            case EnemyType.thief:
                return "Thief";
            case EnemyType.wanderingSwordman:
                return "Wandering\nSwordman";
            default:
                return "?Warrior?";
        }
    }
}
