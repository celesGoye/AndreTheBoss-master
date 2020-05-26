using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodWitch : Enemy
{
    public override int DoAttack(Pawn other)
    {
        int damage = base.DoAttack(other);
        recoverHPPercentage(this, 50);
        return damage;
    }

    public override void DoSkill(Pawn target = null)
    {
        gm.hexMap.ProbeAttackTarget(currentCell);
        foreach(HexCell cell in gm.hexMap.GetAttackableTargets())
        {
            if(cell.pawn != null)
            {
                cell.pawn.TakeDamage(0, 4, this);
            }
        }
    }
}

