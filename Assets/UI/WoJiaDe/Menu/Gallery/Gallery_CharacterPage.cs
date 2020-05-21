using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_CharacterPage : MonoBehaviour
{
	public Transform andrePage;
	public Transform monsterPage;
	public Transform heroPage;
	public Transform adventurerPage;
	public Transform othersPage;
	
	public Transform frontPage;
	
	public GalleryPanel gallery;

	public void OnEnable()
	{
		DisableAll();
		frontPage.gameObject.SetActive(true);
		gallery.previousLayer=0;
	}
	
	public void DisableAll()
	{
		frontPage.gameObject.SetActive(false);
		andrePage.gameObject.SetActive(false);
		monsterPage.gameObject.SetActive(false);
		heroPage.gameObject.SetActive(false);
		adventurerPage.gameObject.SetActive(false);
		othersPage.gameObject.SetActive(false);
	}
	
	public void OnAndreBtn()
	{
		DisableAll();
		andrePage.gameObject.SetActive(true);
		gallery.previousLayer=1;
	}
	public void OnMonsterBtn()
	{
		DisableAll();
		monsterPage.gameObject.SetActive(true);
		gallery.previousLayer=1;
	}
	public void OnHeroBtn()
	{
		DisableAll();
		heroPage.gameObject.SetActive(true);
		gallery.previousLayer=1;
	}
	public void OnAdventurerBtn()
	{
		DisableAll();
		adventurerPage.gameObject.SetActive(true);
		gallery.previousLayer=1;
	}
	public void OnOthersBtn()
	{
		DisableAll();
		othersPage.gameObject.SetActive(true);
		gallery.previousLayer=1;
	}

}
