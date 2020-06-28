using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodWitch : Enemy
{
    public override int DoAttack(Pawn other)
    {
         gm.hexMap.HideIndicator();
        currentTarget.currentCell.indicator.gameObject.SetActive(true);
        currentTarget.currentCell.indicator.SetColor(Indicator.AttackColor);
        currentCell.indicator.gameObject.SetActive(true);
        currentCell.indicator.SetColor(Indicator.StartColor);

        int damage = base.DoAttack(other);
        gm.gameInteraction.pawnActionPanel.uilog.UpdateLog(this.Name + " attacks " + currentTarget.Name);

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
		SetIsAction(false);
    }

    public override void InitPawn()
    {
        skillCounts = 1;
    }
}

