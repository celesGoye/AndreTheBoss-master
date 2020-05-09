using System.Collections;
using System.Collections.Generic;
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
	
	public int remainedStep;
	public ActionType actionType;
	
	public bool isDirty;	// has current data been updated?
	public Vector2 mobility;
	public bool isMagicAttackIgnoreDefense;
	public bool isPhysicAttackIgnoreDefense;
	
	public List<Vector3> buffs;		//Firstvalue:attributeType, Secondvalue:modifiedValue, thirdvalue:counter

    public string Name { get; set; }

	public static int MaxLevel=5;
	
	public HealthBar healthbar;
    public HexCell currentCell;
    public PawnType Type { get; set; }
	
    public void InitializePawn(PawnType type, string name,
    int iniattack, int inidefense, int inihp, int inidexterity, int iniattackRange , int inimagicAttack ,int inimagicDefense ,int inilevel)
    {
        Type = type;
        Name = name;
        attack =currentAttack= iniattack;
        defense = currentDefense=inidefense;
        hp = currentHP=inihp;
        dexterity = currentDexterity=inidexterity;
        attackRange = currentAttackRange=iniattackRange;
		magicAttack=currentMagicAttack=inimagicAttack;
		magicDefense=currentMagicDefense=inimagicDefense;
		level=inilevel;
    }
    public void DoAttack(Pawn other)
    {
        other.currentHP -= (currentAttack - other.currentDefense) > 0 ? currentAttack - other.currentDefense : 1;
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
		if(pawn.currentHP+hp>pawn.GetMaxHP())
			return pawn.GetMaxHP()-pawn.currentHP;
		else
			return hp;
	}
	
	public int recoverHP(Pawn pawn,int hp)
	{
		if(pawn.currentHP+hp>pawn.GetMaxHP())
			return pawn.GetMaxHP()-pawn.currentHP;
		else
			return hp;
	}
	
	public int AttackIgnoreDefense(Pawn pawn, int value){return 0;}
	public int MagicAttackIgnoreDefense(Pawn pawn, int value){return 0;}
	
	public void areaAttack(){;}
	public void areaMagicAttack(){;}
	public void areaMagicAttackIgnoreDefense(){;}
	public void areaAttackIgnoreDefense(){;}
	
	public void addBuff(AttributeType type,int modifiedvalue, int counter)
	{
		buffs.Add(new Vector3((int)type,modifiedvalue,counter));
	}
	
	public void OnDie(){;}
	public void OnActionBegin(){;}
	public void OnActionEnd(){;}
	
	public void Move(HexCell from,HexCell to)
	{
		from.pawn=null;
		to.pawn=this;
		currentCell=to;
	}
	
	public void calculateCurrentValue()
	{
		foreach(Vector3 value in buffs)
			modifyAttribute((AttributeType)value.x,(int)value.y);
		isDirty=true;
	}
	
	private void modifyAttribute(AttributeType type,int modifiedvalue)
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
	
	public void Upgrade()
	{
		CharacterReader characterReader=new CharacterReader();
		characterReader.ReadFile();
		CharacterReader.CharacterData olddata = characterReader.GetCharacterData(Type,Name,level);
		CharacterReader.CharacterData data = characterReader.GetCharacterData(Type,Name,level+1);
		if(data!=null)
		{
			currentAttack = currentAttack-olddata.attack+data.attack;
			currentDefense = currentDefense-olddata.defense+data.defense;
			currentHP = data.HP;
			hp= data.HP;
			currentDexterity = currentDexterity-olddata.dexterity+data.dexterity;
			currentAttackRange = currentAttackRange-olddata.attackRange+data.attackRange;
			currentMagicAttack=currentMagicAttack-olddata.magicAttack+data.magicAttack;
			currentMagicDefense=currentMagicDefense-olddata.magicDefense+data.magicDefense;
		}
		level++;
		healthbar.UpdateLife();
	}
}
