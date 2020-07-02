using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

[RequireComponent(typeof(Button), typeof(AudioSource))]
public class ButtonEffect : MonoBehaviour
{
    public Text text;
    public float fontFocusSize;
    public float fontNormalSize;
    public float fontClickSize;
    public AudioSource audiosource;
    public AudioClip clickSound;
	
	public int newsize;
	private string newtext;
	private Text mytext;
	private float currentSize;
	
    public void OnEnable()
    {
        text = GetComponentInChildren<Text>();
        //text.fontSize = fontNormalSize;
        currentSize = fontNormalSize;
        audiosource = GetComponent<AudioSource>();
        //audiosource.clip = clickSound;
    }
	
	public void Update()
	{
		if(text==null)
		{
			return;
		}
		Regex rgx=new Regex("(?:^|\n)<size=\\d*>");
		newsize=(int)(UnityEngine.Screen.height*currentSize);
		if(rgx.IsMatch(text.text))
			newtext=rgx.Replace(text.text,"<size="+newsize+">",1);
		else
			newtext="<size="+newsize+">"+text.text+"</size>";
		text.text=newtext;
	}
	
    public void OnMouseEnter()
    {
        //text.fontSize = fontFocusSize;
		currentSize=fontFocusSize;
    }

    public void OnMouseExit()
    {
        //text.fontSize = fontNormalSize;
		currentSize=fontNormalSize;
    }

    public void OnMouseDown()
    {
        //text.fontSize = fontClickSize;
		currentSize=fontClickSize;
    }

    public void OnMouseUp()
    {
        //text.fontSize = fontFocusSize;
		currentSize=fontFocusSize;
    }

    public void PlayOnEnter()
    {
        audiosource.Play(0);
    }
}
