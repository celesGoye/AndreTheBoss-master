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
    public Building currentBuildingTarget = null; // building target 
    public enum ActionType{ Attack, AttackBuilding,  Skill, Move, Patrol, None};
    public ActionType nextAction = ActionType.None;
	

    public GameManager gm;

    public int skillCounts;
    public int dropsoul;

    // animation control value
    public bool IsMoving = false;
    public bool IsWaiting = true;

    public List<HexCell> routes = new List<HexCell>();
    private int routePtr = 0;
    private int movespeed = 10;
    private bool isAction = false;

	private AnimatorStateInfo info;
	
    public Pawn GetCurrentTarget() { return currentTarget; }

    public void OnEnable()
    {
        gm = FindObjectOfType<GameManager>();
		if(this.transform.GetChild(0).GetComponent<Animator>()!=null)
			info=this.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
    }

    public override void OnActionBegin()
    {
        base.OnActionBegin();
        gm.gameCamera.FocusOnPoint(currentCell.transform.position);
        isAction = true;
		if(!IsWaiting)
		    DoAction();
    }

    public bool IsAction() { return isAction; }
    public void SetIsAction(bool isAction) { this.isAction = isAction; }

    public virtual void ProbeAction()
    {
        gm.hexMap.ProbeAttackTarget(currentCell);
        List<HexCell> targets = gm.hexMap.GetAttackableTargets();
        List<HexCell> buildingTargets = gm.hexMap.GetBuildingCells();

        if(currentBuildingTarget != null && buildingTargets.Count != 0)
        {
            if(buildingTargets.Contains(currentBuildingTarget.currentCell))
            {
                nextAction = ActionType.AttackBuilding;
                return;
            }
        }

        if (currentTarget != null && targets.Count != 0)
        {
            if(targets.Contains(currentTarget.currentCell))
            {
                float posibility = (float)UnityEngine.Random.Range(0, 1+skillCounts);
                nextAction = posibility <= 1 ? ActionType.Attack : ActionType.Skill;
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
            case ActionType.AttackBuilding:
                DoAttackBuilding();
                SetIsAction(false);
                break;
            case ActionType.Attack:
                DoAttack();
                SetIsAction(false);
                break;
            case ActionType.Move:
                DoMove();
                break;
            case ActionType.Skill:
                DoSkill();
                SetIsAction(false);
                break;
            case ActionType.Patrol:
                DoPatrol();
                break;
            case ActionType.None:
                SetIsAction(false);
                break;
            default:
                break;
        }
    }
	
    public virtual void DoAttackBuilding()
    {
        if (currentBuildingTarget != null)
        {
            if (isDirty)
                UpdateCurrentValue();
            currentBuildingTarget.TakeDamage(this.currentAttack + this.currentMagicAttack);
        }
    }

    public virtual void DoAttack()
    {
        if(currentTarget != null)
        {
            /*
            gm.hexMap.HideIndicator();
            currentTarget.currentCell.indicator.gameObject.SetActive(true);
            currentTarget.currentCell.indicator.SetColor(Indicator.AttackColor);
            currentCell.indicator.gameObject.SetActive(true);
            currentCell.indicator.SetColor(Indicator.StartColor);
            */

            ((Pawn)this).DoAttack(currentTarget);
            gm.gameInteraction.pawnActionPanel.uilog.UpdateLog(this.Name + " attacks " + currentTarget.Name);
           // gm.hexMap.HideIndicator();
        }
    }

    public virtual void DoMove()
    {
        routes.Clear();
        routes = gm.hexMap.GetRoutes(this, currentTarget.currentCell);
        if(routes.Count > 0)
        {
            IsMoving = true;
            routePtr = 0;
        }
        else
        {
            SetIsAction(false);
        }
        //UnityEngine.Debug.Log(Name + " - Current Cell: " + currentCell.ToString());
       // for (int i = 0; i < routes.Count; i++)
            //UnityEngine.Debug.Log("route + " + i + " : " + routes[i].ToString() + "\n");
    }

    public virtual void DoPatrol()
    {
        routes.Clear();
        for (int i = 0; i < 10; i++)
        {
            int dir = UnityEngine.Random.Range(0, 6);
            HexCell cell = currentCell.GetNeighbour((HexDirection)dir);
            if(cell.CanbeDestination())
            {
                routes.Add(cell);
                routePtr = 0;
                IsMoving = true;
                return;
            }
        }

        for (int i = 0; i < 6; i++)
        {
            HexCell cell = currentCell.GetNeighbour((HexDirection)i);
            if (cell.CanbeDestination())
            {
                routes.Add(cell);
                routePtr = 0;
                IsMoving = true;
                return;
            }
        }

        SetIsAction(false);
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
        /*
		if(gm.gameTurnManager.IsEnemyTurn() && IsWaiting )
		{
			//isWaiting=info.IsName("CreateEnemy")?true:false;
			return;
		}
        */
		
        // Moving animation
        if(gm.gameTurnManager.IsEnemyTurn() && isAction && IsMoving)
        {
            if(routePtr >= 0 && routePtr < routes.Count)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, routes[routePtr].transform.position, Time.deltaTime * movespeed);
                if(Vector3.Distance(this.transform.position, routes[routePtr].transform.position) < 0.01f)
                {
                    //gm.hexMap.RevealCell(routes[routePtr++]);
                    //gm.hexMap.SetCharacterCell(this, routes[routePtr]);
                    routePtr++;
                }
            }
            else if(routePtr == routes.Count && routes.Count > 0)
            {
                gm.hexMap.SetCharacterCell(this, routes[routes.Count - 1]);
                //gm.hexMap.RevealCell(routes[routes.Count - 1]);
                IsMoving = false;
                SetIsAction(false);
                IsWaiting = false;
            }
            else
            {
                IsMoving = false;
                SetIsAction(false);
                IsWaiting = false;
            }
        }
        else
        {
            IsWaiting = false;
        }
    }

    public override void OnDie()
    {
        if(this.pawnType == PawnType.Enemy)
        {
            gm.enemyManager.setDeadEnemyType(this.enemyType);
            gm.enemyManager.RemoveEnemyPawn(this);
            gm.itemManager.GetItem(ItemType.Soul, dropsoul);
			gm.enemyManager.GetLoot(this.enemyType);
        }
        else
        {
            gm.monsterManager.RemoveRevivedEnemy(this);
        }
        base.OnDie();
    }
}
