using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAppearance : MonoBehaviour
{
    public Sprite[] appearances=new Sprite[5];
	
	private SpriteRenderer renderer;
	
	public void OnEnable()
	{
		renderer=this.transform.GetChild(0).GetComponent<SpriteRenderer>();
	}
	
	public void UpdateAppearance(int level)
	{
		renderer.sprite=appearances[level-1];
	}
}
