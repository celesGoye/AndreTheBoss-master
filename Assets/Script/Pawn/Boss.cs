using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    private GameManager gameManager;
    
    public void OnEnable()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void InitializeBoss(MonsterType monsterType, string name,
    int attack, int defense, int HP, int dexterity, int attackRange ,int magicAttack , int magicDefense,int level)
    {
        InitializeMonster(MonsterType.boss, name, attack, defense, HP, dexterity, attackRange , magicAttack , magicDefense,level);
    
    }

    public override void DoSkillOne(Pawn other = null)
    {
        if (gameManager.monsterManager.IsFriendlyUnit(other))
            recoverHPPercentage(other, 0.6f);
    }

    public override void DoSkillThree(Pawn other = null)
    {
        gameManager.hexMap.ProbeAttackTarget(this.currentCell);
        foreach(HexCell cell in gameManager.hexMap.GetAttackableTargets())
        {
            Enemy enemy = (Enemy)cell.pawn;

            UpdateCurrentValue();
            int damage = this.currentMagicAttack + this.currentAttack;

            if(enemy != null)
            {
                enemy.TakeDamage(0, damage, this, false, true);
            }
        }
    }

    public override void DoSkillFive(Pawn other = null)
    {
        gameManager.hexMap.ProbeAttackTarget(this.currentCell);
        foreach(HexCell cell in gameManager.hexMap.GetFriendTargets())
        {
            Pawn pawn = cell.pawn;
            if(pawn != null)
                recoverHPPercentage(pawn, 0.4f);
        }
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        recoverHP(this, 2);
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        recoverHP(this, 5);
    }
}
