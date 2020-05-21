using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_Ch_MonsterPage : MonoBehaviour
{
	public List<MonsterType> monsterList;
	public Transform monsters;
	public Transform frontPage;
	public Transform buttons;
	public GalleryPanel gallery;
	public Gallery_Ch_TheMonsterPage theMonsterPage;

	public int currentid;
	
	private int currentPage;
	private int totalPage;
	private GameManager gameManager;
	
	public void OnEnable()
	{
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		currentPage=0;
		currentid=0;
		theMonsterPage.gameObject.SetActive(false);
		frontPage.gameObject.SetActive(true);
		UpdateMonsterPage();
	}
	
	public void UpdateMonsterPage()
	{
		monsterList=new List<MonsterType>();
		for(int i=1;i<(int)MonsterType.NUM;i++)
		{
			if(gameManager.monsterManager.GetMonsterUnlockLevel((MonsterType)i)<=gameManager.GetBossLevel())
				monsterList.Add((MonsterType)i);
		}
		int monsterCount=monsterList.Count;
		
		int monsterShowNum=monsterCount-currentPage*10;
		for(int i=0;i<10;i++)
		{
			if(i<monsterShowNum)
			{
				monsters.GetChild(i).gameObject.SetActive(true);
				Gallery_Ch_MonsterButton monsterButton=monsters.GetChild(i).GetComponent<Gallery_Ch_MonsterButton>();
				monsterButton.type=monsterList[currentPage*10+i];
				monsterButton.id=currentPage*10+i;
				monsterButton.OnEnable();
			}
			else
				monsters.GetChild(i).gameObject.SetActive(false);
		}
				
		totalPage=monsterCount/10;
		buttons.gameObject.SetActive(totalPage==0?false:true);
	}
	
	public void OnMonsterBtn()
	{
		theMonsterPage.gameObject.SetActive(true);
		frontPage.gameObject.SetActive(false);
		gallery.previousLayer=7;
	}
	
	public void OnNextMonster()
	{
		if(currentid<monsterList.Count-1)
			currentid++;
		else
			currentid=0;
		OnMonsterBtn();
		theMonsterPage.UpdateMonster();
	}
	
	public void OnPreviousMonster()
	{
		if(currentid==0)
			currentid=monsterList.Count-1;
		else
			currentid--;
		OnMonsterBtn();
		theMonsterPage.UpdateMonster();
	}
	
	public void OnNextPage()
	{
		if(currentPage<totalPage)
			currentPage++;
		else
			currentPage=0;
		UpdateMonsterPage();
	}
	
	public void OnPreviousPage()
	{
		if(currentPage==0)
			currentPage=totalPage;
		else
			currentPage--;
		UpdateMonsterPage();
	}
}
