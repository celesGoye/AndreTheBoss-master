using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTurnManager : MonoBehaviour
{
	
    private int turnNumber;     // one turn is after player's playing and enemies' movement
    private bool isPlayerTurn;

    private GameManager gm;

    public void initGameTurnManager()
    {
        turnNumber = 0;
        isPlayerTurn = true;
    }

    public void OnEnable()
    {
        if (gm == null)
            gm = GameObject.FindObjectOfType<GameManager>();
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
        gm.gameCamera.FocusOnPoint(gm.boss.transform.position);
        gm.monsterManager.OnMonsterTurnBegin();
        turnNumber++;
        isPlayerTurn = true;
    }

    public void EndPlayerTurn()
    {
        isPlayerTurn = false;

        gm.monsterManager.OnMonsterTurnEnd();
        gm.enemyManager.OnEnemyTurnBegin();

    }

}
