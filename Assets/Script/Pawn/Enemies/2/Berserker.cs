using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : Enemy
{
    public override void DoSkill(Pawn target = null)
    {
        for(HexDirection i = HexDirection.NE; i <= HexDirection.NW; i++)
        {
            HexCell cell = currentCell.GetNeighbour(i);
            if(cell != null && cell.CanbeAttackTargetOf(currentCell))
            {
                cell.pawn.addSkipCounter(1);
            }
        }
    }

    public override void InitPawn()
    {
        skillCounts = 1;
    }
}
