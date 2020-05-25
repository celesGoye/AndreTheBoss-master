using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bard : Enemy
{
    public override void DoSkill(Pawn target = null)
    {
        for (HexDirection i = HexDirection.NE; i < HexDirection.NW; i++)
        {
            HexCell cell = currentCell.GetNeighbour(i);
            if(cell != null && !cell.CanbeAttackTargetOf(currentCell))
            {
                recoverHPPercentage(cell.pawn, 30);
            }
            recoverHPPercentage(this, 30);
        }
    }

    public override void InitPawn()
    {
        skillCounts = 1;
    }
}
