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
        public GameObject PanelCredits;
        public GameObject PanelSettings;
        public GameObject PanelLoadGame;
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
            StartCoroutine(LoadGameAsync(1));

        }

        public void LoadGame()
        {
            if(File.Exists(Application.persistentDataPath + "/atb.dat"))
            {
                PlayerPrefs.SetInt("IsNewGame", 0);
                StartCoroutine(LoadGameAsync(1));
            }
            else
            {
                NewGame();
            }
        }
        
        private IEnumerator LoadGameAsync(int sceneIndex)
        {
            PanelLoadGame.SetActive(true);
            Slider progBar = PanelLoadGame.GetComponentInChildren<Slider>();
            Animator animator = PanelLoadGame.GetComponent<Animator>();
            // Reset progress bar
            progBar.value = 0.1f;

            yield return new WaitForSeconds(0.3f);

            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneIndex);

            ao.allowSceneActivation = false;

            while (!ao.isDone)
            {
                // change progress bar
                progBar.value = Mathf.Lerp(progBar.value, ao.progress / 0.9f, Time.deltaTime);
                if (ao.progress >= 0.9f)
                {
                    progBar.value = 1.0f;
                    // change txt to press any Key
                    if (animator != null && animator.GetBool("LoadComplete") == false)
                        animator.SetBool("LoadComplete", true);

                    if(Input.anyKeyDown)
                    {
                        ao.allowSceneActivation = true;
                    }
                }
                yield return null;
            }

            PanelLoadGame.SetActive(false);
        }

        public void Settings()
        {
            if (PanelSettings != null)
                PanelSettings.gameObject.SetActive(true);
        }

        public void Credits()
        {
            if (PanelCredits != null)
                PanelCredits.gameObject.SetActive(true);
        }

        public void Quit()
        {
            //Debug.Log("Quitting game");
            Application.Quit();
        }
    }

}
