using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Monster
{
    bool isDoPassive2, isDoPassive4;
    

    public Goblin()
    {
        isDoPassive2 = isDoPassive4 = false;
    }
    
    public override void DoSkillOne(Pawn other = null)
    {
        other.TakeDamage(2, 0, this, true);
    }
    public override void PrepareSkillThree()
    {
        pawnAction.DoSkill();
    }
    public override void DoSkillThree(Pawn other = null)
    {
        int temp = GetMagicAttack();
        modifyAttribute(AttributeType.MagicAttack, GetAttack() - temp);
        modifyAttribute(AttributeType.Attack, temp - GetAttack());
    }

    public override void PrepareSkillFive()
    {
        pawnAction.requirePawnSelection = true;
        gm.hexMap.ProbeAttackTarget(currentCell);
        gm.hexMap.ShowFriendCandidates();
    }

    public override void DoSkillFive(Pawn other = null)
    {
        // do die stuff
        if(other != null && !other.currentCell.CanbeAttackTargetOf(this.currentCell))
        {
            other.currentHP = other.GetMaxHP();
        }

        OnDie();
    }

    public override void DoPassiveTwo(Pawn other = null)
    {
        if (isDoPassive2)
            return;

        isDoPassive2 = true;

        modifyAttribute(AttributeType.Dexertiry, 2);

    }

    public override void DoPassiveFour(Pawn other = null)
    {
        if (isDoPassive4)
            return;

        isDoPassive4 = true;
        modifyAttribute(AttributeType.Attack, 5);
    }

}
