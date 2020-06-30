using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAppearance : MonoBehaviour
{
    public Sprite[] appearances=new Sprite[5];
	
	private SpriteRenderer spriterenderer;
	
	public void OnEnable()
	{
		spriterenderer=this.transform.GetChild(0).GetComponent<SpriteRenderer>();
	}
	
	public void UpdateAppearance(int level)
	{
		spriterenderer.sprite=appearances[level-1];
	}
}
