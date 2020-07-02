using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public Toggle musicToggle;
    public Slider volumnSlider;
	
	public AudioSource music;

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
	
	public void ToogleMusic()
	{
		if(musicToggle != null && music!=null)
		{
			if(musicToggle.isOn)
			{
				music.volume=1;
			}
			else
			{
				music.volume=0;
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
