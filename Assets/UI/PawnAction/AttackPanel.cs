using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPanel : MonoBehaviour
{
    public Text skill1;
    public Text skill2;
	public Text textswitch;
	public Image icon1;
	public Image icon2;
	public Button buttonskill1;
	public Button buttonskill2;
	public Button buttonswitch;
	public Button cancelButton;
	public Text skilldescription;
	
	public PawnAction pawnAction;
	public Monster monster;
	
	private CharacterReader characterReader;
	private CharacterReader.CharacterSkillUI skill;
	private HexMap hexMap;
	
	public void OnEnable()
	{
		hexMap=pawnAction.hexMap;
		cancelButton.gameObject.SetActive(false);
		if(characterReader == null)
			characterReader = FindObjectOfType<GameManager>().GetComponent<GameManager>().characterReader;
	}
	
	public void OnSkill(int which)
	{
		cancelButton.gameObject.SetActive(true);
		switch(which)
		{
			case 1:
				buttonskill2.interactable=false;
				buttonswitch.interactable=false;
				break;
			case 2:
				buttonskill1.interactable=false;
				buttonswitch.interactable=false;
				break;
			default:
				break;
				
		}
	}
	
	public void OnCancel()
	{
		if(pawnAction.currentStatus==PawnAction.Status.PrepareDoSkill)
		{
			UpdateAttackPanel();
			pawnAction.currentStatus=PawnAction.Status.Rest;
			pawnAction.gameInteraction.SetIsPawnAction(false);
		}
        hexMap.HideIndicator();
		cancelButton.gameObject.SetActive(false);
	}
	
	public void UpdateAttackPanel()
	{
		
		if(monster==null)
			return;
		
		if(monster.actionType==ActionType.AttackEnds||monster.actionType==ActionType.Nonactionable)
		{
			SetAttackPanelDisabled();
			return;
		}
		
		skill=characterReader.GetMonsterSkillUI(monster.monsterType.ToString(),1);
		if(skill!=null)
		{
			skill1.text=skill.name;
			buttonskill1.interactable=true;
			icon1.sprite=skill.sprite;
		}
		
		skill=characterReader.GetMonsterSkillUI(monster.monsterType.ToString(),monster.GetEquippedSkill());
		if(skill!=null)
		{
			if(monster.GetLevel()>=3)
			{
				skill2.text=skill.name;
				buttonskill2.interactable=true;
				icon2.sprite=skill.sprite;
			}
			else
			{
				skill2.text="Locked";
				buttonskill2.interactable=false;
				icon2.sprite=Resources.Load("UI/skill/NoSkill", typeof(Sprite)) as Sprite;
			}
		}
		
		if(monster.GetLevel()<5)
		{
			buttonswitch.interactable=false;
			textswitch.text="Locked";
		}
		else
		{
			buttonswitch.interactable=true;
			textswitch.text="Switch";
		}
		skill1.gameObject.SetActive(false);
		skill2.gameObject.SetActive(false);
		textswitch.gameObject.SetActive(false);
	}
		
	public void OnPointerEnter(int s)
	{
		switch(s)
		{
			case 1:
				skill=characterReader.GetMonsterSkillUI(monster.monsterType.ToString(),1);
				skilldescription.gameObject.SetActive(true);
				break;
			case 2:
				skill=characterReader.GetMonsterSkillUI(monster.monsterType.ToString(),monster.GetEquippedSkill());
				if(monster.GetLevel()>=3)
					skilldescription.gameObject.SetActive(true);
				break;
			case 3:
				skill=characterReader.GetMonsterSkillUI(monster.monsterType.ToString(),monster.GetEquippedSkill()==3?5:3);
				skilldescription.text=skill.name+"\n"+skill.description;
				if(monster.GetLevel()>=5)
					skilldescription.gameObject.SetActive(true);
				return;
			default:
				return;
		}
		skilldescription.text=skill.description;
		OnPointerEnterAttack(s);
	}
	
	public void OnPointerExit(int which)
	{
		skilldescription.gameObject.SetActive(false);
		OnPointerExitAttack(which);
	}
	
	private void OnPointerEnterAttack(int which)
	{
		if(which==2&&monster.GetLevel()<3)
			return;
		pawnAction.OnPointerEnterAttack(which);
	}
	
	private void OnPointerExitAttack(int which)
	{
		if(which==2&&monster.GetLevel()<3)
			return;
		pawnAction.OnPointerExitAttack();
	}
	
	private void SetAttackPanelDisabled()
	{
		buttonskill1.interactable=false;
		buttonskill2.interactable=false;
		buttonswitch.interactable=false;
	}

}
