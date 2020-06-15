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
	public Button buttonskill2;
	public Button buttonswitch;
	public Text skilldescription;
	
	public Monster monster;
	
	private CharacterReader characterReader;
	private CharacterReader.CharacterSkillUI skill;
	public void OnEnable()
	{
		if(characterReader == null)
			characterReader = FindObjectOfType<GameManager>().GetComponent<GameManager>().characterReader;
		
		if(monster==null)
			return;
		
		skill=characterReader.GetMonsterSkillUI(monster.monsterType.ToString(),1);
		if(skill!=null)
		{
			skill1.text=skill.name;
			//icon1.sprite=skill.icon;
			icon1.sprite=Resources.Load("UI/skill/TestSkill", typeof(Sprite)) as Sprite;
		}
		skill=characterReader.GetMonsterSkillUI(monster.monsterType.ToString(),monster.GetEquippedSkill());
		
		if(skill!=null)
		{
			if(monster.GetLevel()>=3)
			{
				skill2.text=skill.name;
				buttonskill2.interactable=true;
				//icon2.sprite=skill.icon;
				icon2.sprite=Resources.Load("UI/skill/TestSkill", typeof(Sprite)) as Sprite;
			}
			else
			{
				skill2.text="Locked";
				buttonskill2.interactable=false;
				//icon2.sprite=Resources.Load("UI/skill/NoSkill", typeof(Sprite)) as Sprite;
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
			default:
				return;
		}
		skilldescription.text=skill.description;
	}
	
	public void OnPointerExit()
	{
		skilldescription.gameObject.SetActive(false);
	}
}
