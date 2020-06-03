using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class fontsizeControl : MonoBehaviour
{
	public float fontsize;
	
	private int newsize;
	private string newtext;
	private Text mytext;
	
	public void OnEnable()
	{
		mytext=this.GetComponent<Text>();
	}
    void Update()
    {
		if(mytext==null)
			return;
		Regex rgx=new Regex("<size=\\d*>");
		newsize=(int)(UnityEngine.Screen.height*fontsize);
		if(rgx.IsMatch(mytext.text))
			newtext=rgx.Replace(mytext.text,"<size="+newsize+">");
		else
			newtext="<size="+newsize+">"+mytext.text+"</size>";
		mytext.text=newtext;
    }
}
