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
	
	public GameManager gamemanager;
	
	private Pawn currentMonster;
	private Sprite sprite;

    void OnEnable()
    {
        menu.UpdateMenu();
    }
	public void UpdateGeneral(){
		currentMonster=menu.currentMonster;
		nameText.text=currentMonster.Name;
		healthSlider.value=currentMonster.HP/currentMonster.MaxHp;
		lifeText.text=currentMonster.HP+"/"+currentMonster.MaxHp;
		if((sprite=Resources.Load("Image/character/"+currentMonster.Name, typeof(Sprite)) as Sprite)!=null)
			characterImg.sprite =sprite;
		else
			Debug.Log("can't find "+currentMonster.Name);
	}
	
	public void OnPrevoius()
	{
		//Debug.Log("prevoius:-----"+gamemanager.MonsterPawns.Count+"-----"+currentMonster.GetHashCode());
		for(int i=0;i<gamemanager.monsterManager.MonsterPawns.Count;i++)
		{
			if(currentMonster.GetHashCode()==gamemanager.monsterManager.MonsterPawns[i].GetHashCode())
			{
				menu.currentMonster=(i<gamemanager.monsterManager.MonsterPawns.Count-1)?gamemanager.monsterManager.MonsterPawns[i+1]:gamemanager.monsterManager.MonsterPawns[0];
				menu.UpdateMenu();
				menu.upgradePanel.consumePanel.UpdateConsumePanel();
				menu.upgradePanel.UpdateInfo();
				return;
			}
		}
		Debug.Log("failed to find prevoius");
	}
	public void OnNext()
	{
		for(int i=0;i<gamemanager.monsterManager.MonsterPawns.Count;i++){
			if(currentMonster.GetHashCode()==gamemanager.monsterManager.MonsterPawns[i].GetHashCode())
			{
				menu.currentMonster=(i==0)?gamemanager.monsterManager.MonsterPawns[gamemanager.monsterManager.MonsterPawns.Count-1]:gamemanager.monsterManager.MonsterPawns[i-1];
				menu.UpdateMenu();
				menu.upgradePanel.consumePanel.UpdateConsumePanel();
				menu.upgradePanel.UpdateInfo();
				return;
			}
		}
		Debug.Log("failed to find next");
	}
	
	public void OnPointerEnter(Button btn)
	{
		//Debug.Log(btn.gameObject.name+" pointerenter");
		btn.transform.GetChild(0).gameObject.SetActive(true);
	}
	public void OnPointerExit(Button btn)
	{
		//Debug.Log(btn.gameObject.name+" pointerexit");
		btn.transform.GetChild(0).gameObject.SetActive(false);
	}
}
