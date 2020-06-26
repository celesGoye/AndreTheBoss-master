using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralPanel : MonoBehaviour
{
	public MenuControl menu;
	public Text nameText;
	public Text lifeText;
	public Image characterImg;
	public Slider healthSlider;
	
	private MonsterManager monsterManager;
	private Gallery_Ch_TheMonsterPage shortcutPage;
	
	private Monster currentMonster;
	private Sprite sprite;

    void OnEnable()
    {
        UpdateGeneral();
		monsterManager=menu.gameManager.monsterManager;
		shortcutPage=menu.galleryPanel.GetComponent<GalleryPanel>().characterPage.GetComponent<Gallery_CharacterPage>().monsterPage.GetComponent<Gallery_Ch_MonsterPage>().theMonsterPage;
    }
	
	public void Update()
	{
		healthSlider.value=(float)currentMonster.currentHP/currentMonster.GetMaxHP();
	}
	
	public void UpdateGeneral(){
		currentMonster=menu.currentMonster;
		nameText.text=currentMonster.Name;
		lifeText.text=currentMonster.currentHP+"/"+currentMonster.GetMaxHP();
		if((sprite=Resources.Load("Image/character/"+currentMonster.Name, typeof(Sprite)) as Sprite)!=null)
			characterImg.sprite =sprite;
		else if((sprite=Resources.Load("Image/character/"+currentMonster.Name+currentMonster.GetLevel(), typeof(Sprite)) as Sprite)!=null)
			characterImg.sprite=sprite;
	}
	
	public void OnPrevoius()
	{
		if(monsterManager.MonsterPawns.Contains(currentMonster))
		{
			int i=monsterManager.MonsterPawns.IndexOf(currentMonster);
			menu.currentMonster=(i<monsterManager.MonsterPawns.Count-1)?monsterManager.MonsterPawns[i+1]:monsterManager.MonsterPawns[0];
			menu.UpdateMenu();
			menu.upgradePanel.OnNext();
			menu.skillPanel.OnNext();
			return;
		}
		Debug.Log("failed to find prevoius");
	}
	
	public void OnNext()
	{
		if(monsterManager.MonsterPawns.Contains(currentMonster))
		{
			int i=monsterManager.MonsterPawns.IndexOf(currentMonster);
			menu.currentMonster=(i==0)?monsterManager.MonsterPawns[monsterManager.MonsterPawns.Count-1]:monsterManager.MonsterPawns[i-1];
			menu.UpdateMenu();
			menu.upgradePanel.OnNext();
			menu.skillPanel.OnNext();
			return;
		}
		Debug.Log("failed to find next");
	}
	
	public void OnPointerEnter(Button btn)
	{
		btn.transform.GetChild(0).gameObject.SetActive(true);
	}
	
	public void OnPointerExit(Button btn)
	{
		btn.transform.GetChild(0).gameObject.SetActive(false);
	}
	
	public void OnShortcut()
	{
		menu.OnBtnGallery();
		menu.galleryPanel.homePage.gameObject.SetActive(false);
		menu.galleryPanel.characterPage.gameObject.SetActive(true);
		menu.galleryPanel.GetComponent<GalleryPanel>().characterPage.GetComponent<Gallery_CharacterPage>().frontPage.gameObject.SetActive(false);
		menu.galleryPanel.GetComponent<GalleryPanel>().characterPage.GetComponent<Gallery_CharacterPage>().monsterPage.gameObject.SetActive(true);
		menu.galleryPanel.GetComponent<GalleryPanel>().characterPage.GetComponent<Gallery_CharacterPage>().monsterPage.GetComponent<Gallery_Ch_MonsterPage>().frontPage.gameObject.SetActive(false);
		shortcutPage.gameObject.SetActive(true);
		shortcutPage.UpdateMonsterFromShortcut(((Monster)menu.currentMonster).monsterType);
		menu.galleryPanel.previousLayer=-1;
	}
}
