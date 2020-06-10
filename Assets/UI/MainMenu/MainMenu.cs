using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AndreTheBoss.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public Button LoadGameButton;
        public void OnEnable()
        {
            if (LoadGameButton == null)
                return;

            if (File.Exists(Application.persistentDataPath + "/atb.dat"))
            {
                LoadGameButton.gameObject.SetActive(true);
            }
            else
            {
                LoadGameButton.gameObject.SetActive(false);
            }
        }

        public void NewGame()
        {
            //Debug.Log("NewGame was called");
            PlayerPrefs.SetInt("IsNewGame", 1);
            SceneManager.LoadSceneAsync(1);
        }

        public void LoadGame()
        {
            if(File.Exists(Application.persistentDataPath + "/atb.dat"))
            {
                PlayerPrefs.SetInt("IsNewGame", 0);
                SceneManager.LoadScene(1);
            }
            else
            {
                NewGame();
            }
        }

        public void Settings()
        {
            Debug.Log("Settings was called");
        }

        public void Credits()
        {
            Debug.Log("Credits was called");
        }

        public void Quit()
        {
            //Debug.Log("Quitting game");
            Application.Quit();
        }
    }

}
