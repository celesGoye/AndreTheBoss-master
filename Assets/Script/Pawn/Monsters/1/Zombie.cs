using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Monster
{
    private bool isDoPassive2;
    private bool isDoPassive4;

    public Zombie()
    {
        isDoPassive2 = isDoPassive4 = false;
    }
    public override void DoSkillOne(Pawn other = null)
    {
        other.TakeDamage(5, 0, this, true);
    }

    public override void DoSkillThree(Pawn other = null)
    {
        other.TakeDamage(7, 0, this, true);
    }

    public override void DoSkillFive(Pawn other = null)
    {
        int damage = 7;
        if (other.isDirty)
            other.UpdateCurrentValue();

        if (!isIgnoreMagicDefense)
            damage -= other.currentMagicDefense;

        if (damage <= 0)
            damage = 1;

        other.TakeDamage(0, damage, this);
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        if (isDoPassive2)
            return;
        isDoPassive2 = true;

        this.modifyAttribute(AttributeType.Defense, 2);
        UpdateCurrentValue();
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        if (isDoPassive4)
            return;
        isDoPassive4 = true;

        this.modifyAttribute(AttributeType.MagicDefense, 2);
        UpdateCurrentValue();
    }
}
