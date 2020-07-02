using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Enemy
{
    public override void DoSkill(Pawn target = null)
    {
        /*
        target = target != null ? target : GetCurrentTarget();
        if(target != null && target.pawnType == PawnType.Building)
        {
            // destroy buildings
            //GameObject.DestroyImmediate(target.gameObject);
        }
        */

        if(currentBuildingTarget != null)
        {
            gm.buildingManager.DestroyBuilding(currentBuildingTarget);
        }

    }

    /*
    public override void ProbeAction()
    {
        if (currentTarget != null)
        {
            if (currentTarget.currentCell.DistanceTo(currentCell) <= GetAttackRange())
            {
                nextAction = ActionType.Skill;
            }
            else
            {
                nextAction = ActionType.Move;
            }
        }
        else
        {
            HexCell cell = gm.hexMap.GetNearestAttackableTarget(currentCell);
            if (cell != null)
            {
                currentTarget = cell.building.GetComponent<Pawn>();
                if (currentTarget.currentCell.DistanceTo(currentCell) <= GetAttackRange())
                {
                    nextAction = ActionType.Skill;
                }
                else
                {
                    nextAction = ActionType.Move;
                }
            }
            else
            {
                nextAction = ActionType.Patrol;
            }
        }
    }
    */

    public override void InitPawn()
    {
        skillCounts = 1;
    }
}
