using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_Ch_HeroButton : MonoBehaviour
{
	public EnemyType type;
	public Gallery_Ch_HeroPage heroPage;
	public int id;
   
   void Awake()
	{
		this.GetComponent<Button>().onClick.AddListener(OnHeroBtn);
		
	}
	
	public void OnEnable()
	{
		UpdateBtn();
	}
	public void UpdateBtn()
	{
		this.GetComponent<Text>().text="-"+type.ToString()+"-";
	}
	
	public void OnHeroBtn()
	{
		heroPage.currentid=id;
		heroPage.OnHeroBtn();
		
		heroPage.theHeroPage.UpdateHero();
	}
}
