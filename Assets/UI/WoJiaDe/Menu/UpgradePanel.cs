using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
	public MenuControl menu;
	public Upgrade_ConsumePanel consumePanel;
	public Button confirm;
	public Text beforelv;
	public Text afterlv;
	public Text beforeinfo;
	public Text afterinfo;
	
	public Pawn currentMonster;
	private CharacterReader characterReader;
	private GameManager gameManager;
	
	public void OnEnable()
	{
		if(gameManager == null)
			gameManager = FindObjectOfType<GameManager>();
		characterReader=gameManager.characterReader;
		UpdateInfo();
	}
	
	public void UpdateInfo()
	{
		//Debug.Log("UpgradePanel menu.currentMonster"+menu.currentMonster);
		currentMonster=menu.currentMonster;
		consumePanel.UpdateConsumePanel();
		
		if(IsUpgradeOK())
			confirm.interactable=true;
		else
			confirm.interactable=false;
		
		beforelv.text="Lv"+currentMonster.GetLevel();
		afterlv.text=currentMonster.GetLevel()>=Pawn.MaxLevel?"Max":"Lv"+(currentMonster.GetLevel()+1);
		
		CharacterReader.CharacterData olddata = GetOldData();
		CharacterReader.CharacterData data = GetNewData();
		
		beforeinfo.text=currentMonster.GetMaxHP()+"\n"+currentMonster.currentAttack+"\n"
						+currentMonster.currentDefense+"\n"+currentMonster.currentDexterity+"\n"
						+currentMonster.currentMagicAttack+"\n"+currentMonster.currentMagicDefense+"\n"
						+currentMonster.currentAttackRange;
		
		afterinfo.text=currentMonster.GetLevel()>=Pawn.MaxLevel?"-\n-\n-\n-\n-\n-\n-":(currentMonster.GetMaxHP()-olddata.HP+data.HP)+"\n"
						+(currentMonster.currentAttack-olddata.attack+data.attack)+"\n"
						+(currentMonster.currentDefense-olddata.defense+data.defense)+"\n"
						+(currentMonster.currentDexterity-olddata.dexterity+data.dexterity)+"\n"
						+(currentMonster.currentMagicAttack-olddata.magicAttack+data.magicAttack)+"\n"
						+(currentMonster.currentMagicDefense-olddata.magicDefense+data.magicDefense)+"\n"
						+(currentMonster.currentAttackRange-olddata.attackRange+data.attackRange);
	}
	
	public bool IsUpgradeOK()
	{
		if(currentMonster.GetLevel()>=Pawn.MaxLevel)
			return false;
		if(!consumePanel.IsUpgradeOK())
			return false;
		return true;
	}
	
	public void ConfirmUpgrade()
	{
		try
		{
			Monster monster = (Monster)currentMonster;
			monster.Upgrade();
			gameManager.gameInteraction.pawnStatusPanel.UpdatePawnStatusPanel();
			//gameManager.gameInteraction.playerPanel.UpdateBossUI();
			menu.general.UpdateGeneral();
			consumePanel.ConsumeItem();
			UpdateInfo();
		}catch(Exception ex)
		{
			Debug.Log(ex.StackTrace);
		}
	}
	
	public void OnNext()
	{
		currentMonster=menu.currentMonster;
		consumePanel.UpdateConsumePanel();
		//Debug.Log("On next,current monster is:"+menu.currentMonster);
		UpdateInfo();
	}
	
	private CharacterReader.CharacterData GetOldData()
	{
		Monster monster = (Monster)currentMonster;
		return characterReader.GetMonsterData(gameManager.monsterManager.GetMonsterUnlockLevel(monster.monsterType)
				, monster.monsterType.ToString(), monster.GetLevel());
	}
	
	private CharacterReader.CharacterData GetNewData()
	{
		Monster monster = (Monster)currentMonster;
		return characterReader.GetMonsterData(gameManager.monsterManager.GetMonsterUnlockLevel(monster.monsterType)
				, monster.monsterType.ToString(), monster.GetLevel()+1);
	}
}
