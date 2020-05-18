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
	
	private Monster currentMonster;
	private Sprite sprite;

    void OnEnable()
    {
        menu.UpdateMenu();
		monsterManager=menu.gameManager.monsterManager;
    }
	
	public void UpdateGeneral(){
		currentMonster=menu.currentMonster;
		nameText.text=currentMonster.Name;
		healthSlider.value=currentMonster.currentHP/currentMonster.GetMaxHP();
		lifeText.text=currentMonster.currentHP+"/"+currentMonster.GetMaxHP();
		if((sprite=Resources.Load("Image/character/"+currentMonster.Name, typeof(Sprite)) as Sprite)!=null)
			characterImg.sprite =sprite;
		else
			Debug.Log("can't find "+currentMonster.Name);
	}
	
	public void OnPrevoius()
	{
		for(int i=0;i<monsterManager.MonsterPawns.Count;i++)
		{
			if(currentMonster.GetHashCode()==monsterManager.MonsterPawns[i].GetHashCode())
			{
				menu.currentMonster=(i<monsterManager.MonsterPawns.Count-1)?monsterManager.MonsterPawns[i+1]:monsterManager.MonsterPawns[0];
				menu.UpdateMenu();
				menu.upgradePanel.OnNext();
				menu.skillPanel.OnNext();
				return;
			}
		}
		Debug.Log("failed to find prevoius");
	}
	
	public void OnNext()
	{
		for(int i=0;i<monsterManager.MonsterPawns.Count;i++){
			if(currentMonster.GetHashCode()==monsterManager.MonsterPawns[i].GetHashCode())
			{
				menu.currentMonster=(i==0)?monsterManager.MonsterPawns[monsterManager.MonsterPawns.Count-1]:monsterManager.MonsterPawns[i-1];
				menu.UpdateMenu();
				menu.upgradePanel.OnNext();
				menu.skillPanel.OnNext();
				return;
			}
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
}
