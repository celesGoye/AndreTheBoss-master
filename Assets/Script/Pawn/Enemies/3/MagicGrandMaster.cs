using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicGrandMaster : Enemy
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
        Pawn target = pawn != null ? pawn : GetCurrentTarget();
        if(target != null)
        {
            target.TakeDamage(0, 10, this, false, true);
        }
    }

    public void DoSkillOne()
    {
        addBuff(AttributeType.Defense, 3, 1);
    }

    public override void InitPawn()
    {
        skillCounts = 2;
    }
}
