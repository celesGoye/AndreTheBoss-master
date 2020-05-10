using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterActionPanel : MonoBehaviour
{
	public Image image;
	public Button button;
	
	public int index;
	public float offset;
	public float size;
	
	public GameCamera gameCamera;
	private Monster currentMonster;
	
	public void Update()
	{
		Vector2 v=new Vector2(-offset*(index),0);
		this.GetComponent<RectTransform>().anchoredPosition = v*UnityEngine.Screen.height;
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,size*UnityEngine.Screen.height);
	}
	
	public void GetCurrentMonster(Monster monster)
	{
		currentMonster=monster;
	}
	
    public void SetActionPanel(bool isActive)
	{
		image.transform.gameObject.SetActive(isActive);
		//button.transform.gameObject.SetActive(isActive);
	}
	
	public void UpdateActionPanel()
	{
		//image
	}
	
	public void ButtonOnActionPanel()
	{
        gameCamera.FocusOnPoint(currentMonster.transform.position);
	}
}
