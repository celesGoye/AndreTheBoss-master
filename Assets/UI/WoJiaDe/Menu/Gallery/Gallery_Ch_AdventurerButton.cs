using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_Ch_AdventurerButton : MonoBehaviour
{
	public EnemyType type;
	public Gallery_Ch_AdventurerPage adventurerPage;
	public int id;
   
   void Awake()
	{
		this.GetComponent<Button>().onClick.AddListener(OnAdventurerBtn);
		
	}
	
	public void OnEnable()
	{
		UpdateBtn();
	}
	public void UpdateBtn()
	{
		this.GetComponent<Text>().text="-"+type.ToString()+"-";
	}
	
	public void OnAdventurerBtn()
	{
		adventurerPage.currentid=id;
		adventurerPage.OnAdventurerBtn();
		
		adventurerPage.theAdventurerPage.UpdateAdventurer();
	}
}
