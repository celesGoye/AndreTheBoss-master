using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsPause : MonoBehaviour
{
    public GameObject pauseMenu;
	public GameObject settingMenu;

    public void OnEnable()
    {
        pauseMenu = this.gameObject;
    }
    
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
        settingMenu.SetActive(true);		
    }

    public void Exit()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.saveManager.Save();
        SceneManager.LoadScene(0);
    }
}
