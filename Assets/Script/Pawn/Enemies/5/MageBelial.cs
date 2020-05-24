using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBelial : Enemy
{
    public override void DoSkill(int skillid, Pawn target = null)
    {
        switch(skillid)
        {
            case 0:
                DoSkillZero();
                break;
            case 1:
                DoSkillOne();
                break;
            default:
                Debug.Log("On MageBelial: invalid skill id");
                break;
        }
    }

    public void DoSkillZero(Pawn target = null)
    {
        gm.hexMap.ProbeAttackTarget(currentCell);
        foreach(HexCell cell in gm.hexMap.GetAttackableTargets())
        {
            if(cell.pawn != null)
            {
                cell.pawn.TakeDamage(0, 4, this, false, true);
            }
        }
    }

    public void DoSkillOne(Pawn target = null)
    {
        addBuff(AttributeType.Defense, 10, 1);
    }
}
