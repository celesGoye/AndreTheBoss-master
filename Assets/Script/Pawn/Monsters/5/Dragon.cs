using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Dragon : Monster
{
    bool isDoPassive2, isDoPassive4;

    public Dragon()
    {
        isDoPassive2 = isDoPassive4 = false;
    }
    public override void DoSkillOne(Pawn other = null)
    {
        if(CanbeTarget(other))
        {
            other.TakeDamage(0, 10, this, false, true);
        }
    }

    public override void DoSkillThree(Pawn other = null)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.hexMap.ProbeAttackTarget(currentCell);
        foreach(HexCell cell in gm.hexMap.GetAttackableTargets())
        {
            if(CanbeTarget(cell.pawn))
            {
                cell.pawn.TakeDamage(0, 4, this, false, true);
            }
        }
    }

    public override void DoSkillFive(Pawn other = null)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.hexMap.ProbeAttackTarget(currentCell);
        foreach (HexCell cell in gm.hexMap.GetAttackableTargets())
        {
            if (CanbeTarget(cell.pawn))
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
        modifyAttribute(AttributeType.Attack, 5);
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        if (isDoPassive4)
            return;

        isDoPassive4 = true;
        modifyAttribute(AttributeType.MagicAttack, 5);
    }
}
