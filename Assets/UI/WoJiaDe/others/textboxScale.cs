using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textboxScale : MonoBehaviour
{
	public float paddingX;
	public float paddingY;
	public float offsetX;
	public float offsetY;
    public RectTransform rt;
	private RectTransform myrt;
	
	public void OnEnable()
	{
		myrt=this.GetComponent<RectTransform>();
	}
	
	public void Update()
	{
		if(rt==null)
			return;
		myrt.sizeDelta=rt.sizeDelta+ new Vector2(paddingX,paddingY);
		myrt.anchoredPosition =rt.anchoredPosition+ new Vector2(offsetX,offsetY);
	}
}
