using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Drawf : Monster
{
    bool isDoPassive2, isDoPassive4;
    int skilloneDamage = 5;

    public Drawf()
    {
        isDoPassive2 = isDoPassive4 = false;
    }
    public override void DoSkillOne(Pawn other = null)
    {
        other.TakeDamage(skilloneDamage, 0, this, true);
    }

    public override void PrepareSkillThree()
    {
        gm.hexMap.HideIndicator();
        for (HexDirection i = HexDirection.NE; i <= HexDirection.NW; i++)
        {
            HexCell cell = currentCell.GetNeighbour(i);
            if (cell != null && cell.CanbeAttackTargetOf(currentCell))
            {
                cell.indicator.gameObject.SetActive(true);
                cell.indicator.SetColor(Indicator.AttackColor);
            }    
        }
		        // maybe a button for confirmation from player?
        pawnAction.DoSkill();
    }

    public override void DoSkillThree(Pawn other = null) 
    {
        List<Pawn> pawns = new List<Pawn>();
        for (HexDirection i = HexDirection.NE; i <= HexDirection.NW; i++)
        {
            HexCell cell = currentCell.GetNeighbour(i);
            if (cell != null && cell.CanbeAttackTargetOf(currentCell))
                pawns.Add(cell.pawn);
        }
        foreach(Pawn pawn in pawns)
        {
            pawn.TakeDamage(4, 0, this);
        }
    }

    public override void PrepareSkillFive()
    {
        pawnAction.DoSkill();
    }

    public override void DoSkillFive(Pawn other = null) 
    {
        this.addBuff(AttributeType.Defense, 10, 1);
        this.addBuff(AttributeType.MagicDefense, 10, 1);
    }

    public override void DoPassiveTwo(Pawn other = null) 
    {
        if (isDoPassive2)
            return;

        isDoPassive2 = true;
        skilloneDamage += 2;
    }

    public override void DoPassiveFour(Pawn other = null) 
    {
        if (isDoPassive4)
            return;

        isDoPassive4 = true;
        modifyAttribute(AttributeType.Attack, 2);
        UpdateCurrentValue();
    }
}
