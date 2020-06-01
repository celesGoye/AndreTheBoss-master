using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drow : Monster
{
    bool isDoPassive2, isDoPassive4;
    int skilloneDamage = 5;

    public Drow()
    {
        isDoPassive2 = isDoPassive4 = false;
    }

    public override void PrepareSkillOne()
    {
        if (isDirty)
            UpdateCurrentValue();

        pawnAction.requireCellSelection = true;
        gm.hexMap.FindReachableCells(currentCell, currentDexterity);
        gm.hexMap.ShowReachableCells();
    }
    public override void DoSkillOneCell(HexCell cell)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.hexMap.ProbeAttackTarget(this.currentCell);
        if (gm.hexMap.GetEmptyCells().Contains(cell))
            gm.hexMap.SetCharacterCell(this, cell);
    }

    public override void DoSkillThree(Pawn other = null)
    {
        if (CanbeTarget(other))
            other.addSkipCounter(1);
    }

    public override void DoSkillFive(Pawn other = null)
    {
        if(CanbeTarget(other))
        {
            int damage = DoAttack(other);
            recoverHP(this, damage / 2);
        }
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        if (isDoPassive2)
            return;

        isDoPassive2 = true;
        isIgnoreDefense = true;
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        if (isDoPassive4)
            return;

        isDoPassive4 = true;
        modifyAttribute(AttributeType.Dexertiry, 2);
        UpdateCurrentValue();
    }
}
