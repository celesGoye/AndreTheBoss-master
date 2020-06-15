using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	public ItemType itemType;
	public ItemPrimaryType itemPrimaryType;
	public string Intro;
	public string Use;
	public string Access;
	
	public Sprite sprite;
	public BuffEntry buff;
	
	public static Color SoulColor =		new Color(0.82f, 0.91f, 0.91f, 1.00f);
    public static Color FarmColor =		new Color(0.91f, 0.82f, 0.84f, 1.00f);
    public static Color MineColor =		new Color(0.82f, 0.83f, 0.91f, 1.00f);
	public static Color BuffColor =		new Color(0.90f, 0.87f, 0.84f, 1.00f);
}
