using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryUnlocked : MonoBehaviour
{
	public float timelast;
	
	private float currenttime;
	
	public void OnEnable()
	{
		currenttime=timelast;
	}

    // Update is called once per frame
    void Update()
    {
        currenttime-=Time.deltaTime;
		if(currenttime<=0)
			this.gameObject.SetActive(false);
    }
}
