using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_Ch_MonsterButton : MonoBehaviour
{
	public MonsterType type;
	public Gallery_Ch_MonsterPage monsterPage;
	public int id;
   
   void Awake()
	{
		this.GetComponent<Button>().onClick.AddListener(OnMonsterBtn);
		
	}
	
	public void OnEnable()
	{
		UpdateBtn();
	}
	public void UpdateBtn()
	{
		this.GetComponent<Text>().text="-"+type.ToString()+"-";
	}
	
	public void OnMonsterBtn()
	{
		monsterPage.currentid=id;
		monsterPage.OnMonsterBtn();
		
		monsterPage.theMonsterPage.UpdateMonster();
	}
}
