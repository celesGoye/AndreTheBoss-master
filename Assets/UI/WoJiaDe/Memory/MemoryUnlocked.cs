using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryUnlocked : MonoBehaviour
{
	public float timelast;

    // Update is called once per frame
    void Update()
    {
        timelast-=Time.deltaTime;
		if(timelast<=0)
			GameObject.Destroy(this.gameObject);
    }
}
