using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicApprentice : Enemy
{
    public override void DoSkill(Pawn target = null)
    {
        addBuff(AttributeType.Defense, 1, 1);
    }
}
