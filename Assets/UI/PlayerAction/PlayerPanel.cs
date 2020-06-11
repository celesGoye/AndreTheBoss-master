using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
	public MenuControl menu;
	public Text txtLevel;
	public Text txtbuildmode;
	public Text txtActionPoint;
	public Button buttonSkip;
	public Button buttonBuild;
	
	public MonstersTookAction monstersTookAction;
	
	private GameManager gameManager;
	
	public void OnEnable()
	{
		if(gameManager == null)
			gameManager = FindObjectOfType<GameManager>();
		txtbuildmode.text="buildmode ON";
	}
	
    public void OpenMenu()
	{
		menu.SetCurrentMonster(gameManager.monsterManager.MonsterPawns[0]);
		menu.OpenMenu();
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
		txtActionPoint.text="AP: "+gameManager.monsterActionManager.actionPoint;
	}
	
	public void OnSkipTurn()
	{
		gameManager.gameTurnManager.EndPlayerTurn();
	}
	
	public void OnPawnActionChange(bool action)
	{
		EnableButtons(!action);
	}
	
	private void EnableButtons(bool interactable)
	{
		buttonSkip.interactable=interactable;
		buttonBuild.interactable=interactable;
	}
	
	public void OnTurnBegin()
	{
		EnableButtons(true);
	}
	
	public void OnTurnEnd()
	{
		EnableButtons(false);
	}
}
