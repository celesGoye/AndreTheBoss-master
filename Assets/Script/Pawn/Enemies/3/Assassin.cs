using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Enemy
{
    public override void DoSkill(Pawn target = null)
    {
        target = target != null ? target : GetCurrentTarget();
        if(target != null)
        {
            target.TakeDamage((int)(target.GetMaxHP() * 0.3), 0, this, true);
        }
    }

    public override void InitPawn()
    {
        skillCounts = 1;
    }
}
