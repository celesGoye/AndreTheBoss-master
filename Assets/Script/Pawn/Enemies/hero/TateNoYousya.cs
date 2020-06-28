using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TateNoYousya : Enemy
{
    public override int TakeDamage(int damage, int magicDamage, Pawn from = null, bool isIgnoreDefense = false, bool isIgnoreMagicDefense = false)
    {
        if (isIgnoreDefense)
            damage /= 2;
        if (isIgnoreMagicDefense)
            magicDamage /= 2;
        return base.TakeDamage(damage, magicDamage, from, isIgnoreDefense, isIgnoreMagicDefense);
    }

    public override void DoSkill(Pawn target = null)
    {
        addBuff(AttributeType.Defense, 5, 1);
        addBuff(AttributeType.MagicDefense, 5, 1);
        SetIsAction(false);
    }

    public override void InitPawn()
    {
        skillCounts = 1;
    }
}
