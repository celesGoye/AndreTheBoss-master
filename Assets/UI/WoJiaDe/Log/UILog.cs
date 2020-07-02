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
		
		string welcom="点击左侧<color="+TextColor.RedColor+">Build Mode</color>来建造<color="+TextColor.ItemColor+">建筑</color>或召唤<color="+TextColor.ItemColor+">怪物</color>。点击<color="+TextColor.RedColor+">Next Turn</color>进入下一回合。";
		UpdateLog(welcom);
		welcom="<color="+TextColor.OrangeColor+">Now Begin Your GreaTest Quest!</color>";
		UpdateLog(welcom);
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
