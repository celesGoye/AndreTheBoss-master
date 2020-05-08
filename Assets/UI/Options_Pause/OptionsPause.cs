using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPause : MonoBehaviour
{
    public GameObject pauseMenu;
    
    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }

    public void Quit()
    {
        GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>().QuitGame();

    }

    public void Settings()
    {
        // setting menu
    }

    public void Exit()
    {
        // exit to main menu
    }
}
