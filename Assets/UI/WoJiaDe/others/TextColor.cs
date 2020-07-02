using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextColor
{
	public static string ItemColor="#6A5ACD";
	public static string RedColor="#AA5555";
	public static string OrangeColor="#AA7855";
	public static string GreenColor="#37C880";
	public static string BlueColor="#377DC8";
	public static string GreyColor="#999999";
	
	public static string SetTextColor(string text, string color)
	{
		return "<color="+color+">"+text+"</color>";
	}
}
