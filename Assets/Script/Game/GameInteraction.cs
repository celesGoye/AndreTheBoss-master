using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInteraction : MonoBehaviour
{

    public HexMap hexMap;
    public GameCamera gameCamera;
    public Canvas canvas;
    public GameManager gameManager;

    private Pawn selectedPawn;
    private HexCell selectedCell;
    public PawnAction pawnActionPanel;
    public PawnStatus pawnStatusPanel;

    public HexCellAction hexCellActionPanel;
	public HexCellStatus hexCellStatusPanel;
	public BuildingAction buildingActionPanel;
    public MonsterPallete monsterPalletePanel;
    public FacilityPallete facilityPalletePanel;
	public NoticeBoard noticeBoard;
	public UILog uilog;
	public PlayerPanel playerPanel;

    public bool IsPawnAction = false;

    public void OnEnable()
    {
        DisableAllPanels();
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    public void Update()
    {
        if (IsPawnAction || !gameManager.gameTurnManager.IsPlayerTurn())
            return;
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            UpdateSelect();
        }
        else if(Input.GetMouseButtonDown(2) && !EventSystem.current.IsPointerOverGameObject())
        {
            UpdateFocus();
        }
    }

    private void UpdateSelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
			if(facilityPalletePanel.isSelecting)
			{
				 if (hit.collider.GetComponent<HexCell>() != null&&
						hexMap.GetTeleporterBuildableCells(gameManager.hexMap.selectedCell, 
						Teleporter.GetMaxDistance(facilityPalletePanel.currentLevel)).Contains(hit.collider.GetComponent<HexCell>()))
				{
					facilityPalletePanel.currentDestination=hit.collider.GetComponent<HexCell>();
				}
				facilityPalletePanel.facilityBuildPanel.SetIsSelecting(false);
				facilityPalletePanel.facilityBuildPanel.UpdateBuildPanel();
				return;
			}
            DisableAllPanels();
            DisableIndicators();
            if (hit.collider.GetComponent<HexCell>() != null)
            {
                hexMap.SelectHex(hit.point);
                HexCell hitCell = hexMap.GetCellFromPosition(hit.point);
                if(hitCell.buildable)
                {
                    EnableHexCellActionPanel();
					ShowBuildableHex();
					hexCellActionPanel.UpdateHexCellPanel(hitCell);
					hitCell.indicator.SetColor(Indicator.StartColor);
                }
				else
				{
					EnableHexStatusPanel();
					hexCellStatusPanel.UpdateHexStatusPanel(hitCell);
					hitCell.indicator.SetColor(Indicator.StartColor);
					if(hitCell.building!=null)
					{
						EnableBuildingActionPanel();
						buildingActionPanel.UpdateBuildingActionPanel(hitCell);
						if((hitCell.building as Teleporter)!=null)
						{
							((Teleporter)hitCell.building).another.currentCell.indicator.gameObject.SetActive(true);
							((Teleporter)hitCell.building).another.currentCell.indicator.SetColor(Indicator.StartColor);
						}
					}
				}
            }
            else if ((selectedPawn = hit.collider.GetComponent<Pawn>()) != null)
            {
				gameManager.buildingManager.UpdateBuildMode(false);
                if(selectedPawn.pawnType == PawnType.Enemy)
                {
                    pawnStatusPanel.UpdatePawnStatusPanel(selectedPawn);
                    EnablePawnStatusPanel();
                }
                else if(selectedPawn.pawnType == PawnType.Monster)
                {
                    pawnActionPanel.SetPawn(selectedPawn);
                    pawnStatusPanel.UpdatePawnStatusPanel(selectedPawn);
                    EnableAllPawnPanels();
                    hexMap.UnselectHex();
                }
            }
        }
        else
        {
			gameManager.buildingManager.UpdateBuildMode(false);
            Clear();
        }
    }

    private void EnablePawnStatusPanel()
    {
        pawnStatusPanel.gameObject.SetActive(true);
    }

    private void EnableAllPawnPanels()
    {
        pawnActionPanel.gameObject.SetActive(true);
        pawnStatusPanel.gameObject.SetActive(true);
    }
	
	private void EnableHexCellActionPanel()
	{
		hexCellActionPanel.gameObject.SetActive(true);
	}
	
	private void EnableHexStatusPanel()
	{
		hexCellStatusPanel.gameObject.SetActive(true);
	}
	
	private void EnableBuildingActionPanel()
	{
		buildingActionPanel.gameObject.SetActive(true);
	}

    private void DisableAllPanels()
    {
        pawnActionPanel.gameObject.SetActive(false);
        pawnStatusPanel.gameObject.SetActive(false);
        hexCellActionPanel.gameObject.SetActive(false);
        hexCellStatusPanel.gameObject.SetActive(false);
		buildingActionPanel.gameObject.SetActive(false);
        monsterPalletePanel.gameObject.SetActive(false);
        facilityPalletePanel.gameObject.SetActive(false);
		noticeBoard.gameObject.SetActive(false);
    }

    private void DisableAllPalletePanels()
    {
        monsterPalletePanel.gameObject.SetActive(false);
        facilityPalletePanel.gameObject.SetActive(false);
    }

    private void DisableIndicators()
    {
        hexMap.HideIndicator();
    }

    private void UpdateFocus()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<HexCell>() != null)
            {
                gameCamera.FocusOnPoint(hit.point);
            }
        }
    }

    public void Clear()
    {
        ClearScreen();
    }

    private void ClearScreen()
    {
        DisableIndicators();
        DisableAllPanels();
    }
	
	private void ShowBuildableHex()
	{
		gameManager.buildingManager.ShowBuildableHex();
	}

    public void OpenMonsterPallete()
    {
        DisableAllPalletePanels();
		gameManager.buildingManager.UpdateBuildMode(false);
        monsterPalletePanel.gameObject.SetActive(true);
    }

    public void OpenFacilityPallete()
    {
        DisableAllPalletePanels();
		gameManager.buildingManager.UpdateBuildMode(false);
        facilityPalletePanel.gameObject.SetActive(true);
    }
	
	public void GameInteractionOnPlayerTurnBegin()
	{
		//pawnStatusPanel.UpdatePawnStatusPanel();
		Clear();
	}

}


