using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMaster : Enemy
{
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
            default:
                break;
        }
    }

    public void DoSkillZero(Pawn pawn = null)
    {
        Pawn target;
        if (pawn != null)
            target = pawn;
        else
            target = GetCurrentTarget();

        if (target != null)
        {
            target.TakeDamage(0, 7, this, false, true);
        }
    }

    public void DoSkillOne()
    {
        addBuff(AttributeType.MagicDefense, 3, 1);
    }
}
