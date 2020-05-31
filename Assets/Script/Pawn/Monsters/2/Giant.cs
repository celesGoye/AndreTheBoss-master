using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : Monster
{
    bool isDoPassive2, isDoPassive4;
    int skillthreeTurns = 1;

    public Giant()
    {
        isDoPassive2 = isDoPassive4 = false;
    }
    public override void DoSkillOne(Pawn other = null)
    {
        other.TakeDamage(4, 0, this, true);
    }

    public override void PrepareSkillThree()
    {
        pawnAction.DoSkill();
    }

    public override void DoSkillThree(Pawn other = null)
    {
        addBuff(AttributeType.Defense, 5, skillthreeTurns);
    }

    public override void DoSkillFive(Pawn other = null)
    {
        other.TakeDamage(0, 10, this);
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        isDoPassive2 = true;
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        if (isDoPassive4)
            return;

        isDoPassive4 = true;
        skillthreeTurns = 2;
    }

    public override void OnDie()
    {
        if(isDoPassive2)
        {
            gm.hexMap.ProbeAttackTarget(currentCell);
            foreach (HexCell cell in gm.hexMap.GetAttackableTargets())
            {
                cell.pawn.TakeDamage(10, 0);
            }
        }
        base.OnDie();
    }
}
