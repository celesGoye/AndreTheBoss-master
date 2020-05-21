using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_Ch_AdventurerPage : MonoBehaviour
{
	public List<EnemyType> adventurerList;
	public Transform adventurers;
	public Transform frontPage;
	public Transform buttons;
	public GalleryPanel gallery;
	public Gallery_Ch_TheAdventurerPage theAdventurerPage;

	public int currentid;
	
	private int currentPage;
	private int totalPage;
	private GameManager gameManager;
	
	public void OnEnable()
	{
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		currentPage=0;
		currentid=0;
		theAdventurerPage.gameObject.SetActive(false);
		frontPage.gameObject.SetActive(true);
		UpdateAdventurerPage();
	}
	
	public void UpdateAdventurerPage()
	{
		adventurerList=new List<EnemyType>();
		for(int i=1;i<(int)EnemyType.NUM;i++)
		{
			if(i%5!=4&&gameManager.enemyManager.getEnemyLevel((EnemyType)i)<=gameManager.GetBossLevel())
				adventurerList.Add((EnemyType)i);
		}
		int adventurerCount=adventurerList.Count;
		
		int adventurerShowNum=adventurerCount-currentPage*10;
		for(int i=0;i<10;i++)
		{
			if(i<adventurerShowNum)
			{
				adventurers.GetChild(i).gameObject.SetActive(true);
				Gallery_Ch_AdventurerButton adventurerButton=adventurers.GetChild(i).GetComponent<Gallery_Ch_AdventurerButton>();
				adventurerButton.type=adventurerList[currentPage*10+i];
				adventurerButton.id=currentPage*10+i;
				adventurerButton.OnEnable();
			}
			else
				adventurers.GetChild(i).gameObject.SetActive(false);
		}
				
		totalPage=adventurerCount/10;
		buttons.gameObject.SetActive(totalPage==0?false:true);
	}
	
	public void OnAdventurerBtn()
	{
		theAdventurerPage.gameObject.SetActive(true);
		frontPage.gameObject.SetActive(false);
		gallery.previousLayer=9;
	}
	
	public void OnNextAdventurer()
	{
		if(currentid<adventurerList.Count-1)
			currentid++;
		else
			currentid=0;
		OnAdventurerBtn();
		theAdventurerPage.UpdateAdventurer();
	}
	
	public void OnPreviousAdventurer()
	{
		if(currentid==0)
			currentid=adventurerList.Count-1;
		else
			currentid--;
		OnAdventurerBtn();
		theAdventurerPage.UpdateAdventurer();
	}
	
	public void OnNextPage()
	{
		if(currentPage<totalPage)
			currentPage++;
		else
			currentPage=0;
		UpdateAdventurerPage();
	}
	
	public void OnPreviousPage()
	{
		if(currentPage==0)
			currentPage=totalPage;
		else
			currentPage--;
		UpdateAdventurerPage();
	}
}
