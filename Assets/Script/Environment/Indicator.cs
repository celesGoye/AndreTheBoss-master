using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    /*public static Color StartColor = Color.blue;
    public static  Color EndColor = Color.green;
    public static Color RouteColor = Color.white;
    public static Color AttackColor = Color.red;
	public static Color BuildColor = Color.yellow;
    public static Color FriendColor = Color.cyan;*/
	
	public static Color StartColor =new Color(0.22f, 0.39f, 0.89f, 1.00f);
    public static  Color EndColor =	new Color(0.31f, 0.85f, 0.33f, 1.00f);
    public static Color RouteColor = Color.white;
    public static Color AttackColor =	new Color(0.65f, 0.16f, 0.16f, 1.00f);
	public static Color BuildColor = 	new Color(0.93f, 0.91f, 0.67f, 1.00f);
    public static Color FriendColor = 	new Color(0.69f, 0.93f, 0.93f, 1.00f);
    public static Color AttackColorTRN =	new Color(0.65f, 0.16f, 0.16f, 0.20f);
    public static Color FriendColorTRN = 	new Color(0.69f, 0.93f, 0.93f, 0.20f);
	public static Color StartColorTRN =		new Color(0.22f, 0.39f, 0.89f, 0.20f);
	public static Color BuildColorTRN = 	new Color(0.93f, 0.91f, 0.67f, 0.20f);

    public void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
	
	public void OnDisable()
	{
		//Debug.Log("bye");
	}
}
