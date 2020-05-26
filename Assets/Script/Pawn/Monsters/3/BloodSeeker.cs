using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSeeker : Monster
{
    bool isDoPassive2, isDoPassive4;
    bool isSirenShell = false;

    public BloodSeeker()
    {
        isDoPassive2 = isDoPassive4 = false;
    }
    public override void DoSkillOne(Pawn other = null)
    {
        isSirenShell = true;
    }

    public override void DoSkillThree(Pawn other = null)
    {
        DoAttack(other);
        other.TakeDamage(2, 0, this, true);
    }

    public override void DoSkillFive(Pawn other = null)
    {
        for (HexDirection i = HexDirection.NE; i <= HexDirection.NW; i++)
        {
            HexCell cell = currentCell.GetNeighbour(i);
            if(cell != null && cell.CanbeAttackTargetOf(currentCell))
            {
                cell.pawn.addSkipCounter(1);
            }
        }
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        if (isDoPassive2)
            return;

        isDoPassive2 = true;

    }

    public override void DoPassiveFour(Pawn other = null)
    {
        if (isDoPassive4)
            return;

        isDoPassive4 = true;

    }

    public override int TakeDamage(int damage, int magicDamage, Pawn from = null, bool isIgnoreDefense = false, bool isIgnoreMagicDefense = false)
    {
        if(isSirenShell)
        {
            damage -= 5;
            if (damage <= 0)
                damage = 1;
        }
        return base.TakeDamage(damage, magicDamage, from, isIgnoreDefense, isIgnoreMagicDefense);
    }

    public override void OnActionBegin()
    {
        isSirenShell = false;

        base.OnActionBegin();
    }

    public override int DoAttack(Pawn other)
    {
        UpdateCurrentValue();
        int num = 0;
        for (HexDirection i = HexDirection.NE; i <= HexDirection.NW; i++)
        {
            HexCell cell = currentCell.GetNeighbour(i);
            if (cell != null && cell.pawn != this && cell.CanbeAttackTargetOf(currentCell))
                num++;
        }
        if (num == 0)
            num = 1;

        if(GetLevel() >= 4)
        {
            currentMagicAttack *= num;
        }
        if(GetLevel() >= 2)
        {
            currentAttack *= num;
        }

        int damage = base.DoAttack(other);
        isDirty = true;
        return damage;
    }
}
