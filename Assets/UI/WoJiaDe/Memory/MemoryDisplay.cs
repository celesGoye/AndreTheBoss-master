using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MemoryDisplay : MonoBehaviour
{
    public Image bg;
	public Text txtmemory;
	public Button button;
    public AudioSource audiosource;
	public string memory;
	
	public float bgalpha;
	public float bgspeed;
	public float textinterval;
	public float textspeed;
	
	private bool isEnter;
	private float currentalpha;
	
	private bool isOver;
	private string textprintedlines;
	private string textcurrentline; 
	private float textcurrenttime;
	private float textcurrentalpha;
	
	public void OnEnable()
	{
		audiosource=GetComponent<AudioSource>();
		audiosource.Play(0);
		bg.color=new Color(0,0,0,0);
		isEnter=true;
		button.interactable=false;
		audiosource.volume=1;
		isOver=false;
		textprintedlines="";
		textcurrentline="";
		
		GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>().audioManager.SetBgmVolume(-100);
	}
	
    void Update()
    {
        if(isEnter)
		{
			if(currentalpha<bgalpha)
			{
				currentalpha+=bgspeed*Time.deltaTime;
				GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>().audioManager.SetBgmVolume(-100*currentalpha/bgalpha);
			}
		}
		else
		{
				currentalpha-=bgspeed*Time.deltaTime;
				audiosource.volume=currentalpha/bgalpha;
				GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>().audioManager.SetBgmVolume(-100*currentalpha/bgalpha);
			if(currentalpha<=0)
				this.gameObject.SetActive(false);
		}
		bg.color=new Color(0,0,0,currentalpha);
		UpdateText();
    }
	
	public void UpdateMemory(string str)
	{
		memory=str;
	}
	
	public void OnConfirm()
	{
		isEnter=false;
		button.interactable=false;
	}
	
	public void UpdateText()
	{
		
		if(textcurrentalpha<1)
			textcurrentalpha+=textspeed*Time.deltaTime;
		txtmemory.text=textprintedlines+"<color=#"+ColorUtility.ToHtmlStringRGBA(new Color(1,1,1,textcurrentalpha))+">"+textcurrentline+"</color>";
		
		if(isOver)
			return;
		textcurrenttime+=Time.deltaTime;
		if(textcurrenttime>=textinterval)
		{
			textcurrenttime=0;
			textcurrentalpha=0;
			textprintedlines+=textcurrentline;
			string temp=memory.Substring(textprintedlines.Length,memory.Length-textprintedlines.Length);
			if(textprintedlines.Length>=memory.Length)
			{
				isOver=true;
				button.interactable=true;
				textcurrentline=temp;
			}
			else
				textcurrentline=temp.Substring(0,temp.IndexOf("\n")+1);
		}
	}
}
