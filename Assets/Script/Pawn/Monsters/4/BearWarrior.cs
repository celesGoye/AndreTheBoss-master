using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearWarrior : Monster
{
    bool isDoPassive2, isDoPassive4;

    bool isSkillFive = false;

    int extraDamage = 0;

    public BearWarrior()
    {
        isDoPassive2 = isDoPassive4 = false;
    }
    public override void DoSkillOne(Pawn other = null)
    {
        for (HexDirection i = HexDirection.NE; i <= HexDirection.NW; i++)
        {
            HexCell cell = currentCell.GetNeighbour(i);
            if(CanbeTarget(cell))
            {
                cell.pawn.TakeDamage(2+extraDamage, 0, this, true);
            }
        }
    }

    public override void DoSkillThree(Pawn other = null)
    {
        if (other == null || CanbeTarget(other.currentCell))
            return;

        UpdateCurrentValue();
        other.UpdateCurrentValue();

        int damage = currentAttack, magicDamage = currentMagicAttack;

        if (!isIgnoreDefense)
        {
            damage -= other.currentDefense;
            if (damage <= 0)
                damage = 1;
        }
        if (!isIgnoreMagicDefense)
        {
            magicDamage -= other.currentMagicDefense;
            if (magicDamage <= 0)
                magicDamage = 1;
        }

        if (other.currentHP != other.GetMaxHP())
            other.TakeDamage(5, 0, this, true);

        other.TakeDamage(damage, magicDamage, this, isIgnoreDefense, isIgnoreMagicDefense);
    }

    public override void DoSkillFive(Pawn other = null)
    {
        isSkillFive = true;
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        if (isDoPassive2)
            return;

        isDoPassive2 = true;
        extraDamage += 1;
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        if (isDoPassive4)
            return;

        isDoPassive4 = true;
        extraDamage += 2;
    }

    public override void OnActionBegin()
    {
        isSkillFive = false;
        base.OnActionBegin();
    }

    public override int TakeDamage(int damage, int magicDamage, Pawn from = null, bool isIgnoreDefense = false, bool isIgnoreMagicDefense = false)
    {
        if(isSkillFive)
        {
            int remain = 5 - damage;
            if (remain >= 0)
            {
                damage = 0;
                int remain2 = remain - magicDamage;
                if (remain2 >= 0)
                    magicDamage = 0;
                else
                    magicDamage -= remain;
            }
            else
            {
                damage -= 5;
            }
        }

        return base.TakeDamage(damage, magicDamage, from, isIgnoreDefense, isIgnoreMagicDefense);
    }
}
