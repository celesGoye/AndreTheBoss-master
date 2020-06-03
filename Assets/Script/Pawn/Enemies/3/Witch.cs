using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : Enemy
{
    public override void DoSkill(Pawn target = null)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.hexMap.ProbeAttackTarget(currentCell);
        foreach(HexCell cell in gm.hexMap.GetAttackableTargets())
        {
            if (cell.pawn != null)
                cell.pawn.TakeDamage(0, 2, this, false, true);
        }
    }

    public override void InitPawn()
    {
        skillCounts = 1;
    }
}
