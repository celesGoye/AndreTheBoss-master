using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoneman : Monster
{
    bool isDoPassive2, isDoPassive4;
    bool isHarden = false;

    public Stoneman()
    {
        isDoPassive2 = isDoPassive4 = false;
    }
    public override void DoSkillOne(Pawn other = null)
    {
        for (HexDirection i = HexDirection.NE; i <= HexDirection.NW; i++)
        {
            HexCell cell = this.currentCell.GetNeighbour(i);
            if(cell != null && cell.CanbeAttackTargetOf(this.currentCell))
            {
                cell.pawn.TakeDamage(5, 0, this);
            }
        }
    }

    public override void DoSkillThree(Pawn other = null)
    {
        isHarden = true;
    }

    public override void DoSkillFive(Pawn other = null)
    {
        this.addBuff(AttributeType.Dexertiry, 5, 1);
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        if (isDoPassive2)
            return;

        isDoPassive2 = true;
        modifyAttribute(AttributeType.Defense, 1);
        modifyAttribute(AttributeType.MagicDefense, 1);
        UpdateCurrentValue();
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        if (isDoPassive4)
            return;

        isDoPassive4 = true;
        modifyAttribute(AttributeType.Defense, 3);
        modifyAttribute(AttributeType.MagicDefense, 3);
        UpdateCurrentValue();
    }

    public override void OnActionBegin()
    {
        isHarden = false;
        base.OnActionBegin();
    }

    public override void TakeDamage(int damage, int magicDamage, Pawn from = null, bool isIgnoreDefense = false, bool isIgnoreMagicDefense = false)
    {
        if (isHarden)
            return;

        base.TakeDamage(damage, magicDamage, from, isIgnoreDefense, isIgnoreMagicDefense);
    }
}
