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
	private float lastheight=0f;
	private bool changed;
	
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
		
		if(text!=null&&(changed||lastheight!=UnityEngine.Screen.height))
		{
			UpdateText();
			Debug.Log("update");
		}
		
	}
	
    public void OnMouseEnter()
    {
        //text.fontSize = fontFocusSize;
		currentSize=fontFocusSize;
		changed=true;
		Debug.Log("hello");
    }

    public void OnMouseExit()
    {
        //text.fontSize = fontNormalSize;
		currentSize=fontNormalSize;
		changed=true;
    }

    public void OnMouseDown()
    {
        //text.fontSize = fontClickSize;
		currentSize=fontClickSize;
		changed=true;
    }

    public void OnMouseUp()
    {
        //text.fontSize = fontFocusSize;
		currentSize=fontFocusSize;
		changed=true;
    }

    public void PlayOnEnter()
    {
        audiosource.Play(0);
    }
	
	public void UpdateText()
	{
		changed=false;
		lastheight=UnityEngine.Screen.height;
		Regex rgx=new Regex("(?:^|\n)<size=\\d*>");
		newsize=(int)(lastheight*currentSize);
		if(rgx.IsMatch(text.text))
			newtext=rgx.Replace(text.text,"<size="+newsize+">",1);
		else
			newtext="<size="+newsize+">"+text.text+"</size>";
		text.text=newtext;
	}
}
