using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : Enemy
{
    public override void InitPawn()
    {
        isIgnoreDefense = isIgnoreMagicDefense = true;
        skillCounts = 1;
    }

    public override void DoSkill(Pawn target = null)
    {
        target = target != null ? target : GetCurrentTarget();

        if(target != null)
        {
            target.addBuff(AttributeType.MagicDefense, -3, 1);
        }
    }

}
