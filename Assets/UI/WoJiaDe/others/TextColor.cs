using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextColor
{
	public static string ItemColor="#6A5ACD";
	public static string BuffColor="";
	public static string DebuffColor="";
	
	public static string SetTextColor(string text, string color)
	{
		return "<color="+color+">"+text+"</color>";
	}
}
