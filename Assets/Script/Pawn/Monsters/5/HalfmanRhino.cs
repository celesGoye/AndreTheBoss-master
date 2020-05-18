using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfmanRhino : Monster
{
    bool isDoPassive2, isDoPassive4;

    public HalfmanRhino()
    {
        isDoPassive2 = isDoPassive4 = false;
    }
    public override void DoSkillOne(Pawn other = null)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        for (HexDirection i = HexDirection.NE; i <= HexDirection.NW; i++)
        {
            HexCell cell = currentCell.GetNeighbour(i);
            if(cell.CanbeAttackTargetOf(currentCell))
            {
                gm.hexMap.SetCharacterCell(cell.pawn, cell.GetNeighbour(i));
            }
        }
    }

    public override void DoSkillThree(Pawn other = null)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if(CanbeTarget(other))
        {
            DoAttack(other);
            for(HexDirection i = HexDirection.NE; i <= HexDirection.NW; i++)
            {
                if(currentCell.GetNeighbour(i) == other.currentCell)
                {
                    gm.hexMap.SetCharacterCell(other, other.currentCell.GetNeighbour(i));
                }
            }
        }
    }

    public override void DoSkillFive(Pawn other = null)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        bool oldIsIgnoreDefense = isIgnoreDefense;
        isIgnoreDefense = true;
        for (HexDirection i = HexDirection.NE; i <= HexDirection.NW; i++)
        {
            HexCell cell = currentCell.GetNeighbour(i);
            if (cell.CanbeAttackTargetOf(currentCell))
            {
                DoAttack(cell.pawn);
            }
        }
        isIgnoreDefense = oldIsIgnoreDefense;
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        if (isDoPassive2)
            return;

        isDoPassive2 = true;
        modifyAttribute(AttributeType.Attack, 1);
    }

    public override void DoPassiveFour(Pawn other = null)
    {
        if (isDoPassive4)
            return;

        isDoPassive4 = true;
        modifyAttribute(AttributeType.Attack, 2);
    }
}
