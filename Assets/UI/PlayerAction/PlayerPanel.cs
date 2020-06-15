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
	public Image imgBoss;
	public Button buttonSkip;
	public Button buttonBuild;
	
	public ActionableMonsters actionableMonsters;
	
	private GameManager gameManager;
	
	public void OnEnable()
	{
		if(gameManager == null)
			gameManager = FindObjectOfType<GameManager>();
		txtbuildmode.text="buildmode ON";
		UpdateBossUI();
	}
	
    public void OpenMenu()
	{
		menu.SetCurrentMonster(gameManager.boss);
		menu.OpenMenu();
	}
	
	public void UpdateBuildMode()
	{
		gameManager.buildingManager.UpdateBuildMode(!gameManager.buildingManager.buildmode);
	}
	
	public void Update()
	{
		actionableMonsters.UpdateActionableMonsters();
		txtbuildmode.text=gameManager.buildingManager.buildmode?"buildmode On":"buildmode Off";
	}
	
	public void UpdateBossUI()
	{
		txtLevel.text="Lv."+gameManager.boss.GetLevel();
		if((Resources.Load("UI/avatar/boss"+gameManager.boss.GetLevel(), typeof(Sprite)) as Sprite)!=null)
			imgBoss.sprite=Resources.Load("UI/avatar/boss"+gameManager.boss.GetLevel(), typeof(Sprite)) as Sprite;
		else 
			Debug.Log("??");
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
