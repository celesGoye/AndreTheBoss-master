using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
	public int level;
	public int attack;
	public int magicAttack;
	public int defense;
	public int magicDefense;
	public int hp;
	public int dexterity;
	public int attackRange;
	
	public int currentAttack;
	public int currentMagicAttack;
	public int currentDefense;
	public int currentMagicDefense;
	public int currentHP;
	public int currentDexterity;
	public int currentAttackRange;

	public int skipCounter;
	public bool isSkip;

	
	public bool isDirty;	// has current data been updated?
	
	public bool isIgnoreDefense;
	public bool isIgnoreMagicDefense;
	
	public List<Vector3> buffs;		//Firstvalue:attributeType, Secondvalue:modifiedValue, thirdvalue:counter

    public string Name { get; set; }

	public static int MaxLevel=5;
	
	public HealthBar healthbar;
    public HexCell currentCell;
    public PawnType pawnType { get; set; }

	
    public void InitializePawn(PawnType type, string name, int initlevel,
	int initattack, int initmagicAttack, int initdefense, int initmagicDefense, int inithp, int initdexterity, int initattackRange)
    {
        pawnType = type;
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
		InitPawn();
    }

	public virtual void InitPawn() {; }		// for cub class initialization

    public virtual int DoAttack(Pawn other)	// default attack action
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

		return other.TakeDamage(damage, magicDamage, this, isIgnoreDefense, isIgnoreMagicDefense);
    }

	public virtual int TakeDamage(int damage, int magicDamage, Pawn from=null, bool isIgnoreDefense = false, bool isIgnoreMagicDefense = false)
	{
		currentHP -= damage + magicDamage;

		if(currentHP <= 0)
			OnDie();

		return damage + magicDamage;
	}
	
	public void LifeChange(int change,Pawn pawn)
	{
		pawn.currentHP+=change;
		pawn.healthbar.UpdateLife();
	}
	public int GetMaxHP() { return hp; }
	public int GetLevel() { return level; }
	public int GetAttack(){return attack;}
	public int GetMagicAttack() { return magicAttack; }
	public int GetDefense() { return defense; }
	public int GetMagicDefense() { return magicDefense; }
	public int GetDexterity() { return dexterity; }
	public int GetAttackRange() { return attackRange; }

	public int recoverHPPercentage(Pawn other, float percentage)
	{
		int hp=(int)(other.GetMaxHP()*percentage);
		int ret = 0;
		if(other.currentHP+hp>= other.GetMaxHP())
		{
			ret = other.GetMaxHP() - other.currentHP;
			other.currentHP = other.GetMaxHP();
		}
		else
		{
			ret = hp;
			other.currentHP += hp;
		}
		if(other != null)
		{
			GameManager gm = FindObjectOfType<GameManager>();
			gm.gameInteraction.pawnStatusPanel.UpdatePawnStatusPanel(other);
		}
		return ret;
	}
	
	public int recoverHP(Pawn other, int hp)
	{
		int ret = 0;
		if(other.currentHP+hp>= other.GetMaxHP())
		{
			ret = other.GetMaxHP() - other.currentHP;
			other.currentHP = other.GetMaxHP();
		}
		else
		{
			ret = hp;
			other.currentHP += hp;
		}
		if (other != null)
		{
			GameManager gm = FindObjectOfType<GameManager>();
			gm.gameInteraction.pawnStatusPanel.UpdatePawnStatusPanel(other);
		}
		return ret;
	}
	
	public void addBuff(AttributeType type,int modifiedvalue, int counter)
	{
		buffs.Add(new Vector3((int)type,modifiedvalue,counter));
		isDirty = true;
	}

	public void addSkipCounter(int turnToSkip)
	{
		skipCounter += turnToSkip;
	}

	public virtual void OnDie() 
	{
	}

	public virtual void OnActionBegin(){;}
	public virtual void OnActionEnd(){;}
	
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

	// call before turn begin
	public void UpdatePawn()
	{
		UpdateCounter();
		calculateCurrentValue();
	}

	public void UpdateCurrentValue()
	{
		if (isDirty)
			calculateCurrentValue();
	}

	// utility
	public bool CanbeTarget(HexCell cell)
	{
		return cell != null && cell.pawn != null && cell.pawn != this && cell.CanbeAttackTargetOf(currentCell);
	}

	public bool CanbeTarget(Pawn pawn)
	{
		return pawn != null && pawn != this && pawn.currentCell.CanbeAttackTargetOf(currentCell);
	}
}
