using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public List<HealthBar> healthBars;
	public HealthBar healthBar_Prefab;
	public Camera mainCam;
	
	public Transform parent;
	
	public HealthBar InitializeHealthBar(Pawn pawn)
	{
		HealthBar hb = new HealthBar();
		hb=Instantiate<HealthBar>(healthBar_Prefab);
		healthBars.Add(hb);
		hb.transform.SetParent(parent);
		//hb.transform.SetParent(this.transform);
		hb.pawn=pawn;
		
		//Debug.Log("manager "+pawn.Name);
		hb.slider=hb.GetComponent<Slider>();
		hb.Init();
		hb.UpdateLife();
		hb.mainCam=mainCam;
		return hb;
	}
	public void RemoveHealthBar(HealthBar hb)//。。。我想表达什么？i是啥
	{
		int i=0;
		foreach(HealthBar bar in healthBars)
		{
			if(bar==hb)
				healthBars.Remove(bar);
			i++;
		}
	}
}
