using Boo.Lang.Environments;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : Pawn
{
    public EnemyType enemyType;
    public void InitializeEnemy(EnemyType enemyType, string name, int level, int skillCounts,
    int attack,int magicAttack, int defense, int magicDefense, int HP, int dexterity, int attackRange)
    {
        this.enemyType = enemyType;
        Name = enemyType.ToString();
        InitializePawn(PawnType.Enemy, name,level, attack,magicAttack, defense,magicDefense, HP, dexterity, attackRange);
        
        this.skillCounts = skillCounts;
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

    public bool IsMoving = false;
    public List<HexCell> routes = new List<HexCell>();
    private int routePtr = 0;
    private int movespeed = 1000;

    public Pawn GetCurrentTarget() { return currentTarget; }

    public void OnEnable()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public virtual void ProbeAction()
    {
        gm.hexMap.ProbeAttackTarget(currentCell);
        List<HexCell> targets = gm.hexMap.GetAttackableTargets();

        if (currentTarget != null)
        {
            if(targets.Contains(currentTarget.currentCell))
            {
                float posibility = (float)Random.Range(0, 1+skillCounts);
                nextAction = posibility < 1 ? ActionType.Attack : ActionType.Skill;
            }
            else
            {
                nextAction = ActionType.Move;
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
            ((Pawn)this).DoAttack(currentTarget);
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
            int dir = Random.Range(0, 6);
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
        int skillid = Random.Range(0, skillCounts);
        DoSkill(skillid, target);
    }

    public virtual void DoSkill(int skillid, Pawn target = null)
    {

    }

    public void Update()
    {
        // Moving animation
        if(gm.gameTurnManager.IsEnemyTurn() && IsMoving)
        {
            if(routePtr < routes.Count)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, routes[routePtr].transform.position, Time.deltaTime * movespeed);
                if(Vector3.Distance(this.transform.position, routes[routePtr].transform.position) < 0.01f)
                {
                    routePtr++;
                }
            }
            else
            {
                gm.hexMap.SetCharacterCell(this, routes[routes.Count - 1]);
                IsMoving = false;
            }
        }
    }
}
