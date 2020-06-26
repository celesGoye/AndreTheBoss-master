using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public GameObject createEffPrefab;
	public GameObject createMonEffPrefab;
	
	public void PlayCreateEff(Vector3 pos)
	{
		GameObject newanim=Instantiate<GameObject>(createEffPrefab);
		newanim.transform.SetParent(this.transform);
		newanim.transform.position=pos;
	}
	
	public void PlayCreateMonEff(Vector3 pos)
	{
		GameObject newanim=Instantiate<GameObject>(createMonEffPrefab);
		newanim.transform.SetParent(this.transform);
		newanim.transform.position=pos;
	}
}
