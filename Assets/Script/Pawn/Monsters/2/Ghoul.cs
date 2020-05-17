using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Ghoul : Monster
{
    bool isReflect = false;

    public override void DoSkillOne(Pawn other = null)
    {
        UpdateCurrentValue();
        int damage = this.currentAttack + this.currentMagicAttack;
        other.TakeDamage(damage, 0, this, true);
    }

    public override void DoSkillThree(Pawn other = null)
    {
        isReflect = true;
    }

    public override void DoSkillFive(Pawn other = null)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if(other != null && other != this && !other.currentCell.CanbeAttackTargetOf(this.currentCell))
            gm.hexMap.SwapPawns(this, other);
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        // boss gain 6 souls
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        
        // boss gain 10 souls
    }

    public override void TakeDamage(int damage, int magicDamage, Pawn from = null, bool isIgnoreDefense = false, bool isIgnoreMagicDefense = false)
    { 
        if(isReflect)
        {
            if(from != null)
            {
                from.TakeDamage(damage, magicDamage, this, isIgnoreDefense, isIgnoreMagicDefense);
            }
        }
        else
            base.TakeDamage(damage, magicDamage, from, isIgnoreDefense, isIgnoreMagicDefense);
    }

    public override void OnActionBegin()
    {
        isReflect = false;
        base.OnActionBegin();
    }

    public override void OnDie()
    {
        if (this.GetLevel() >= 4)
        {
            DoPassiveFour();
            DoPassiveTwo();
        }
        else if (this.GetLevel() >= 2)
            DoPassiveTwo();

        base.OnDie();
    }


}
