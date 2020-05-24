using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public abstract class Monster: Pawn
{
    public MonsterType monsterType;

    public int currentSkill; // either 1, 3, 5 not exceed current level
	public int equippedSkill; //3, 5
	
	public int remainedStep;
	public ActionType actionType;
	
    Dictionary<int, string> skillsAndPassives;

    public void InitializeMonster(MonsterType monsterType, string name, int level,
        int attack, int magicAttack, int defense, int magicDefense, int HP, int dexterity, int attackRange)
    {
        this.monsterType = monsterType;
        Name = monsterType.ToString();
        InitializePawn(PawnType.Monster, name, level, attack, magicAttack, defense, magicDefense, HP, dexterity, attackRange);

        currentSkill = 1;
		equippedSkill=3;

        skillsAndPassives = new Dictionary<int, string>();
        ReadSkillNames(this);
    }

    private static void ReadSkillNames(Monster monster)
    {
        //TODO: Read skill names to skillsAndPassives;
        
    }

    public int SetCurrentSkill(int which)
    {
        if(which < this.GetLevel() && which % 2 == 1)
        {
            currentSkill = which;

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
	}

    // Skills to be overrided in child classes
    public virtual void DoSkillOne(Pawn other = null) { }

    public virtual void DoSkillOne(HexCell cell = null) { }

    public virtual void DoSkillThree(Pawn other = null) { }

    public virtual void DoSkillFive(Pawn other = null) { }

    public virtual void DoPassiveTwo(Pawn other = null) { }

    public virtual void DoPassiveFour(Pawn other = null) { }

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
                return "Tide Hunter";
            case MonsterType.chimera:
                return "Hague Beast";
            case MonsterType.bugbear:
                return "Bear Warrior";
            case MonsterType.drow:
                return "Taran Spider";
            case MonsterType.centaur:
                return "Halfman Rhino";
            case MonsterType.mindflayer:
                return "Murloc Leader";
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


}
