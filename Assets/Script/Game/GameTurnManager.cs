using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTurnManager
{
	
    private int turnNumber;     // one turn is after player's playing and enemies' movement
    private bool isPlayerTurn;

    public GameTurnManager()
    {
        turnNumber = 0;
        isPlayerTurn = true;
    }

    public void IncreaseGameTurn()
    {
        turnNumber++;
        isPlayerTurn = true;
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

    public void EndPlayerTurn()
    {
        isPlayerTurn = false;
    }

}
