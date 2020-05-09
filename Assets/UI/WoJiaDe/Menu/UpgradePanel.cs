using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
	public MenuControl menu;
	public ItemManager itemManager;
	public Upgrade_ConsumePanel consumePanel;
	public Button confirm;
	public Text beforelv;
	public Text afterlv;
	public Text beforeinfo;
	public Text afterinfo;
	
	public Pawn currentMonster;
	private CharacterReader characterReader;
    // Start is called before the first frame update


	void OnEnable(){
		characterReader = new CharacterReader();
        characterReader.ReadFile();
		menu.UpdateMenu();
		UpdateInfo();
	}
	
	public void UpdateInfo(){
		Debug.Log("UpgradePanel menu.currentMonster"+menu.currentMonster);
		currentMonster=menu.currentMonster;
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
	
	public bool IsUpgradeOK(){
		if(currentMonster.GetLevel()>=Pawn.MaxLevel)
			return false;
		if(!consumePanel.IsUpgradeOK())
			return false;
		return true;
	}
	
	public void ConfirmUpgrade(){
		currentMonster.Upgrade();
		menu.pawnStatus.UpdatePawnStatusPanel(currentMonster);
		menu.UpdateMenu();
		UpdateInfo();
		consumePanel.ConsumeItem();
		consumePanel.UpdateConsumePanel();
	}
	
	private CharacterReader.CharacterData GetOldData()
	{
		return characterReader.GetCharacterData(currentMonster.Type,currentMonster.Name, currentMonster.GetLevel());
	}
	
	private CharacterReader.CharacterData GetNewData()
	{
		return characterReader.GetCharacterData(currentMonster.Type,currentMonster.Name, currentMonster.GetLevel()+1);
	}
}
