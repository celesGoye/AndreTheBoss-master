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
    public MonsterPallete monsterPalletePanel;
    public FacilityPallete facilityPalletePanel;
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
            DisableAllPanels();
            DisableIndicators();
            if (hit.collider.GetComponent<HexCell>() != null)
            {
                hexMap.SelectHex(hit.point);
                HexCell hitCell = hexMap.GetCellFromPosition(hit.point);
                if(hitCell.buildable)
                {
                    EnableHexCellActionPanel();
					hexCellActionPanel.UpdateHexCellPanel(hitCell);
                }
            }
            else if ((selectedPawn = hit.collider.GetComponent<Pawn>()) != null)
            {
				playerPanel.EndBuildMode();
                if(selectedPawn.Type == PawnType.Enemy)
                {
                    pawnStatusPanel.UpdatePawnStatusPanel(selectedPawn);
                    EnablePawnStatusPanel();
                }
                else if(selectedPawn.Type == PawnType.Monster)
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
			playerPanel.EndBuildMode();
            ClearScreen();
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
    /*private void EnableHexCellActionPanelAt(HexCell cell)
    {
        hexCellActionPanel.gameObject.SetActive(true);
        //hexCellActionPanel.transform.position = Camera.main.WorldToScreenPoint(cell.transform.position);
    }*///就不用了

    private void DisableAllPanels()
    {
        pawnActionPanel.gameObject.SetActive(false);
        pawnStatusPanel.gameObject.SetActive(false);
        hexCellActionPanel.gameObject.SetActive(false);
        monsterPalletePanel.gameObject.SetActive(false);
        facilityPalletePanel.gameObject.SetActive(false);
        
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

    public void OpenMonsterPallete()
    {
        DisableAllPalletePanels();
        monsterPalletePanel.gameObject.SetActive(true);
    }

    public void OpenFacilityPallete()
    {
        DisableAllPalletePanels();
        facilityPalletePanel.gameObject.SetActive(true);
    }

}


