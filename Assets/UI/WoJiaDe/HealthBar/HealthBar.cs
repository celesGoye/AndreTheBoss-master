﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Camera mainCam;
	public Pawn pawn;
	public Building building;
	public Slider slider;
	public Image fill;
	
	public Color friendColor;
	public Color enemyColor;
	public Color buildingColor;
	public Color damageColor;
	public Color recoverColor;
	
	public Text number;
	public Animator animator;
	
	private float maxlife;
	private float currentlife;
	
	public void OnEnable()
	{
		
	}
	
	public void Init()
	{
		if(building!=null)
		{
			this.GetComponent<followGameObject>().follow=building.GetComponent<Transform>();
			fill.color=buildingColor;
		}
		else
		{	
			this.GetComponent<followGameObject>().follow=pawn.GetComponent<Transform>();
			fill.color=pawn.pawnType==PawnType.Monster?friendColor:enemyColor;
		}
	}
	public void UpdateLife()
	{
		if(pawn==null)
			return;
		maxlife=pawn.GetMaxHP();
		currentlife=pawn.currentHP;
		slider.value=currentlife/maxlife;
	}
	
	public void UpdateBuildingLife()
	{
		if(building==null)
			return;
		maxlife=building.GetMaxHP();
		currentlife=building.GetCurrentHP();
		slider.value=(float)currentlife/maxlife;
	}

	public void OnDestroy()
	{
		if(slider != null)
		{
			//GameObject.Destroy(slider);
			GameObject.Destroy(this.gameObject);
		}
		
		GameObject.Destroy(gameObject);
	}
	
	public void OnDamageAnim(int value)
	{
		number.text="-"+value;
		number.color=damageColor;
		if(animator!=null)
			animator.SetBool("Begin",true);
	}

	public void OnRecoverAnim(int value)
	{
		number.text="+"+value;
		number.color=recoverColor;
		if(animator!=null)
			animator.SetBool("Begin",true);
	}
}
