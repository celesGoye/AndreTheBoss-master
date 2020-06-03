using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public abstract class Monster: Pawn
{
    public MonsterType monsterType;

    public int defaultSkill; // level 1 skill
	public int equippedSkill; //3 or 5
	
	public int remainedStep;
	public ActionType actionType;
	
    Dictionary<int, string> skillsAndPassives;

    // a contorl boolean for monster action, turn it off after [swichskill | attack | doskill]
    public bool CanDoAction;

    public GameManager gm;
    public PawnAction pawnAction;

    public void InitializeMonster(MonsterType monsterType, string name, int level,
        int attack, int magicAttack, int defense, int magicDefense, int HP, int dexterity, int attackRange)
    {
        this.monsterType = monsterType;
        Name = monsterType.ToString();
        InitializePawn(PawnType.Monster, name, level, attack, magicAttack, defense, magicDefense, HP, dexterity, attackRange);

        defaultSkill = 1;
		equippedSkill = 3;

        skillsAndPassives = new Dictionary<int, string>();
        ReadSkillNames(this);

        CanDoAction = true;

        gm = FindObjectOfType<GameManager>();
        pawnAction = gm.gameInteraction.pawnActionPanel;
    }

    private static void ReadSkillNames(Monster monster)
    {
        //TODO: Read skill names to skillsAndPassives;
        
    }

    public int SetEquippedSkill(int which)
    {
        if(which <= GetLevel() && (which == 3 || which == 5))
        {
            equippedSkill = which;

            // TODO: UI update stuff
            return which;
        }
        return 0;    // not success
    }
	
	public int GetEquippedSkill()
	{
		return equippedSkill;
	}
	
	public void SwitchSkill()
	{
		if(this.GetLevel()!=5)
			return;

		equippedSkill=(equippedSkill==3)?5:3;

        // TODO: UI updating

        CanDoAction = false;
	}

    // Skills to be overrided in child classes
    public virtual void DoSkillOne(Pawn other = null) { }

    public virtual void DoSkillOneCell(HexCell cell = null) { }
	
    public virtual void DoSkillThreeCell(HexCell cell = null) { }

    public virtual void DoSkillFiveCell(HexCell cell = null) { }
	
    public virtual void DoSkillThree(Pawn other = null) { }

    public virtual void DoSkillFive(Pawn other = null) { }

    public virtual void DoPassiveTwo(Pawn other = null) { }

    public virtual void DoPassiveFour(Pawn other = null) { }

    public virtual void PrepareSkillOne() 
    {
        pawnAction.requirePawnSelection = true;
        gm.hexMap.ProbeAttackTarget(currentCell);
        gm.hexMap.ShowAttackCandidates();
    }
    public virtual void PrepareSkillThree() 
    {
        pawnAction.requirePawnSelection = true;
        gm.hexMap.ProbeAttackTarget(currentCell);
        gm.hexMap.ShowAttackCandidates();
    }
    public virtual void PrepareSkillFive() 
    {
        pawnAction.requirePawnSelection = true;
        gm.hexMap.ProbeAttackTarget(currentCell);
        gm.hexMap.ShowAttackCandidates();
    }

    public override string ToString()
    {
        switch(this.monsterType)
        {
            case MonsterType.boss:
                return "Andre The Boss";
            case MonsterType.zombie:
                return "Zombie";
            case MonsterType.sprite:
                return "Sprite";
            case MonsterType.druid:
                return "Druid";
            case MonsterType.dwarf:
                return "Dwarf";
            case MonsterType.giant:
                return "Giant";
            case MonsterType.ghoul:
                return "Ghoul";
            case MonsterType.stoneman:
                return "Stoneman";
            case MonsterType.goblin:
                return "Goblin";
            case MonsterType.bloodseeker:
                return "Bloodseeker";
            case MonsterType.chimera:
                return "Chimera";
            case MonsterType.bugbear:
                return "Bugbear";
            case MonsterType.drow:
                return "Drow";
            case MonsterType.centaur:
                return "Centaur";
            case MonsterType.mindflayer:
                return "Mind Flayer";
            case MonsterType.dragon:
                return "Dragon";
            default:
                return "?Monster?";
        }
    }

    public void UpdateMonster()
    {
        UpdatePawn();
    }

    public override void OnActionBegin()
    {
        CanDoAction = true;
        base.OnActionBegin();
    }

    public override void OnActionEnd()
    {
        CanDoAction = false;
        base.OnActionEnd();
    }

	 public void Upgrade()
    {
        if (GetLevel() == 5)
            return;

        CharacterReader.CharacterData olddata = gm.characterReader.GetMonsterData(
            gm.monsterManager.GetMonsterUnlockLevel(this.monsterType), this.monsterType.ToString(), level);
        CharacterReader.CharacterData data = gm.characterReader.GetMonsterData(
            gm.monsterManager.GetMonsterUnlockLevel(this.monsterType), this.monsterType.ToString(), level + 1);

        if (data != null)
        {
            currentHP = hp = data.HP;
            currentAttack = attack - olddata.attack + data.attack;
            currentMagicAttack = magicAttack = magicAttack - olddata.magicAttack + data.magicAttack;
            currentDefense = defense - olddata.defense + data.defense;
            magicDefense = magicDefense - olddata.magicDefense + data.magicDefense;
            currentDexterity = dexterity = dexterity - olddata.dexterity + data.dexterity;
            currentAttackRange = attackRange = attackRange - olddata.attackRange + data.attackRange;
            level++;

            isDirty = true; // need to update current value with buffs
        }
        healthbar.UpdateLife();

        if (GetLevel() == 2)
            this.DoPassiveTwo();
        else if (GetLevel() == 4)
            this.DoPassiveFour();
    }
}
