using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryPanel : MonoBehaviour
{
	public Transform homePage;			//0
	public Transform characterPage;		//1
	public Transform buildingPage;		//2
	public Transform encounterPage;		//3
	public Transform memoryPage;		//4
	public Transform tandoPage;			//5
	public Transform itemPage;			//6
	public Transform character_monsterPage;		//7
	public Transform character_heroPage;		//8
	public Transform character_adventurerPage;	//9
	
	public Transform backBtn;
	
	public MenuControl menu;
	
	public int previousLayer;

	public void OnEnable()
	{
		DisableAll();
		homePage.gameObject.SetActive(true);
	}
	
	public void DisableAll()
	{
		
		homePage.gameObject.SetActive(false);
		characterPage.gameObject.SetActive(false);
		buildingPage.gameObject.SetActive(false);
		encounterPage.gameObject.SetActive(false);
		memoryPage.gameObject.SetActive(false);
		tandoPage.gameObject.SetActive(false);
		itemPage.gameObject.SetActive(false);
	}
	
	public void OnCharacterBtn()
	{
		//Debug.Log("gallery---character");
		DisableAll();
		characterPage.gameObject.SetActive(true);
		backBtn.gameObject.SetActive(true);
		previousLayer=0;
	}
	public void OnBuildingBtn()
	{
		//Debug.Log("gallery---building");
		DisableAll();
		buildingPage.gameObject.SetActive(true);
		backBtn.gameObject.SetActive(true);
		previousLayer=0;
	}
	public void OnEncounterBtn()
	{
		//Debug.Log("gallery---encounter");
		DisableAll();
		encounterPage.gameObject.SetActive(true);
		backBtn.gameObject.SetActive(true);
		previousLayer=0;
	}
	public void OnMemoryBtn()
	{
		//Debug.Log("gallery---memory");
		DisableAll();
		memoryPage.gameObject.SetActive(true);
		backBtn.gameObject.SetActive(true);
		previousLayer=0;
	}
	public void OnTerrainAndObstacleBtn()
	{
		//Debug.Log("gallery---t&o");
		DisableAll();
		tandoPage.gameObject.SetActive(true);
		backBtn.gameObject.SetActive(true);
		previousLayer=0;
	}

	public void OnItemBtn()
	{
		//Debug.Log("gallery---item");
		DisableAll();
		itemPage.gameObject.SetActive(true);
		backBtn.gameObject.SetActive(true);
		previousLayer=0;
	}
	
	public void OnBackBtn()
	{
		switch(previousLayer)
		{
			case 0:
				DisableAll();
				backBtn.gameObject.SetActive(false);
				menu.OnBtnGallery();
				break;
			case 1:
				OnCharacterBtn();
				break;
			case 2:
				OnBuildingBtn();
				break;
			case 3:
				OnEncounterBtn();
				break;
			case 4:
				OnMemoryBtn();
				break;
			case 5:
				OnTerrainAndObstacleBtn();
				break;
			case 6:
				OnItemBtn();
				break;
			case 7:
				OnCharacterBtn();
				characterPage.GetComponent<Gallery_CharacterPage>().OnMonsterBtn();
				break;
			case 8:
				OnCharacterBtn();
				characterPage.GetComponent<Gallery_CharacterPage>().OnHeroBtn();
				break;
			case 9:
				OnCharacterBtn();
				characterPage.GetComponent<Gallery_CharacterPage>().OnAdventurerBtn();
				break;
		}
	}
}
