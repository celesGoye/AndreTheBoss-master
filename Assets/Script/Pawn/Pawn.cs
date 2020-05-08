using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public int Dexterity { get; set; }
    public int Level { get; set; }
    public int HP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int AttackRange { get; set; }
	public int Magic{get;set;}
	public int Resistance{get;set;}
    public string Name { get; set; }
	
	public int MaxHp{ get; set; }
	public HealthBar Healthbar;
	
	public static int MaxLevel=5;
	
    public HexCell currentCell;
    public PawnType Type { get; set; }
    public void InitializePawn(PawnType type, string name,
    int attack, int defense, int hp, int dexterity, int attackRange , int magic ,int resistance)
    {
        Type = type;
        Name = name;
        Attack = attack;
        Defense = defense;
        HP = hp;
        Dexterity = dexterity;
        AttackRange = attackRange;
		Magic=magic;
		Resistance=resistance;
		MaxHp=hp;
		
    }
    public void DoAttack(Pawn other)
    {
        other.HP -= (Attack - other.Defense) > 0 ? Attack - other.Defense : 1;
    }
	
	public void LifeChange(int change,Pawn pawn)
	{
		pawn.HP+=change;
		pawn.Healthbar.UpdateLife();
	}
}
