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

    public Transform actionPanel;
    public Button transferButton;
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
    private bool validSkillTarget;

    public void OnEnable()
    {
        currentStatus = Status.Rest;
        attackPanel.monster = (Monster)selectedPawn;
        attackPanel.gameObject.SetActive(false);
        actionPanel.transform.gameObject.SetActive(true);
        if (selectedPawn.currentCell.building != null
            && selectedPawn.currentCell.building.GetBuildingType() == BuildingType.Teleporter
            && selectedPawn.currentCell.building.GetComponent<Teleporter>().GetIsValid())
            transferButton.gameObject.SetActive(true);
        else
            transferButton.gameObject.SetActive(false);
    }

    public void SetPawn(Pawn pawn)
    {
        selectedPawn = pawn;
        //Debug.Log(selectedPawn.currentCell.GetComponent<RectTransform>() == null);
        this.GetComponent<followGameObject>().follow = selectedPawn.GetComponent<Transform>();
    }

    public void ShowAttackPanel()
    {
        attackPanel.gameObject.SetActive(true);
        actionPanel.transform.gameObject.SetActive(false);
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
        this.transform.gameObject.SetActive(false);
    }

    public void UseTeleporter()
    {
        if (selectedPawn != null)
        {
            uilog.UpdateLog(selectedPawn.Name + " is trying to transfer");
            //Debug.Log(selectedPawn.Name + " is trying to transfer");
            gameInteraction.IsPawnAction = true;
            currentStatus = Status.IsTransfering;
            UpdateRoot(selectedPawn, selectedPawn.currentCell, selectedPawn.currentCell.building.GetComponent<Teleporter>().another.currentCell);
            selectedPawn.currentCell.building.GetComponent<Teleporter>().SetIsValid(false);
            //Debug.Log("pawn use teleporter: " + selectedPawn.currentCell.building.GetComponent<Teleporter>().GetIsValid());
            if (selectedPawn.currentCell.building != null
                && selectedPawn.currentCell.building.GetBuildingType() == BuildingType.Teleporter
                && selectedPawn.currentCell.building.GetComponent<Teleporter>().GetIsValid())
                transferButton.gameObject.SetActive(true);
            else
                transferButton.gameObject.SetActive(false);
        }
    }

    public void PrepareAttack()
    {
        if (selectedPawn != null)
        {
            uilog.UpdateLog(selectedPawn.Name + " is trying to attack");
            //Debug.Log(selectedPawn.Name + " is trying to attack");
            gameInteraction.IsPawnAction = true;
            currentStatus = Status.PrepareAttack;
            validAttackTarget = true;
            hexMap.ProbeAttackTarget(selectedPawn.currentCell);
            hexMap.ShowAttackCandidates();
        }
    }

    public void PrepareDoSkill(int which)   // 1 - defaultSkill, 2 - equippedSkill
    {
        requirePawnSelection = false;
        requireCellSelection = false;
        currentStatus = Status.PrepareDoSkill;
        validSkillTarget = false;
        gameInteraction.IsPawnAction = true;
        try {
            Monster monster = (Monster)selectedPawn;
            if (monster != null)
            {
                uilog.UpdateLog(monster.Name + " is trying to do skill");
                gameInteraction.IsPawnAction = true;
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
            uilog.UpdateLog(selectedPawn.Name + " is trying to move");
            //Debug.Log(selectedPawn.Name + " is trying to move");
            gameInteraction.IsPawnAction = true;
            currentStatus = Status.PrepareMove;
            validRoute = true;
            hexMap.FindReachableCells(selectedPawn.currentCell, ((Monster)selectedPawn).remainedStep);
            hexMap.ShowReachableCells();
        }
    }

    public void Move()
    {

        if (selectedPawn.pawnType == PawnType.Monster)
        {
            monsterActionManager.SetActionType(hexMap.GetPathLength(), (Monster)selectedPawn);
        }
        uilog.UpdateLog(selectedPawn.Name + " Moves");
        //Debug.Log(selectedPawn.Name + " Moves");
        hexMap.HideIndicator();
        currentStatus = Status.IsMoving;

        UpdateRoot(selectedPawn, selectedPawn.currentCell, routes[routes.Count - 1]);
        if (selectedPawn.currentCell.building != null
            && selectedPawn.currentCell.building.GetBuildingType() == BuildingType.Teleporter
            && selectedPawn.currentCell.building.GetComponent<Teleporter>().GetIsValid())
            transferButton.gameObject.SetActive(true);
        else
            transferButton.gameObject.SetActive(false);
    }

    public void Attack()
    {
        uilog.UpdateLog(selectedPawn.Name + " Attacks" + attackTarget);
        //Debug.Log(selectedPawn.Name + " Attacks" + attackTarget);
        hexMap.HideIndicator();
        currentStatus = Status.IsAttacking;
        selectedPawn.DoAttack(attackTarget);
    }

    public void DoSkill()
    {
        try
        {
            Monster monster = (Monster)selectedPawn;
            if (monster != null)
            {
                switch (selectedSkill)
                {
                    case 1:
                        uilog.UpdateLog(monster.Name + " do skill one");
                        monster.DoSkillOne(skillTarget);
                        break;
                    case 3:
                        uilog.UpdateLog(monster.Name + " do skill three");
                        monster.DoSkillThree(skillTarget);
                        break;
                    case 5:
                        uilog.UpdateLog(monster.Name + " do skill five");
                        monster.DoSkillFive(skillTarget);
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
                uilog.UpdateLog(enemy.Name + " do skill");
            }
            catch (InvalidCastException ex2)
            {
                Debug.Log(ex2.StackTrace);
            }
        }
        ClearStatus();
    }

    public void Skip()
    {
        if (selectedPawn != null)
        {
            //0,0
            uilog.UpdateLog(selectedPawn.Name + " is skiping turn");
            //Debug.Log(selectedPawn.Name + " is skiping turn");
        }
    }

    public void OpenMenu()
    {
        menu.SetCurrentMonster(selectedPawn);
        menu.OpenMenu();
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
                    ClearStatus();
                }
            }
        }
        else if (currentStatus == Status.PrepareMove)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                UpdateRoute();
                if (!validRoute || !hexMap.IsReachable(getCurrentPointerCell()))
                    ClearStatus();
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
                        ClearStatus();
                    }
                }
            }
            else if (requireCellSelection)
            {
                currentStatus = Status.IsDoSkill;
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
            if (pawn.currentCell.CanbeAttackTargetOf(selectedPawn.currentCell))
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
        if (cell != null && cell.pawn != null)
        {
            validSkillTarget = true;
            skillTarget = cell.pawn;
        }
        else
        {
            Pawn pawn = getCurrentPointerPawn();
            if (pawn != null)
            {
                validSkillTarget = true;
                skillTarget = pawn;
            }
        }
    }


    private void ClearStatus()
    {
        hexMap.HideIndicator();
        currentStatus = Status.Rest;
        gameInteraction.IsPawnAction = false;
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
	
}
