using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followGameObject : MonoBehaviour
{
	public Transform follow;
	public Vector3 offset;
    
	private RectTransform rect;
	
	public void OnEnable()
	{
		rect=this.GetComponent<RectTransform>();
	}
	
	public void Update()
	{
		if(follow!=null)
			rect.anchoredPosition3D=follow.position+offset;
	}
}
