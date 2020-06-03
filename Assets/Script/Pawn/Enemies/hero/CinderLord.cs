using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinderLord : Enemy
{

    bool isReflect = false;
    public override void DoSkill(int skillid, Pawn target = null)
    {
        switch(skillid)
        {
            case 0:
                DoSkillZero();
                break;
            case 1:
                DoSkillOne();
                break;
        }

    }

    public void DoSkillZero()
    {
        gm.hexMap.ProbeAttackTarget(currentCell);
        foreach(HexCell cell in gm.hexMap.GetAttackableTargets())
        {
            if(cell.pawn != null)
            {
                cell.pawn.TakeDamage((int)(currentHP * .3), 0, this);
            }
        }
    }

    public void DoSkillOne()
    {
        isReflect = true;
    }

    public override void OnActionBegin()
    {
        isReflect = false;
    }

    public override int TakeDamage(int damage, int magicDamage, Pawn from = null, bool isIgnoreDefense = false, bool isIgnoreMagicDefense = false)
    {
        if (isIgnoreDefense)
            damage = 0;
        if (isIgnoreMagicDefense)
            magicDamage = 0;

        damage = base.TakeDamage(damage, magicDamage, from, isIgnoreDefense, isIgnoreMagicDefense);

        if(isReflect)
        {
            if (from != null)
                from.TakeDamage((int)(damage * .5), 0, this, true);
        }

        return damage;
    }

    public override void InitPawn()
    {
        skillCounts = 2;
    }
}
