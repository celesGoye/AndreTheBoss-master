using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robinhood : Enemy
{
    public override void DoSkill(int skillid, Pawn target = null)
    {
        switch(skillid)
        {
            case 0:
                DoSkillZero(target);
                break;
            case 1:
                DoSkillOne();
                break;
            default:
                Debug.Log("On Robinhood: invalid skillid");
                break;
        }
    }

    public void DoSkillZero(Pawn target = null)
    {
        target = target != null ? target : GetCurrentTarget();
        if(target != null)
        {
            target.TakeDamage(5, 0, this, true);
        }
    }

    public void DoSkillOne(Pawn target = null)
    {
        foreach(Enemy enemy in gm.enemyManager.getCurrentEnemies())
        {
            enemy.addBuff(AttributeType.Dexertiry, 2, 0);
            enemy.isDirty = true;
        }
    }

    public override void InitPawn()
    {
        skillCounts = 2;
    }
}
