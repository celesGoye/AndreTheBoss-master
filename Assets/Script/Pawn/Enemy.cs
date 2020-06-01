using Boo.Lang.Environments;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : Pawn
{
    public EnemyType enemyType;
    
    public void InitializeEnemy(EnemyType enemyType, string name, int level, int skillCounts,
    int attack,int magicAttack, int defense, int magicDefense, int HP, int dexterity, int attackRange, int dropsoul)
    {
        this.enemyType = enemyType;
        Name = enemyType.ToString();
        InitializePawn(PawnType.Enemy, name,level, attack,magicAttack, defense,magicDefense, HP, dexterity, attackRange);
        
        this.skillCounts = skillCounts;
        this.dropsoul = dropsoul;
    }

    public override string ToString()
    {
        switch(this.enemyType)
        {
            case EnemyType.magicapprentice:
                return "Magic\nApprentice";
            case EnemyType.thief:
                return "Thief";
            case EnemyType.wanderingswordman:
                return "Wandering\nSwordman";
            default:
                return "?Warrior?";
        }
    }

    public Pawn currentTarget = null;  // attack target
    public enum ActionType{ Attack, Skill, Move, Patrol, None};
    public ActionType nextAction = ActionType.None;

    public GameManager gm;

    public int skillCounts;
    public int dropsoul;

    public bool IsMoving = false;
    public List<HexCell> routes = new List<HexCell>();
    private int routePtr = 0;
    private int movespeed = 10;
    private bool isAction = false;

    public Pawn GetCurrentTarget() { return currentTarget; }

    public void OnEnable()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public override void OnActionBegin()
    {
        base.OnActionBegin();
        gm.gameCamera.FocusOnPoint(currentCell.transform.position);
        isAction = true;
        ProbeAction();
        DoAction();
    }

    public bool IsAction() { return isAction; }

    public virtual void ProbeAction()
    {
        gm.hexMap.ProbeAttackTarget(currentCell);
        List<HexCell> targets = gm.hexMap.GetAttackableTargets();

        if (currentTarget != null && targets.Count != 0)
        {
            if(targets.Contains(currentTarget.currentCell))
            {
                float posibility = (float)UnityEngine.Random.Range(0, 1+skillCounts);
                nextAction = posibility < 1 ? ActionType.Attack : ActionType.Skill;
            }
            else
            {
                HexCell cell = gm.hexMap.GetNearestAttackableTarget(currentCell);
                if (cell != null)
                {
                    currentTarget = cell.pawn;
                    nextAction = ActionType.Move;
                }
                else
                {
                    currentTarget = null;
                    nextAction = ActionType.Patrol;
                }
            }
        }
        else
        {
            HexCell cell = gm.hexMap.GetNearestAttackableTarget(currentCell);
            if (cell != null)
            {
                currentTarget = cell.pawn;
                nextAction = ActionType.Move;
            }
            else
            {
                nextAction = ActionType.Patrol;
            }
        }

    }

    public virtual void DoAction()
    {
        ProbeAction();
        switch(nextAction)
        {
            case ActionType.Attack:
                DoAttack();
                break;
            case ActionType.Move:
                DoMove();
                break;
            case ActionType.Skill:
                DoSkill();
                break;
            case ActionType.Patrol:
                DoPatrol();
                break;
            case ActionType.None:
            default:
                break;
        }
    }

    public virtual void DoAttack()
    {
        if(currentTarget != null)
        {
            gm.hexMap.HideIndicator();
            currentTarget.currentCell.indicator.gameObject.SetActive(true);
            currentTarget.currentCell.indicator.SetColor(Indicator.AttackColor);
            currentCell.indicator.gameObject.SetActive(true);
            currentCell.indicator.SetColor(Indicator.StartColor);

            ((Pawn)this).DoAttack(currentTarget);
            gm.gameInteraction.pawnActionPanel.uilog.UpdateLog(this.Name + " attacks " + currentTarget.Name);
           // gm.hexMap.HideIndicator();
        }
    }

    public virtual void DoMove()
    {
        routes = gm.hexMap.GetRoutes(this, currentTarget.currentCell);
        if(routes.Count > 0)
        {
            IsMoving = true;
            routePtr = 0;
        }
    }

    public virtual void DoPatrol()
    {
        for (int i = 0; i < 10; i++)
        {
            int dir = UnityEngine.Random.Range(0, 6);
            HexCell cell = currentCell.GetNeighbour((HexDirection)dir);
            if(cell.CanbeDestination())
            {
                routes.Clear();
                routes.Add(cell);
                routePtr = 0;
                IsMoving = true;
                break;
            }
        }
    }

    public virtual void DoSkill(Pawn target = null)
    {
        int skillid = UnityEngine.Random.Range(0, skillCounts);
        DoSkill(skillid, currentTarget);
        //Debug.Log(((Pawn)this).ToString() + " do skill");
    }

    public virtual void DoSkill(int skillid, Pawn target = null)
    {

    }

    public void Update()
    {
        // Moving animation
        if(gm.gameTurnManager.IsEnemyTurn() && IsMoving)
        {
            if(routePtr >= 0 && routePtr < routes.Count)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, routes[routePtr].transform.position, Time.deltaTime * movespeed);
                if(Vector3.Distance(this.transform.position, routes[routePtr].transform.position) < 0.01f)
                {
                    //gm.hexMap.RevealCell(routes[routePtr++]); 
                    routePtr++;
                }
            }
            else if(routePtr == routes.Count)
            {
                gm.hexMap.SetCharacterCell(this, routes[routes.Count - 1]);
                //gm.hexMap.RevealCell(routes[routes.Count - 1]);
                IsMoving = false;
            }
            else
            {
                IsMoving = false;
            }
        }
        else
        {
            isAction = false;
        }
    }

    public override void OnDie()
    {
        if(this.pawnType == PawnType.Enemy)
        {
            gm.enemyManager.setDeadEnemyType(this.enemyType);
            gm.enemyManager.RemoveEnemyPawn(this);
            gm.itemManager.GetItem(ItemType.Soul, dropsoul);
        }
        else
        {
            gm.monsterManager.RemoveRevivedEnemy(this);
        }
        base.OnDie();
    }
}
