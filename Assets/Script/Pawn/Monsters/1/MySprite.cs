using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySprite : Monster
{
    bool isDoPassive2;
    bool isDoPassive4;


    public MySprite()
    {
        isDoPassive2 = isDoPassive4 = false;
    }

    public override void DoSkillOne(Pawn other = null)
    {
        other.TakeDamage(4, 0, this, true);
    }

    public override void DoSkillThree(Pawn other = null)
    {
        other.TakeDamage(0, 7, this);
    }

    public override void DoSkillFive(Pawn other = null)
    {
        other.TakeDamage(0, 10, this);
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
        this.isIgnoreMagicDefense = true;
    }
}
