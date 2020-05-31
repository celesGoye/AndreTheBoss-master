using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindFlayer : Monster
{
    bool isDoPassive2, isDoPassive4;
    bool isHarden = false;

    public MindFlayer()
    {
        isDoPassive2 = isDoPassive4 = false;
    }


    public override void PrepareSkillOne()
    {
        pawnAction.DoSkill();
    }
    public override void DoSkillOne(Pawn other = null)
    {
        isHarden = true;
    }

    public override void PrepareSkillThree()
    {
        pawnAction.DoSkill();
    }
    public override void DoSkillThree(Pawn other = null)
    {
        addBuff(AttributeType.Dexertiry, 5, 1);
    }

    public override void PrepareSkillFive()
    {
        pawnAction.DoSkill();
    }
    public override void DoSkillFive(Pawn other = null)
    {
        for(HexDirection i = HexDirection.NE; i <= HexDirection.NW; i++)
        {
            HexCell cell = currentCell.GetNeighbour(i);
            if(CanbeTarget(cell))
            {
                cell.pawn.TakeDamage(3, 0, this, true);
            }
        }
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        if (isDoPassive2)
            return;

        isDoPassive2 = true;
        modifyAttribute(AttributeType.Dexertiry, 1);
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        if (isDoPassive4)
            return;

        isDoPassive4 = true;
        modifyAttribute(AttributeType.Dexertiry, 2);
    }

    public override int TakeDamage(int damage, int magicDamage, Pawn from = null, bool isIgnoreDefense = false, bool isIgnoreMagicDefense = false)
    {
        if(isHarden)
        {
            return 0;
        }
        return base.TakeDamage(damage, magicDamage, from, isIgnoreDefense, isIgnoreMagicDefense);
    }

    public override void OnActionBegin()
    {
        isHarden = false;
        base.OnActionBegin();
    }
}
