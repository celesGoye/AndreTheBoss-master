using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JinjaMiko : Enemy
{
    public override void OnActionEnd()
    {
        recoverHPPercentage(this, 20);
    }

    public override void DoSkill(Pawn target = null)
    {
        target = target != null ? target : GetCurrentTarget();
        if(target != null)
        {
            target.TakeDamage(0, 14, this, false, true);
        }
    }
}
