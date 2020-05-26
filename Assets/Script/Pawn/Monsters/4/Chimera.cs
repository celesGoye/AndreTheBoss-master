using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimera : Monster
{
    bool isDoPassive2, isDoPassive4;
    int skilloneDamage = 5;

    public Chimera()
    {
        isDoPassive2 = isDoPassive4 = false;
    }
    public override void DoSkillOne(Pawn other = null)
    {
        for (HexDirection i = HexDirection.NE; i <= HexDirection.NW; i++)
        {
            HexCell cell = currentCell.GetNeighbour(i);
            if (CanbeTarget(cell))
                cell.pawn.TakeDamage(2, 0, this, true);
        }
    }

    public override void DoSkillThree(Pawn other = null)
    {
        if(other != null && CanbeTarget(other.currentCell))
        {
            other.TakeDamage(3, 0, this, true);
            other.addSkipCounter(1);
        }
    }

    public override void DoSkillFive(Pawn other = null)
    {
        if(other != null && CanbeTarget(other.currentCell))
        {
            other.TakeDamage(7, 0, this, true);
        }
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        if (isDoPassive2)
            return;

        isDoPassive2 = true;

        modifyAttribute(AttributeType.Defense, 2);
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        if (isDoPassive4)
            return;

        isDoPassive4 = true;
        modifyAttribute(AttributeType.MagicDefense, 5);
    }
}
