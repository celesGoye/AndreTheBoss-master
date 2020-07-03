using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using System;

public class PawnAction : MonoBehaviour
{
    private Pawn selectedPawn;
    public GameInteraction gameInteraction;
    public MonsterActionManager monsterActionManager;
    public MonsterManager monsterManager;
    public HexMap hexMap;
    public Text txt_pawn;

	public Transform overallVisible;
    public Transform actionPanel;
	public Button moveButton;
	public Button attackButton;
	public Button skillButton;
    public Button transferButton;
	public Button infoButton;
	public Button cancelButton;
    public AttackPanel attackPanel;

    public UILog uilog;
    public MenuControl menu;

    public float moveSpeed = 1f;

    public enum Status { PrepareAttack, PrepareMove, PrepareDoSkill, Rest, IsMoving, IsAttacking, IsDoSkill, IsTransfering };

    public Status currentStatus = Status.Rest;

    private List<HexCell> routes;
    private int currentTarget;
    private bool validRoute;

    private Pawn attackTarget;
    private bool validAttackTarget;

    public bool requirePawnSelection;      // for skill action
    public bool requireCellSelection;
    private int selectedSkill;
    private Pawn skillTarget;
    private HexCell skillTargetCell;
    private bool validSkillTarget;
	private CharacterReader characterReader;

    public void OnEnable()
    {
        currentStatus = Status.Rest;
        attackPanel.monster = (Monster)selectedPawn;
		overallVisible.gameObject.SetActive(true);
        attackPanel.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        actionPanel.gameObject.SetActive(true);
        ActiveTransferButton();
		UpdatePawnActionPanel();
		
		
		if(characterReader == null)
			characterReader = FindObjectOfType<GameManager>().GetComponent<GameManager>().characterReader;
    }


    public void SetPawn(Pawn pawn)
    {
        selectedPawn = pawn;
        //Debug.Log(selectedPawn.currentCell.GetComponent<RectTransform>() == null);
        //this.GetComponent<followGameObject>().follow = selectedPawn.GetComponent<Transform>();
    }

    public void ShowAttackPanel()
    {			
        attackPanel.gameObject.SetActive(true);
        actionPanel.gameObject.SetActive(false);
		attackPanel.UpdateAttackPanel();
    }
	
	public void OnCancel()
	{
		if(currentStatus==Status.PrepareAttack||currentStatus==Status.PrepareMove||currentStatus==Status.PrepareDoSkill)
		{
			UpdatePawnActionPanel();
			ClearStatus();
		}
		Debug.Log("cancel");
		hexMap.HideIndicator();
		cancelButton.gameObject.SetActive(false);
	}

    public void OnSkill1()  // currentSkill
    {
        PrepareDoSkill(1);
    }

    public void OnSkill2()  // equippedSkill
    {
        PrepareDoSkill(2);
    }

    public void OnSwitchSkill()
    {
        ((Monster)selectedPawn).SwitchSkill();
		monsterActionManager.MonsterAttack((Monster)selectedPawn);
        this.gameObject.SetActive(false);
    }
	
	public void ActiveTransferButton()
	{
        if (selectedPawn == null || selectedPawn.currentCell == null)
            return;

			if(selectedPawn.currentCell.building!=null
				&&selectedPawn.currentCell.building.GetBuildingType()==BuildingType.Teleporter
				&&selectedPawn.currentCell.building.GetComponent<Teleporter>().GetIsValid()
				&&selectedPawn.currentCell.building.GetComponent<Teleporter>().another.currentCell.pawn==null)
				{
					transferButton.gameObject.SetActive(true);
					transferButton.transform.GetChild(0).gameObject.SetActive(false);
					transferButton.interactable=true;
				}
			else
				transferButton.gameObject.SetActive(false);
	}
	
    public void UseTeleporter()
    {
        if (selectedPawn != null)
        {
            uilog.UpdateLog("<color="+TextColor.BlueColor+">"+selectedPawn.Name +"</color> is trying to transfer");
            //Debug.Log(selectedPawn.Name + " is trying to transfer");
            gameInteraction.SetIsPawnAction(true);
            currentStatus = Status.IsTransfering;
            UpdateRoot(selectedPawn, selectedPawn.currentCell, selectedPawn.currentCell.building.GetComponent<Teleporter>().another.currentCell);
            selectedPawn.currentCell.building.GetComponent<Teleporter>().SetIsValid(false);
            //Debug.Log("pawn use teleporter: " + selectedPawn.currentCell.building.GetComponent<Teleporter>().GetIsValid());
            ActiveTransferButton();
        }
    }
	
	public void UpdatePawnActionPanel()
	{
		try
		{
			Monster monster=(Monster)selectedPawn;
            if (monster == null)
                return;
			if(monster.actionType==ActionType.Nonactionable)
			{
				moveButton.interactable=false;
				attackButton.interactable=false;
				skillButton.interactable=false;
			}
			else if(monster.actionType==ActionType.AttackEnds)
			{
				moveButton.interactable=true;
				attackButton.interactable=false;
				skillButton.interactable=false;
			}
			else if(monster.actionType==ActionType.MoveEnds)
			{
				moveButton.interactable=false;
				attackButton.interactable=true;
				skillButton.interactable=true;
			}
			else
			{
				moveButton.interactable=true;
				attackButton.interactable=true;
				skillButton.interactable=true;
			}
			infoButton.interactable=true;
			transferButton.interactable=true;
		}
		catch (InvalidCastException ex)
        {
			 Debug.Log(ex.StackTrace);
		}
	}

    public void PrepareAttack()
    {
        if (selectedPawn != null)
        {
            //uilog.UpdateLog("<color="+TextColor.BlueColor+">"+selectedPawn.Name +"</color> is trying to attack");
            //Debug.Log(selectedPawn.Name + " is trying to attack");
            gameInteraction.SetIsPawnAction(true);
            currentStatus = Status.PrepareAttack;
            validAttackTarget = true;
            hexMap.ProbeAttackTarget(selectedPawn.currentCell);
            hexMap.ShowAttackCandidates();
			
			skillButton.interactable=false;
			moveButton.interactable=false;
			infoButton.interactable=false;
			transferButton.interactable=false;
			
			cancelButton.gameObject.SetActive(true);
        }
    }
	
	public void OnPointerEnterAttack(int which)
	{
		if(currentStatus!=Status.Rest)
			return;
		
		if(which==0)
		{
			hexMap.ProbeAttackTarget(selectedPawn.currentCell);
			hexMap.ShowReachableEnemyCells();
		}
		else
		{
			Monster monster=(Monster)selectedPawn;
			SkillTargetType type=SkillTargetType.None;
			int range=-1;
			
			if(which==1)
			{
				type=characterReader.GetMonsterSkillTargetType(monster.Name,monster.defaultSkill);
				range=characterReader.GetMonsterSkillRange(monsterManager.GetMonsterUnlockLevel(monster.monsterType),monster.Name,monster.defaultSkill,monster.level);		
			}
			else if(which==2)
			{
				type=characterReader.GetMonsterSkillTargetType(monster.Name,monster.equippedSkill);
				range=characterReader.GetMonsterSkillRange(monsterManager.GetMonsterUnlockLevel(monster.monsterType),monster.Name,monster.equippedSkill,monster.level);		
			}
			
			if(range<0)
				return;
			
			hexMap.ProbeAttackTarget(selectedPawn.currentCell,range);
			switch((int)type)
			{
				case (int)SkillTargetType.Enemy:
					hexMap.ShowReachableEnemyCells();
					break;
				case (int)SkillTargetType.Friend:
					hexMap.ShowReachableFriendCells();
					break;
				case (int)SkillTargetType.Building:
					hexMap.ShowReachableBuildingCells();
					break;
				case (int)SkillTargetType.Empty:
					hexMap.ShowReachableEmptyCells();
					break;
				case (int)SkillTargetType.Self:
					hexMap.ShowReachableFriendCells();
					break;
				default:
					break;
			}
		}
	}
	
	public void OnPointerExitAttack()
	{
		if(currentStatus==Status.Rest)
			hexMap.HideIndicator();
	}

    public void PrepareDoSkill(int which)   // 1 - defaultSkill, 2 - equippedSkill
    {
        requirePawnSelection = false;
        requireCellSelection = false;
        currentStatus = Status.PrepareDoSkill;
        validSkillTarget = false;
        gameInteraction.SetIsPawnAction(true);
		attackPanel.OnSkill(which);
        try {
            Monster monster = (Monster)selectedPawn;
            if (monster != null)
            {
                //uilog.UpdateLog("<color="+TextColor.BlueColor+">"+monster.Name +"</color> is trying to do skill");
                 gameInteraction.SetIsPawnAction(true);
                switch (which)
                {
                    case 1:
                        selectedSkill = 1;
                        monster.PrepareSkillOne();
                        break;
                    case 2:
                        int equippedSkill = monster.GetEquippedSkill();
                        if (equippedSkill == 3)
                        {
                            selectedSkill = 3;
                            monster.PrepareSkillThree();
                        }
                        else if (equippedSkill == 5)
                        {
                            selectedSkill = 5;
                            monster.PrepareSkillFive();
                        }
                        else
                        {
                            ClearStatus();
                        }
                        break;
                    default:
                        ClearStatus();
                        break;
                }
            }

        }
        catch (InvalidCastException ex)
        {
            Debug.Log(ex.StackTrace);
            try
            {
                Enemy enemy = (Enemy)selectedPawn;

            } catch (InvalidCastException ex2)
            {
                Debug.Log(ex2.StackTrace);
            }
        }
    }

    public void PrepareMove()
    {
        if (selectedPawn != null)
        {
            //uilog.UpdateLog("<color="+TextColor.BlueColor+">"+selectedPawn.Name +"</color> is trying to move");
            //Debug.Log(selectedPawn.Name + " is trying to move");
            gameInteraction.SetIsPawnAction(true);
            currentStatus = Status.PrepareMove;
            validRoute = true;
            hexMap.FindReachableCells(selectedPawn.currentCell, ((Monster)selectedPawn).remainedStep);
            hexMap.ShowReachableCells();
			
			skillButton.interactable=false;
			attackButton.interactable=false;
			infoButton.interactable=false;
			transferButton.interactable=false;
			
			cancelButton.gameObject.SetActive(true);
        }
    }

    public void Move()
    {

        if (selectedPawn.pawnType == PawnType.Monster)
        {
            monsterActionManager.SetActionType(hexMap.GetPathLength(), (Monster)selectedPawn);
        }
        uilog.UpdateLog("<color="+TextColor.BlueColor+">"+selectedPawn.Name +"</color> Moves");
        //Debug.Log(selectedPawn.Name + " Moves");
        hexMap.HideIndicator();
        currentStatus = Status.IsMoving;

        UpdateRoot(selectedPawn, selectedPawn.currentCell, routes[routes.Count - 1]);
        ActiveTransferButton();
		overallVisible.gameObject.SetActive(false);
    }

    public void Attack()
    {
        uilog.UpdateLog("<color="+TextColor.BlueColor+">"+selectedPawn.Name +"</color>  Attacks" + attackTarget);
		selectedPawn.PlayAttack();
        //Debug.Log(selectedPawn.Name + " Attacks" + attackTarget);
        hexMap.HideIndicator();
        currentStatus = Status.IsAttacking;
        selectedPawn.DoAttack(attackTarget);
		monsterActionManager.MonsterAttack((Monster)selectedPawn);
        overallVisible.gameObject.SetActive(false);
    }

    public void DoSkill()
    {
        if(requireCellSelection)
        {
            try
            {
                Monster monster = (Monster)selectedPawn;
                if (monster != null)
                {
                    switch (selectedSkill)
                    {
                        case 1:
                            uilog.UpdateLog("<color="+TextColor.BlueColor+">"+monster.Name +"</color> do skill one <color="+TextColor.OrangeColor+">"+monster.skillnames[0]+"</color>");
                            monster.DoSkillOneCell(skillTargetCell);
							selectedPawn.PlayAttack();
                            break;
                        case 3:
                            uilog.UpdateLog("<color="+TextColor.BlueColor+">"+monster.Name +"</color> do skill three <color="+TextColor.OrangeColor+">"+monster.skillnames[2]+"</color>");
                            monster.DoSkillThreeCell(skillTargetCell);
							selectedPawn.PlayAttack();
                            break;
                        case 5:
                            uilog.UpdateLog("<color="+TextColor.BlueColor+">"+monster.Name +"</color> do skill five <color="+TextColor.OrangeColor+">"+monster.skillnames[4]+"</color>");
                            monster.DoSkillFiveCell(skillTargetCell);
							selectedPawn.PlayAttack();
                            break;
                        default:
                            ClearStatus();
                            break;
                    }
                }
            }
            catch (InvalidCastException ex)
            {
                Debug.Log(ex.StackTrace);
                try
                {
                    Enemy enemy = (Enemy)selectedPawn;
                    uilog.UpdateLog("<color="+TextColor.BlueColor+">"+enemy.Name +"</color> do skill");
					selectedPawn.PlayAttack();
                    // TODO: Enemey control
                }
                catch (InvalidCastException ex2)
                {
                    Debug.Log(ex2.StackTrace);
                }
            }
            ClearStatus();
        }
        else
        {
            try
            {
                Monster monster = (Monster)selectedPawn;
                if (monster != null)
                {
                    switch (selectedSkill)
                    {
                        case 1:
                            uilog.UpdateLog("<color="+TextColor.BlueColor+">"+monster.Name +"</color> do skill one <color="+TextColor.OrangeColor+">"+monster.skillnames[0]+"</color>");
                            monster.DoSkillOne(skillTarget);
							selectedPawn.PlayAttack();
                            break;
                        case 3:
                            uilog.UpdateLog("<color="+TextColor.BlueColor+">"+monster.Name +"</color> do skill three<color="+TextColor.OrangeColor+">"+monster.skillnames[2]+"</color>");
                            monster.DoSkillThree(skillTarget);
							selectedPawn.PlayAttack();
                            break;
                        case 5:
                            uilog.UpdateLog("<color="+TextColor.BlueColor+">"+monster.Name +"</color> do skill five<color="+TextColor.OrangeColor+">"+monster.skillnames[4]+"</color>");
                            monster.DoSkillFive(skillTarget);
							selectedPawn.PlayAttack();
                            break;
                        default:
                            ClearStatus();
                            break;
                    }
                }
            }
            catch (InvalidCastException ex)
            {
                Debug.Log(ex.StackTrace);
                try
                {
                    Enemy enemy = (Enemy)selectedPawn;
                    uilog.UpdateLog("<color="+TextColor.BlueColor+">"+enemy.Name +"</color> do skill");
					selectedPawn.PlayAttack();
                }
                catch (InvalidCastException ex2)
                {
                    Debug.Log(ex2.StackTrace);
                }
            }
            ClearStatus();
        } 
		try
        {
            Monster monster = (Monster)selectedPawn;
			monsterActionManager.MonsterAttack(monster);
		}
		catch(InvalidCastException ex)
		{
			Debug.Log(ex.StackTrace);
		}
		gameInteraction.Clear();
    }

    public void Skip()
    {
        if (selectedPawn != null)
        {
            uilog.UpdateLog("<color="+TextColor.BlueColor+">"+selectedPawn.Name +"</color> is skiping turn");
            //Debug.Log(selectedPawn.Name + " is skiping turn");
        }
    }

    public void OpenMenu()
    {
        menu.SetCurrentMonster(selectedPawn);
        menu.OpenMenu();
		overallVisible.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (currentStatus == Status.Rest)
            return;
        else if (currentStatus == Status.PrepareAttack)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                UpdateAttackTarget();
                if (validAttackTarget && (hexMap.IsReachable(getCurrentPointerCell()) || hexMap.IsReachable(getCurrentPointerPawn().currentCell)))
                {
                    Attack();
                }
                else
                {
                    OnCancel();
                }
            }
        }
        else if (currentStatus == Status.PrepareMove)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                UpdateRoute();
                if (!validRoute || !hexMap.IsReachable(getCurrentPointerCell()))
                    OnCancel();
                else
                {
                    routes = hexMap.GetCurrentRoutes();
                    currentTarget = 0;
                    Move();

                }
            }
        }
        else if (currentStatus == Status.IsMoving)
        {
            selectedPawn.transform.position = Vector3.Lerp(selectedPawn.transform.position,
                routes[currentTarget].transform.position, Time.deltaTime * moveSpeed);
            float distance = Vector3.Distance(selectedPawn.transform.position, routes[currentTarget].transform.position);
            if (distance < 0.01f)
            {
                hexMap.RevealCellsFrom(routes[currentTarget]);
                if (currentTarget < routes.Count - 1)
                {
                    currentTarget++;
                }

                else
                {
                    ClearStatus();
                }
            }
        }
        else if (currentStatus == Status.IsAttacking)
        {
            //Debug.Log("Showing attack animation");
            ClearStatus();
        }
        else if (currentStatus == Status.IsTransfering)
        {
            //Debug.Log("transfering");
            selectedPawn.transform.position = selectedPawn.currentCell.transform.position;
            ClearStatus();
        }
        else if (currentStatus == Status.PrepareDoSkill)
        {
            if (requirePawnSelection)
            {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    UpdateSkillTarget();
                    if (validSkillTarget)
                    {
                        DoSkill();
                    }
                    else
                    {
                        attackPanel.OnCancel();
                    }
                }
            }
            else if (requireCellSelection)
            {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    UpdateSkillTarget();
                    if (validSkillTarget)
                    {
                        DoSkill();
                    }
                    else
                    {
                        attackPanel.OnCancel();
                    }
                }
            }
            else
            {
                currentStatus = Status.IsDoSkill;
            }
        }
        else if (currentStatus == Status.IsDoSkill)
        {
            DoSkill();
        }
    }

    private void UpdateRoute()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<HexCell>())
            {
                HexCell toCell = hit.collider.GetComponent<HexCell>();
                if (!toCell.CanbeDestination() ||
                    toCell == selectedPawn.currentCell ||
                    !hexMap.IsReachable(toCell))
                {
                    validRoute = false;
                }
                else
                {
                    validRoute = true;
                }
                hexMap.FindPath(selectedPawn.currentCell, toCell);
                //hexMap.ShowPath(selectedPawn.currentCell, toCell);
            }
        }
    }

    public void UpdateAttackTarget()
    {
        HexCell cell = getCurrentPointerCell();
        if (cell != null && cell.CanbeAttackTargetOf(selectedPawn.currentCell))
        {
            validAttackTarget = true;
            attackTarget = cell.pawn;
        }
        else
        {
            Pawn pawn = getCurrentPointerPawn();
            if (pawn != null &&pawn.currentCell.CanbeAttackTargetOf(selectedPawn.currentCell))
            {
                validAttackTarget = true;
                attackTarget = pawn;
            }
            else
            {
                validAttackTarget = false;
            }
        }
    }

    public void UpdateSkillTarget()
    {
        HexCell cell = getCurrentPointerCell();
		
        if (requireCellSelection && cell != null && hexMap.GetBuildingCells().Contains(cell))
        {
            skillTargetCell = cell;
            validSkillTarget = true;
        }

        if (cell != null && cell.pawn != null)
        {
            validSkillTarget = true;
            skillTarget = cell.pawn;
        }
        else
        {
            Pawn pawn = getCurrentPointerPawn();
            if (pawn != null&&hexMap.GetAttackableTargets().Contains(pawn.currentCell))
            {
                if(requireCellSelection)
                {
                    skillTargetCell = pawn.currentCell;
                    if (skillTargetCell != null)
                        validSkillTarget = true;
                }
                validSkillTarget = true;
                skillTarget = pawn;
            }
        }
    }


    private void ClearStatus()
    {
        hexMap.HideIndicator();
        currentStatus = Status.Rest;
        gameInteraction.SetIsPawnAction(false);
    }

    public HexCell getCurrentPointerCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<HexCell>())
            {
                return hit.collider.GetComponent<HexCell>();
            }
        }
        return null;
    }

    public Pawn getCurrentPointerPawn()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<Monster>())
            {
                return hit.collider.GetComponent<Monster>();
            }
            else if (hit.collider.GetComponent<Enemy>())
            {
                return hit.collider.GetComponent<Enemy>();
            }
        }
        return null;
    }

    public void UpdateRoot(Pawn pawn, HexCell oldCell, HexCell newCell)
    {
        pawn.currentCell = newCell;
        newCell.pawn = pawn;
        oldCell.pawn = null;
    }
	
	public void OnPointerEnter(GameObject text)
	{
		text.SetActive(true);
	}
	
	public void OnPointerExit(GameObject text)
	{
		text.SetActive(false);
	}
}
