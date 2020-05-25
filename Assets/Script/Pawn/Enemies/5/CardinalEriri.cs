using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardinalEriri : Enemy
{
    public override void DoSkill(Pawn target = null)
    {
        target = target != null ? target : GetCurrentTarget();
        if(target != null)
        {
            currentHP -= 5;
            addBuff(AttributeType.MagicAttack, 10, 0);
            isDirty = true;
            DoAttack(target);
            if (currentHP <= 0)
                OnDie();
        }
    }

    public override void InitPawn()
    {
        skillCounts = 1;
    }

}
