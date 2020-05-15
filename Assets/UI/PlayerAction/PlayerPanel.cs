using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
	public Text txtLevel;
	public Text txtbuildmode;
	public Text txtActionPoint;
	
	public MonstersTookAction monstersTookAction;
	
	private GameManager gameManager;
	
	public void OnEnable()
	{
		if(gameManager == null)
			gameManager = FindObjectOfType<GameManager>();
		txtbuildmode.text="buildmode ON";
	}
	
    public void OnOpenMenu()
	{
		
	}
	
	public void UpdateBuildMode()
	{
		gameManager.buildingManager.UpdateBuildMode(!gameManager.buildingManager.buildmode);
	}
	
	public void Update()
	{
		txtLevel.text="Lv."+gameManager.monsterManager.MonsterPawns[0].GetLevel();
		monstersTookAction.UpdateMonstersTookAction();
		txtbuildmode.text=gameManager.buildingManager.buildmode?"buildmode On":"buildmode Off";
		txtActionPoint.text="ActionPoint:"+gameManager.monsterActionManager.actionPoint;
	}
	
	public void OnSkipTurn()
	{
		gameManager.gameTurnManager.EndPlayerTurn();
		gameManager.gameTurnManager.IncreaseGameTurn();
		gameManager.monsterActionManager.MonsterActionOnPlayerTurnBegin();
		gameManager.gameInteraction.pawnStatusPanel.UpdatePawnStatusPanel();
	}

}
