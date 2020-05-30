using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrchestraLeader : Enemy
{
    public override void DoSkill(Pawn target = null)
    {
        foreach(Enemy enemy in gm.enemyManager.getCurrentEnemies())
        {
            enemy.recoverHPPercentage(enemy, 30);
        }
    }

    public override void InitPawn()
    {
        isIgnoreMagicDefense = true;
        skillCounts = 1;
    }
}
