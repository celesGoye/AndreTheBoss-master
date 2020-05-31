using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Ghoul : Monster
{
    bool isReflect = false;
    bool isDoPassiveTwo, isDoPassiveFour;

    public Ghoul()
    {
        isDoPassiveFour = isDoPassiveTwo = false;
    }

    public override void DoSkillOne(Pawn other = null)
    {
        UpdateCurrentValue();
        int damage = this.currentAttack + this.currentMagicAttack;
        other.TakeDamage(damage, 0, this, true);
    }

    public override void PrepareSkillThree()
    {
        pawnAction.DoSkill();
    }

    public override void DoSkillThree(Pawn other = null)
    {
        isReflect = true;
    }

    public override void PrepareSkillFive()
    {
        pawnAction.requirePawnSelection = true;
        gm.hexMap.ProbeAttackTarget(currentCell);
        gm.hexMap.ShowFriendCandidates();
    }

    public override void DoSkillFive(Pawn other = null)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if(other != null && other != this && !other.currentCell.CanbeAttackTargetOf(this.currentCell))
            gm.hexMap.SwapPawns(this, other);
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        // boss gain 6 souls after this unit dies
        isDoPassiveTwo = true;
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        // boss gain 10 souls after this unit dies
        isDoPassiveFour = true;
    }

    public override int TakeDamage(int damage, int magicDamage, Pawn from = null, bool isIgnoreDefense = false, bool isIgnoreMagicDefense = false)
    { 
        if(isReflect)
        {
            if(from != null)
            {
                return from.TakeDamage(damage, magicDamage, this, isIgnoreDefense, isIgnoreMagicDefense);
            }
        }
        return base.TakeDamage(damage, magicDamage, from, isIgnoreDefense, isIgnoreMagicDefense);
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
            // Boss gain soul: 16
        }
        else if (this.GetLevel() >= 2)
        {
            // Boss gain soul: 6
        }

        base.OnDie();
    }


}
