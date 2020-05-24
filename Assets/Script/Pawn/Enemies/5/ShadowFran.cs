using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFran : Enemy
{
    public override int TakeDamage(int damage, int magicDamage, Pawn from = null, bool isIgnoreDefense = false, bool isIgnoreMagicDefense = false)
    {
        if (from != null)
        {
            gm.hexMap.ProbeAttackTarget(from.currentCell);
            List<HexCell> cells = gm.hexMap.GetAttackableTargets();
            if (cells.Count > 1)
                return 0;
            else
                return base.TakeDamage(damage, magicDamage, from, isIgnoreDefense, isIgnoreMagicDefense);
        }
        return base.TakeDamage(damage, magicDamage, from, isIgnoreDefense, isIgnoreMagicDefense);
    }
}
