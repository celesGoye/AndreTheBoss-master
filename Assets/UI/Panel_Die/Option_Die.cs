using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option_Die : MonoBehaviour
{
    public GameObject Panel_Die;
	public MemoryPanel memoryPanel;
	public Image bg;
	
	public float bgspeed;
	private float currentalpha;
	private bool isEnter;
	
	public void OnEnable()
	{
		memoryPanel.DisplayMemory(2);
		isEnter=true;
		bg=this.GetComponent<Image>();
	}
	
	public void Update()
	{
        if(isEnter)
		{
			if(currentalpha<1)
			{
				currentalpha+=bgspeed*Time.deltaTime;
				bg.color=new Color(0,0,0,currentalpha);
			}
			else
				isEnter=false;
		}
	}

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
