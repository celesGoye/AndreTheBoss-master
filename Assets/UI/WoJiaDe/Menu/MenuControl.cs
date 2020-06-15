using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
	public GameManager gameManager;
	public Monster currentMonster;
	public UpgradePanel upgradePanel;
	public GalleryPanel galleryPanel;
	public SkillPanel skillPanel;
	public InventoryPanel inventoryPanel;
	public GeneralPanel general;
	public Text title;
	public PawnStatus pawnStatus;

    public void UpdateMenu()
    {
        upgradePanel.UpdateInfo();
		general.UpdateGeneral();
    }
	
	public void OnEnable()
	{
		if(currentMonster!=null)
			Debug.Log("current monster: "+currentMonster.Name);
		else
			Debug.Log("current monster is null!");
		//Debug.Log("menu enabled ");
	}
	
	public void DisableAll()
	{
		
		upgradePanel.gameObject.SetActive(false);
		galleryPanel.gameObject.SetActive(false);
		skillPanel.gameObject.SetActive(false);
		inventoryPanel.gameObject.SetActive(false);
		general.gameObject.SetActive(false);
	}
	
	public void OnBtnClose()
	{
		//Debug.Log("menu closed");
		this.gameObject.SetActive(false);
	}
	public void OnBtnGallery()
	{
		//Debug.Log("menu gallery");
		DisableAll();
		galleryPanel.gameObject.SetActive(true);
		title.text="Gallery";
	}
	public void OnBtnUpgrade()
	{
		//Debug.Log("menu upgrade panel");
		DisableAll();
		upgradePanel.gameObject.SetActive(true);
		general.gameObject.SetActive(true);
		title.text="Upgrade";
	}
	public void OnBtnSkill()
	{
		//Debug.Log("menu skill");
		DisableAll();
		skillPanel.gameObject.SetActive(true);
		general.gameObject.SetActive(true);
		skillPanel.InitSkillPanel();
		title.text="Skill";
	}
	public void OnBtnInventory()
	{
		//Debug.Log("menu inventory");
		DisableAll();
		inventoryPanel.gameObject.SetActive(true);
		general.gameObject.SetActive(true);
		inventoryPanel.UpdateInventory();
		title.text="Inventory";
	}
	public void OpenMenu()
	{
		this.gameObject.SetActive(true);
		OnBtnUpgrade();
		UpdateMenu();
	}
	public void SetCurrentMonster(Pawn pawn)
	{
		for(int i=0;i<gameManager.monsterManager.MonsterPawns.Count;i++)
		{
			if(pawn==gameManager.monsterManager.MonsterPawns[i])
				currentMonster=gameManager.monsterManager.MonsterPawns[i];
		}
	}
}
