using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Camera mainCam;
	public Pawn pawn;
	public Slider slider;
	
	private float maxlife;
	private float currentlife;
	
	public void OnEnable()
	{
		
	}
	public void Init()
	{
		this.GetComponent<followGameObject>().follow=pawn.GetComponent<Transform>();
	}
	public void UpdateLife(){
		maxlife=pawn.GetMaxHP();
		currentlife=pawn.currentHP;
		slider.value=currentlife/maxlife;
	}
}
