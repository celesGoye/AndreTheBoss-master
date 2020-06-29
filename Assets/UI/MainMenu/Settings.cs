using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public Slider volumnSlider;

    public void OnEnable()
    {
        if (fullScreenToggle != null)
            fullScreenToggle.isOn = Screen.fullScreen;

        if (volumnSlider != null)
            volumnSlider.value = AudioListener.volume;
    }
    public void ToggleFullScreen()
    {
        if(fullScreenToggle != null)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            if(fullScreenToggle.isOn)
            {
                Screen.fullScreen = true;
            }
            else
            {
                Screen.fullScreen = false;
            }
        }
    }

    public void ChangeVolumn()
    {
        if(volumnSlider != null)
        {
            AudioListener.volume = volumnSlider.value;
            Debug.Log("Current Volumn: " + AudioListener.volume);
        }
    }
}
