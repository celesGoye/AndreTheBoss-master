using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : Enemy
{
    public override void DoSkill(Pawn target = null)
    {
        addBuff(AttributeType.Dexertiry, 4, 1);
    }


}
