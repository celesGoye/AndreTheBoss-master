using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_Ch_HeroPage : MonoBehaviour
{
	public List<EnemyType> heroList;
	public Transform heros;
	public Transform frontPage;
	public Transform buttons;
	public GalleryPanel gallery;
	public Gallery_Ch_TheHeroPage theHeroPage;

	public int currentid;
	
	private int currentPage;
	private int totalPage;
	private GameManager gameManager;
	
	public void OnEnable()
	{
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		currentPage=0;
		currentid=0;
		theHeroPage.gameObject.SetActive(false);
		frontPage.gameObject.SetActive(true);
		UpdateHeroPage();
	}
	
	public void UpdateHeroPage()
	{
		heroList=new List<EnemyType>();
		for(int i=1;i<(int)EnemyType.NUM;i++)
		{
			if(i%5==4&&gameManager.enemyManager.getEnemyLevel((EnemyType)i)<=gameManager.GetBossLevel())
				heroList.Add((EnemyType)i);
		}
		int heroCount=heroList.Count;
		
		int heroShowNum=heroCount-currentPage*10;
		for(int i=0;i<10;i++)
		{
			if(i<heroShowNum)
			{
				heros.GetChild(i).gameObject.SetActive(true);
				Gallery_Ch_HeroButton heroButton=heros.GetChild(i).GetComponent<Gallery_Ch_HeroButton>();
				heroButton.type=heroList[currentPage*10+i];
				heroButton.id=currentPage*10+i;
				heroButton.OnEnable();
			}
			else
				heros.GetChild(i).gameObject.SetActive(false);
		}
				
		totalPage=heroCount/10;
		buttons.gameObject.SetActive(totalPage==0?false:true);
	}
	
	public void OnHeroBtn()
	{
		theHeroPage.gameObject.SetActive(true);
		frontPage.gameObject.SetActive(false);
		gallery.previousLayer=8;
	}
	
	public void OnNextHero()
	{
		if(currentid<heroList.Count-1)
			currentid++;
		else
			currentid=0;
		OnHeroBtn();
		theHeroPage.UpdateHero();
	}
	
	public void OnPreviousHero()
	{
		if(currentid==0)
			currentid=heroList.Count-1;
		else
			currentid--;
		OnHeroBtn();
		theHeroPage.UpdateHero();
	}
	
	public void OnNextPage()
	{
		if(currentPage<totalPage)
			currentPage++;
		else
			currentPage=0;
		UpdateHeroPage();
	}
	
	public void OnPreviousPage()
	{
		if(currentPage==0)
			currentPage=totalPage;
		else
			currentPage--;
		UpdateHeroPage();
	}
}
