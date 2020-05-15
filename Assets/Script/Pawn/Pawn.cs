using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
	private int level;
	private int attack;
	private int magicAttack;
	private int defense;
	private int magicDefense;
	private int hp;
	private int dexterity;
	private int attackRange;
	
	public int currentAttack;
	public int currentMagicAttack;
	public int currentDefense;
	public int currentMagicDefense;
	public int currentHP;
	public int currentDexterity;
	public int currentAttackRange;

	public int skipCounter;
	public bool isSkip;

	public int remainedStep;
	public ActionType actionType;
	
	public bool isDirty;	// has current data been updated?
	
	public bool isIgnoreDefense;
	public bool isIgnoreMagicDefense;
	
	public List<Vector3> buffs;		//Firstvalue:attributeType, Secondvalue:modifiedValue, thirdvalue:counter

    public string Name { get; set; }

	public static int MaxLevel=5;
	
	public HealthBar healthbar;
    public HexCell currentCell;
    public PawnType Type { get; set; }
	
    public void InitializePawn(PawnType type, string name, int initlevel,
	int initattack, int initmagicAttack, int initdefense, int initmagicDefense, int inithp, int initdexterity, int initattackRange)
    {
        Type = type;
        Name = name;
		level=initlevel;

        attack =currentAttack= initattack;
        defense = currentDefense=initdefense;
        hp = currentHP=inithp;
        dexterity = currentDexterity=initdexterity;
        attackRange = currentAttackRange=initattackRange;
		magicAttack=currentMagicAttack=initmagicAttack;
		magicDefense=currentMagicDefense=initmagicDefense;

		skipCounter = 0;
		isSkip = false;

		isIgnoreDefense = isIgnoreMagicDefense = false;
    }
    public void DoAttack(Pawn other)	// default attack action
    {
		if (isDirty)
			calculateCurrentValue();
		if (other.isDirty)
			other.calculateCurrentValue();

		int damage = currentAttack, magicDamage = currentMagicAttack;

        if(!isIgnoreDefense)
		{
			damage -= other.currentDefense;
			if (damage <= 0)
				damage = 1;
		}
		if(!isIgnoreMagicDefense)
		{
			magicDamage -= other.currentMagicDefense;
			if (magicDamage <= 0)
				magicDamage = 1;
		}

		other.TakeDamage(damage, magicDamage);
    }

	public void TakeDamage(int damage, int magicDamage)
	{
		currentHP -= damage + magicDamage;

		if(currentHP <= 0)
			OnDie();
	}
	
	public void LifeChange(int change,Pawn pawn)
	{
		pawn.currentHP+=change;
		pawn.healthbar.UpdateLife();
	}
	public int GetMaxHP()
	{
		return hp;
	}
	public int GetLevel()
	{
		return level;
	}
	
	public int recoverHPPercentage(Pawn pawn,float percentage)
	{
		int hp=(int)(pawn.GetMaxHP()*percentage);
		int ret = 0;
		if(pawn.currentHP+hp>=pawn.GetMaxHP())
		{
			ret = pawn.GetMaxHP() - pawn.currentHP;
			pawn.currentHP = pawn.GetMaxHP();
		}
		else
		{
			ret = hp;
			pawn.currentHP += hp;
		}
		return ret;
	}
	
	public int recoverHP(Pawn pawn,int hp)
	{
		int ret = 0;
		if(pawn.currentHP+hp>=pawn.GetMaxHP())
		{
			ret = pawn.GetMaxHP() - pawn.currentHP;
			pawn.currentHP = pawn.GetMaxHP();
		}
		else
		{
			ret = hp;
			pawn.currentHP += hp;
		}
		return ret;
	}
	
	public void addBuff(AttributeType type,int modifiedvalue, int counter)
	{
		buffs.Add(new Vector3((int)type,modifiedvalue,counter));
		isDirty = true;
	}

	public void OnDie() {; }
	public void OnActionBegin(){;}
	public void OnActionEnd(){;}
	
	public void Move(HexCell from,HexCell to)
	{
		if (!to.CanbeDestination())
			return;

		from.pawn=null;
		to.pawn=this;
		currentCell=to;
	}
	
	private void calculateCurrentValue()
	{
		currentAttack = attack;
		currentMagicAttack = magicAttack;
		currentDefense = defense;
		currentMagicDefense = magicDefense;
		currentDexterity = dexterity;
		currentAttackRange  = attackRange;

		foreach (Vector3 value in buffs)
			modifyCurrentAttribute((AttributeType)value.x,(int)value.y);

		isDirty=false;
	}
	
	private void modifyCurrentAttribute(AttributeType type,int modifiedvalue)
	{
		switch(type)
			{
				case AttributeType.Attack:
					currentAttack+=modifiedvalue;
					break;
				case AttributeType.MagicAttack:
					currentMagicAttack+=modifiedvalue;
					break;
				case AttributeType.Defense:
					currentDefense+=modifiedvalue;
					break;
				case AttributeType.MagicDefense:
					currentMagicDefense+=modifiedvalue;
					break;
				case AttributeType.Dexertiry:
					currentDexterity+=modifiedvalue;
					break;
				case AttributeType.AttackRange:
					currentAttackRange+=modifiedvalue;
					break;
			}
	}

	public void modifyAttribute(AttributeType type, int modifiedvalue)
	{
		switch (type)
		{
			case AttributeType.Attack:
				attack += modifiedvalue;
				break;
			case AttributeType.MagicAttack:
				magicAttack += modifiedvalue;
				break;
			case AttributeType.Defense:
				defense += modifiedvalue;
				break;
			case AttributeType.MagicDefense:
				magicDefense += modifiedvalue;
				break;
			case AttributeType.Dexertiry:
				dexterity += modifiedvalue;
				break;
			case AttributeType.AttackRange:
				attackRange += modifiedvalue;
				break;
		}
		isDirty = true;
	}
	
	public void Upgrade()
	{
		if (level == 5 || Type == PawnType.Enemy)
			return;

		GameManager gm = FindObjectOfType<GameManager>();

		Monster monster = (Monster)this;
		
		CharacterReader.CharacterData olddata = gm.characterReader.GetMonsterData(
			gm.monsterManager.GetMonsterUnlockLevel(monster.monsterType),monster.monsterType.ToString(),level);
		CharacterReader.CharacterData data = gm.characterReader.GetMonsterData(
			gm.monsterManager.GetMonsterUnlockLevel(monster.monsterType), monster.monsterType.ToString(), level+1);

		if (data!=null)
		{
			currentHP = hp= data.HP;
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
	}

	private void UpdateCounter()
	{
		// Update buff counter
		Vector3[] buffs = this.buffs.ToArray<Vector3>();
		this.buffs = new List<Vector3>();
		for (int i = 0; i < buffs.Length; i++)
		{
			Vector3 buff = buffs[i];
			if (buff.z-- > 0)
				this.buffs.Add(buff);
		}
		// Update immobility counter
		if(skipCounter > 0)
		{
			skipCounter--;
			isSkip = true;
		}
		else
		{
			isSkip = false;
		}
	}

	public void UpdatePawn()
	{
		UpdateCounter();
		calculateCurrentValue();
	}
}
