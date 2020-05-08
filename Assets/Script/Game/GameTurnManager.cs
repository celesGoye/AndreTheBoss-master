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

    public static int[] heroAppearingTurn =
    {
        10, 20, 30,
    };

    public void IncreaseGameTurn()
    {
        turnNumber++;
        isPlayerTurn = true;
    }

    public int CurrentGameTurn()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    public void EndPlayerTurn()
    {
        isPlayerTurn = false;
    }

}
