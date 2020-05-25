using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoyalInquisitor : Enemy
{
    public override int TakeDamage(int damage, int magicDamage, Pawn from = null, bool isIgnoreDefense = false, bool isIgnoreMagicDefense = false)
    {
        if (isIgnoreDefense)
            damage /= 2;
        if (isIgnoreMagicDefense)
            magicDamage /= 2;

        return base.TakeDamage(damage, magicDamage, from, isIgnoreDefense, isIgnoreMagicDefense);
    }

    public override void DoSkill(Pawn target = null)
    {
        target = target != null ? target : GetCurrentTarget();
        if(target != null)
        {
            DoAttack(target);

            if (target != null)
                DoAttack();
            else
            {
                gm.hexMap.ProbeAttackTarget(currentCell);
                if(gm.hexMap.GetAttackableTargets().Count > 0)
                {
                    HexCell cell = gm.hexMap.GetAttackableTargets()[0];
                    if(cell.pawn != null)
                        DoAttack(cell.pawn);
                }
            }
        }
    }

    public override void InitPawn()
    {
        skillCounts = 1;
    }
}
