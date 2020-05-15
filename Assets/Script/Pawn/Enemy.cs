using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Pawn
{
    public EnemyType enemyType;
    public void InitializeEnemy(EnemyType enemyType, string name,int level,
    int attack,int magicAttack, int defense, int magicDefense, int HP, int dexterity, int attackRange )
    {
        this.enemyType = enemyType;
        Name = enemyType.ToString();
        InitializePawn(PawnType.Enemy, name,level, attack,magicAttack, defense,magicDefense, HP, dexterity, attackRange);
    }

    public override string ToString()
    {
        switch(this.enemyType)
        {
            case EnemyType.magicapprentice:
                return "Magic\nApprentice";
            case EnemyType.thief:
                return "Thief";
            case EnemyType.wanderingswordman:
                return "Wandering\nSwordman";
            default:
                return "?Warrior?";
        }
    }
}
