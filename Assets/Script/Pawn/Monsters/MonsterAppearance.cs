using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAppearance : MonoBehaviour
{
    public Transform[] appearances=new Transform[5];
	
	public void UpdateAppearance(int level)
	{
		for(int i=0;i<5;i++)
			appearances[i].gameObject.SetActive(false);
		appearances[level-1].gameObject.SetActive(true);
	}
}
