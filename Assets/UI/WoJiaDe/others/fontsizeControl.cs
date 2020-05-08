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
    // Update is called once per frame
    void Update()
    {
		Regex rgx=new Regex("<size=\\d*>");
		newsize=(int)(UnityEngine.Screen.height*fontsize);
		newtext=rgx.Replace(this.GetComponent<Text>().text,"<size="+newsize+">");
		this.GetComponent<Text>().text=newtext;
    }
}
