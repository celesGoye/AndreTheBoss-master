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
	
	public bool isUIupdated;
	
	public bool isIgnoreDefense;
	public bool isIgnoreMagicDefense;
	
	public List<Vector3> buffs;		//Firstvalue:attributeType, Secondvalue:modifiedValue, thirdvalue:counter

    public string Name { get; set; }

	public static int MaxLevel=5;
	
	public Animator animator;
	public HealthBar healthbar;
    public HexCell currentCell;
    public PawnType pawnType { get; set; }
	
	public UILog uilog;
	
	
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
		animator=this.transform.GetChild(0).GetComponent<Animator>();
		if(animator==null)
		{
			UnityEngine.Debug.Log("animator==null!");
		}
		
		uilog= FindObjectOfType<GameManager>().GetComponent<GameManager>().gameInteraction.uilog;
		
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
		else
		{
			PlayTakeDamage(damage + magicDamage);
		}
		
		if(this.pawnType==PawnType.Monster)
			uilog.UpdateLog("<color="+TextColor.BlueColor+">"+this.Name+"</color> take damage by<color="+TextColor.RedColor+"> "+(damage+magicDamage)+"</color>");
		else if(this.pawnType==PawnType.Enemy)
			uilog.UpdateLog("<color="+TextColor.GreyColor+">"+this.Name+" take damage by <color="+TextColor.RedColor+"> "+(damage+magicDamage)+"</color></color>");
		
		return damage + magicDamage;
	}
	
	public void LifeChange(int change,Pawn pawn)
	{
		pawn.currentHP+=change;
		//pawn.healthbar.UpdateLife();
		isUIupdated=false;
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
		
		if(other.pawnType==PawnType.Monster)
			uilog.UpdateLog("<color="+TextColor.BlueColor+">"+other.Name+"</color> recovered <color="+TextColor.GreenColor+"> "+ret+"</color> hp");
		else if(other.pawnType==PawnType.Enemy)
			uilog.UpdateLog("<color="+TextColor.GreyColor+">"+other.Name+"  recovered <color="+TextColor.GreenColor+"> "+ret+"</color> hp</color>");
		
		PlayRecover(ret);
		
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
		
		if(other.pawnType==PawnType.Monster)
			uilog.UpdateLog("<color="+TextColor.BlueColor+">"+other.Name+"</color> recovered <color="+TextColor.GreenColor+"> "+ret+"</color> hp");
		else if(other.pawnType==PawnType.Enemy)
			uilog.UpdateLog("<color="+TextColor.GreyColor+">"+other.Name+"  recovered <color="+TextColor.GreenColor+"> "+ret+"</color> hp</color>");
		
		PlayRecover(ret);
		
		return ret;
	}
	
	public void addBuff(AttributeType type,int modifiedvalue, int counter)
	{
		buffs.Add(new Vector3((int)type,modifiedvalue,counter));
		isDirty = true;
		
		switch((int)type)
		{
			case(int)AttributeType.Attack:
			case(int)AttributeType.MagicAttack:
			case(int)AttributeType.Defense:
			case(int)AttributeType.MagicDefense:
			case(int)AttributeType.Dexertiry:
			case(int)AttributeType.AttackRange:
				if(this.pawnType==PawnType.Monster)
					uilog.UpdateLog("<color=" +TextColor.BlueColor+">"+this.Name+"</color>'s "+type.ToString()+(modifiedvalue>0?" increased ":" decreadsed ")+modifiedvalue+" points in "+counter +"turn");
				else if(this.pawnType==PawnType.Enemy)
					uilog.UpdateLog("<color="+TextColor.GreyColor+">"+this.Name+"'s "+type.ToString()+(modifiedvalue>0?" increased ":" decreadsed ")+modifiedvalue+" points in "+counter +"turn</color>");
				break;
			case(int)AttributeType.Mobility:
				if(this.pawnType==PawnType.Monster)
					uilog.UpdateLog("<color="+TextColor.BlueColor+">"+this.Name+"</color> can't move for "+counter +" turn");
				else if(this.pawnType==PawnType.Enemy)
					uilog.UpdateLog("<color="+TextColor.GreyColor+">"+this.Name+" can't move for "+counter +" turn</color>");
				break;
			default:
				break;
		}
	}

	public void addSkipCounter(int turnToSkip)
	{
		skipCounter += turnToSkip;
	}

	public virtual void OnDie() 
	{
		currentCell.pawn = null;
		if (healthbar != null)
		{
			GameManager gm = FindObjectOfType<GameManager>();
			gm.healthbarManager.RemoveHealthBar(healthbar);
		}
		if(animator!=null)
		{
			PlayDie();
		}
		else
		{
			GameObject.Destroy(gameObject);
		}
		
		if(this.pawnType==PawnType.Monster)
			uilog.UpdateLog("<color="+TextColor.BlueColor+">"+this.Name+"</color><color="+TextColor.RedColor+"> die</color>");
		else if(this.pawnType==PawnType.Enemy)
			uilog.UpdateLog("<color="+TextColor.GreyColor+">"+this.Name+"</color><color="+TextColor.RedColor+"> die</color>");
	}

	public virtual void OnActionBegin()
	{
		UpdatePawn();
	}
	
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
					
				case AttributeType.Mobility:
					currentDexterity=0;
					if(this as Monster!=null)
						((Monster)this).remainedStep=0;
					break;
				case AttributeType.HP:
					recoverHP(this,modifiedvalue);
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
		
		isUIupdated=false;
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
	
	public void PlayTakeDamage(int value)
	{
		if(animator!=null)
		{
			animator.SetBool("TakeDamage",true);
		}
		healthbar.OnDamageAnim(value);
		FindObjectOfType<GameManager>().gameCamera.GetComponent<Animator>().SetBool("Begin",true);
	}
	
	public void PlayDie()
	{
		if(animator!=null)
		{
			//UnityEngine.Debug.Log("die");
			animator.SetBool("Die",true);
		}
	}
	
	public void PlayRecover(int value)
	{
		if(animator!=null)
		{
			animator.SetBool("Recover",true);
		}
		healthbar.OnRecoverAnim(value);
	}
	
	public void PlayAttack()
	{
		if(animator!=null)
		{
			animator.SetBool("Attack",true);
		}
	}
}
