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
		HealthBar hb=Instantiate<HealthBar>(healthBar_Prefab);
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

	// part of cleanup process referenced by Pawn.OnDie()
	public void RemoveHealthBar(HealthBar hb)
	{
		if(healthBars.Contains(hb))
		{
			healthBars.Remove(hb);
			hb.OnDestroy();
		}
	}
}
