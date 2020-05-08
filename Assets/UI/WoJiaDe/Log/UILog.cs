using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILog : MonoBehaviour
{
	public static int MaxLog=20;
	public Text txt_log;
	public Scrollbar scrollbar;
	
	private Queue<string> logs;
    
	public void OnEnable(){
		logs= new Queue<string>();
	}
	public void UpdateLog(string str){
		logs.Enqueue(str);
		if(logs.Count>MaxLog)
			logs.Dequeue();
		txt_log.text="<size=40>";
		foreach(string logStr in logs){
			txt_log.text+=logStr+"\n";
		}
		txt_log.text+="</size>";
		scrollbar.value=0;
	}
}
