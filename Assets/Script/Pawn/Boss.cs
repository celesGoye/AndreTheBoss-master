using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Boss : Monster
{

    public void OnEnable()
    {
    }

    public void InitializeBoss(MonsterType monsterType, string name,
    int attack, int defense, int HP, int dexterity, int attackRange ,int magicAttack , int magicDefense,int level)
    {
        InitializeMonster(MonsterType.boss, name, attack, defense, HP, dexterity, attackRange , magicAttack , magicDefense,level);
    
    }

    public override void PrepareSkillOne() 
    {
        pawnAction.requirePawnSelection = true;
        gm.hexMap.ProbeAttackTarget(this.currentCell);
        List<HexCell> cells = gm.hexMap.GetFriendTargets();
        foreach(HexCell cell in cells)
        {
            cell.indicator.SetColor(Indicator.FriendColor);
        }
    }

    public override void DoSkillOne(Pawn other = null)
    {
        if (gm.monsterManager.IsFriendlyUnit(other))
            recoverHPPercentage(other, 0.6f);
    }

    public override void PrepareSkillThree() 
    {
        gm.hexMap.ProbeAttackTarget(this.currentCell);
        List<HexCell> targets = gm.hexMap.GetAttackableTargets();
        foreach(HexCell cell in targets)
        {
            cell.indicator.SetColor(Indicator.AttackColor);
        }
    }


    public override void DoSkillThree(Pawn other = null)
    {
        gm.hexMap.ProbeAttackTarget(this.currentCell);
        foreach(HexCell cell in gm.hexMap.GetAttackableTargets())
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


    public override void PrepareSkillFive() { }

    public override void DoSkillFive(Pawn other = null)
    {
        gm.hexMap.ProbeAttackTarget(this.currentCell);
        foreach(HexCell cell in gm.hexMap.GetFriendTargets())
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
