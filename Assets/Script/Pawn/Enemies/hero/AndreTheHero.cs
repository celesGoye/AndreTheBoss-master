using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndreTheHero : Enemy
{
    public override void DoSkill(int skillid, Pawn target = null)
    {
        switch (skillid)
        {
            case 0:
                DoSkillZero();
                break;
            case 1:
                DoSkillOne();
                break;
        }

    }

    public void DoSkillZero()
    {
        gm.hexMap.ProbeAttackTarget(currentCell);
        foreach (HexCell cell in gm.hexMap.GetAttackableTargets())
        {
            if (cell.pawn != null)
            {
                cell.pawn.TakeDamage((int)(currentHP * .5), 0, this);
            }
        }
    }

    public void DoSkillOne()
    {
        gm.hexMap.ProbeAttackTarget(currentCell);
        foreach (HexCell cell in gm.hexMap.GetFriendTargets())
        {
            if (cell.pawn != null)
            {
                recoverHPPercentage(cell.pawn, .4f);
            }
        }
    }

    public override int TakeDamage(int damage, int magicDamage, Pawn from = null, bool isIgnoreDefense = false, bool isIgnoreMagicDefense = false)
    {
        if (isIgnoreDefense)
            damage = 0;
        if (isIgnoreMagicDefense)
            magicDamage = 0;

        damage = base.TakeDamage(damage, magicDamage, from, isIgnoreDefense, isIgnoreMagicDefense);

        return damage;
    }

    public override void InitPawn()
    {
        skillCounts = 2;
        this.isIgnoreDefense = this.isIgnoreMagicDefense = true;
    }
	
	public override void OnDie()
    {
        gm.PlayerWinPanel.gameObject.SetActive(true);
        base.OnDie();
    }
}
