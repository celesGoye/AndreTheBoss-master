using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventDisplayer : MonoBehaviour
{
    public GameEvent gameEvent;
    public HexCell currentCell;
	
	private GameEventPanel panel;
	private GameManager gameManager;
	private bool isActived;

    public void Update()
    {
		if(gameManager.gameTurnManager.IsPlayerTurn()&&!isActived&&currentCell.pawn!=null&&currentCell.pawn.pawnType==PawnType.Monster&&!panel.gameObject.activeInHierarchy )
		{
			isActived=true;
			panel.gameObject.SetActive(true);
			panel.UpdateGameEventPanel(gameEvent);
			panel.currentCell=currentCell;
			panel.currentMonster=(Monster)currentCell.pawn;
			panel.displayer=this;
		}
    }
	
	public void InitDisplayer()
	{
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		panel=gameManager.gameInteraction.gameEventPanel;

	}
}
