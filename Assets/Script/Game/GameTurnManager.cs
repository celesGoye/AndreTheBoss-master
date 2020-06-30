using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTurnManager : MonoBehaviour
{
	
    private int turnNumber;     // one turn is after player's playing and enemies' movement
	private int heroTurn;		//current turn to next hero turn
    private bool isPlayerTurn;

    private GameManager gm;

    public GameObject turnIndicator;
    private Text txtTurnNum;
    private Text txtHeroTurn;
	private int[] heroAppearingTurn;

    public void InitGameTurnManager()
    {
        turnNumber = 1;
        isPlayerTurn = true;
        if (turnIndicator != null)
		{
            //txtTurnNum = turnIndicator.GetComponentInChildren<Text>();
            txtTurnNum = turnIndicator.transform.GetChild(0).GetComponent<Text>();
			txtHeroTurn =turnIndicator.transform.GetChild(1).GetComponent<Text>();
		}
		ResetCurrentTurn(turnNumber);
    }
	
	public void ResetCurrentTurn(int turnNum)
    {
        this.turnNumber = turnNum;
        isPlayerTurn = true;
        if(txtTurnNum != null) txtTurnNum.text = "Turn "+this.turnNumber;
		for(int i=0;i<heroAppearingTurn.Length;i++)
		{
			if(this.turnNumber<=heroAppearingTurn[i]&&(i==0||this.turnNumber>heroAppearingTurn[i-1]))
			{
				heroTurn=heroAppearingTurn[i]-this.turnNumber;
				break;
			}
		}
        if(txtHeroTurn != null) txtHeroTurn.text = "next hero:\n"+heroTurn+" turn";
    }

    public void OnEnable()
    {
        if (gm == null)
            gm = GameObject.FindObjectOfType<GameManager>();
		
		heroAppearingTurn=EnemyManager.heroAppearingTurn;
    }

    public int GetCurrentGameTurn()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    public bool IsEnemyTurn()
    {
        return !isPlayerTurn;
    }

    // use below two functions to switch from player to enemy or vice-versa
    public void NextGameTurn()
    {
        gm.monsterManager.OnMonsterTurnBegin();
		gm.monsterActionManager.OnMonsterTurnBegin();
		gm.gameInteraction.OnMonsterTurnBegin();
		gm.buildingManager.OnMonsterTurnBegin();
        gm.gameEventManager.OnTurnBegin();
        turnNumber++;
        isPlayerTurn = true;
        if(txtTurnNum != null) txtTurnNum.text = "Turn "+turnNumber;
		for(int i=0;i<heroAppearingTurn.Length;i++)
		{
			if(this.turnNumber<=heroAppearingTurn[i]&&(i==0||this.turnNumber>heroAppearingTurn[i-1]))
			{
				heroTurn=heroAppearingTurn[i]-this.turnNumber;
				break;
			}
		}
        if(txtHeroTurn != null) txtHeroTurn.text = "next hero:\n"+heroTurn+" turn";

        gm.hexMap.HideIndicator();
        gm.gameCamera.FocusOnPoint(gm.boss.transform.position);
    }

    public void EndPlayerTurn()
    {
        isPlayerTurn = false;

        gm.monsterManager.OnMonsterTurnEnd();
        gm.gameEventManager.OnTurnEnd();
        gm.enemyManager.OnEnemyTurnBegin();
		
		gm.gameInteraction.OnMonsterTurnEnd();
    }

}
