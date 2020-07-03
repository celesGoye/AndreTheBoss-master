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
	public Image skillIcon1;
	public Image skillIcon2;
	public Slider healthSlider;
	public Button dismissBtn;
	
	private MonsterManager monsterManager;
	private Gallery_Ch_TheMonsterPage shortcutPage;
	private Gallery_Ch_AndrePage shortcutAndrePage;
	private CharacterReader characterReader;
	private CharacterReader.CharacterSkillUI skill;
	
	private Monster currentMonster;
	private Sprite sprite;

	public Transform dismissPanel;
    void OnEnable()
    {
        UpdateGeneral();
		monsterManager=menu.gameManager.monsterManager;
		shortcutPage=menu.galleryPanel.GetComponent<GalleryPanel>().characterPage.GetComponent<Gallery_CharacterPage>().monsterPage.GetComponent<Gallery_Ch_MonsterPage>().theMonsterPage;
		shortcutAndrePage=menu.galleryPanel.GetComponent<GalleryPanel>().characterPage.GetComponent<Gallery_CharacterPage>().andrePage.GetComponent<Gallery_Ch_AndrePage>();
		dismissPanel.gameObject.SetActive(false);
    }
	
	public void Update()
	{
		healthSlider.value=(float)currentMonster.currentHP/currentMonster.GetMaxHP();
	}
	
	public void UpdateGeneral()
	{
		if(characterReader == null)
			characterReader = FindObjectOfType<GameManager>().GetComponent<GameManager>().characterReader;
		
		currentMonster=menu.currentMonster;
		nameText.text=currentMonster.Name;
		lifeText.text=currentMonster.currentHP+"/"+currentMonster.GetMaxHP();
		
		if((sprite=Resources.Load("Image/character/"+currentMonster.Name, typeof(Sprite)) as Sprite)!=null)
			characterImg.sprite =sprite;
		else
		{
			int lv=currentMonster.GetLevel();
			while((sprite=Resources.Load("Image/character/"+currentMonster.Name+lv, typeof(Sprite)) as Sprite)==null&&lv>1)
				lv--;
			if((sprite=Resources.Load("Image/character/"+currentMonster.Name+lv, typeof(Sprite)) as Sprite)!=null)
				characterImg.sprite=sprite;
		}
		
		skill=characterReader.GetMonsterSkillUI(currentMonster.monsterType.ToString(),1);
		if(skill!=null)
			skillIcon1.sprite=skill.sprite;
		
		skill=characterReader.GetMonsterSkillUI(currentMonster.monsterType.ToString(),currentMonster.GetEquippedSkill());
		if(skill!=null)
		{
			if(currentMonster.GetLevel()>=3)
			{
				skillIcon2.sprite=skill.sprite;
			}
			else
			{
				skillIcon2.sprite=Resources.Load("UI/skill/NoSkill", typeof(Sprite)) as Sprite;
			}
		}
		if(currentMonster.monsterType==MonsterType.boss)
			dismissBtn.gameObject.SetActive(false);
		else
			dismissBtn.gameObject.SetActive(true);
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
		if(menu.currentMonster.monsterType!=MonsterType.boss)
		{
			menu.galleryPanel.GetComponent<GalleryPanel>().characterPage.GetComponent<Gallery_CharacterPage>().monsterPage.gameObject.SetActive(true);
			menu.galleryPanel.GetComponent<GalleryPanel>().characterPage.GetComponent<Gallery_CharacterPage>().monsterPage.GetComponent<Gallery_Ch_MonsterPage>().frontPage.gameObject.SetActive(false);
			shortcutPage.gameObject.SetActive(true);
			shortcutPage.UpdateMonsterFromShortcut(menu.currentMonster.monsterType);
		}
		else
		{
			shortcutAndrePage.gameObject.SetActive(true);
		}
		
		menu.galleryPanel.previousLayer=-1;
	}
	
	public void OnDismissSure()
    {
		Monster monster = currentMonster;
		OnNext();
		monster.OnDie();
		menu.gameManager.monsterManager.Dismiss(monster);
	}
	
	public void OnDismiss()
	{
		dismissPanel.gameObject.SetActive(true);
	}
}
