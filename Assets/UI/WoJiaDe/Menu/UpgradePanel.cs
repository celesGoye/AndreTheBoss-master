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
		
		beforelv.text="Lv"+currentMonster.Level;
		afterlv.text=currentMonster.Level>=Pawn.MaxLevel?"Max":"Lv"+(currentMonster.Level+1);
		
		CharacterReader.CharacterData olddata = GetOldData();
		CharacterReader.CharacterData data = GetNewData();
		
		beforeinfo.text=currentMonster.MaxHp+"\n"+currentMonster.Attack+"\n"
						+currentMonster.Defense+"\n"+currentMonster.Dexterity+"\n"
						+currentMonster.Magic+"\n"+currentMonster.Resistance+"\n"
						+currentMonster.AttackRange;
		
		afterinfo.text=currentMonster.Level>=Pawn.MaxLevel?"-\n-\n-\n-\n-\n-\n-":(currentMonster.MaxHp-olddata.HP+data.HP)+"\n"
						+(currentMonster.Attack-olddata.attack+data.attack)+"\n"
						+(currentMonster.Defense-olddata.defense+data.defense)+"\n"
						+(currentMonster.Dexterity-olddata.dexterity+data.dexterity)+"\n"
						+(currentMonster.Magic-olddata.magic+data.magic)+"\n"
						+(currentMonster.Resistance-olddata.resistance+data.resistance)+"\n"
						+(currentMonster.AttackRange-olddata.attackRange+data.attackRange);
	}
	
	public bool IsUpgradeOK(){
		if(currentMonster.Level>=Pawn.MaxLevel)
			return false;
		if(!consumePanel.IsUpgradeOK())
			return false;
		return true;
	}
	
	public void ConfirmUpgrade(){
		LevelUp();
		menu.pawnStatus.UpdatePawnStatusPanel(currentMonster);
		menu.UpdateMenu();
		UpdateInfo();
		consumePanel.ConsumeItem();
		consumePanel.UpdateConsumePanel();
	}
	
	private CharacterReader.CharacterData GetOldData()
	{
		return characterReader.GetCharacterData(currentMonster.Type,currentMonster.Name, currentMonster.Level);
	}
	
	private CharacterReader.CharacterData GetNewData()
	{
		return characterReader.GetCharacterData(currentMonster.Type,currentMonster.Name, currentMonster.Level+1);
	}
	
	public void LevelUp()
	{
		if(currentMonster.Level==0)
		{
			Debug.Log("level mustn't be 0");
			return;
		}
		else if(currentMonster.Level+1>Pawn.MaxLevel)
		{
			Debug.Log("out of maxlevel");
			return;
		}

		CharacterReader.CharacterData olddata = GetOldData();
		CharacterReader.CharacterData data = GetNewData();
		if(data!=null)
		{
			currentMonster.Attack = currentMonster.Attack-olddata.attack+data.attack;
			currentMonster.Defense = currentMonster.Defense-olddata.defense+data.defense;
			currentMonster.HP = data.HP;
			currentMonster.MaxHp= data.HP;
			currentMonster.Dexterity = currentMonster.Dexterity-olddata.dexterity+data.dexterity;
			currentMonster.AttackRange = currentMonster.AttackRange-olddata.attackRange+data.attackRange;
		}
		
		Debug.Log("upgradePanel currentMonster.level"+currentMonster.Level);
		currentMonster.Level++;
		currentMonster.Healthbar.UpdateLife();
	}
}
