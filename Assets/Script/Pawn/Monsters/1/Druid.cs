using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Druid : Monster
{

    int turnToSkip;
    bool isDoPassive2, isDoPassive4;


    public Druid()
    {
        turnToSkip = 1;
        isDoPassive2 = isDoPassive4 = false;
    }

    public override void DoSkillOne(Pawn other = null)
    {
        other.TakeDamage(4, 0, null, true);
    }

    // TODO: implementation of farm class

    public override void PrepareSkillThree()
    {
        pawnAction.requireCellSelection = true;
        gm.hexMap.ProbeAttackTarget(this.currentCell);
        gm.hexMap.ShowBuildingCandidates();
    }
	
    public override void DoSkillThreeCell(HexCell cell = null)
    {
		gm.buildingManager.BuildingAccelerate(cell.building);
    }

    public override void PrepareSkillFive()
    {
        gm.hexMap.ProbeAttackTarget(currentCell);
        gm.hexMap.ShowFriendCandidates();
        pawnAction.DoSkill();
    }

    public override void DoSkillFive(Pawn other = null)
    {
        gm.hexMap.ProbeAttackTarget(this.currentCell);
        foreach (HexCell cell in gm.hexMap.GetFriendTargets())
        {
            Pawn pawn = cell.pawn;
            if (pawn != null)
                recoverHPPercentage(pawn, 0.4f);
        }
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        if (isDoPassive2)
            return;

        isDoPassive2 = true;
        modifyAttribute(AttributeType.Dexertiry, 1);
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        if (isDoPassive4)
            return;

        isDoPassive4 = true;
        turnToSkip = 3;
    }
}
