using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUnlockPanel : MonoBehaviour
{
	public Transform locked;
	public Transform unlocked;
	public int index;
	public Transform equipped;
	public Text skillname;
	public Image skillicon;
	
	private bool isPassive;
	private CharacterReader characterReader;
	
	public void OnEnable()
	{
		isPassive=index%2==0?true:false;
		if(characterReader == null)
			characterReader = FindObjectOfType<GameManager>().GetComponent<GameManager>().characterReader;
		
	}
	
    public void SetIsLocked(bool islocked)
	{
		locked.gameObject.SetActive(islocked);
		unlocked.gameObject.SetActive(!islocked);
	}
	
	public void UpdateUnlockPanel(Monster monster)
	{
		if(characterReader==null)
			return;
		if(monster.GetLevel()<index)
			SetIsLocked(true);
		else
		{
			SetIsLocked(false);
			if(index==1||index==monster.equippedSkill)
				equipped.gameObject.SetActive(true);
			else
				equipped.gameObject.SetActive(monster.equippedSkill==index?true:false);
			
			skillname.text=characterReader.GetMonsterSkillUI(monster.monsterType.ToString(),index).name;
		}
	}
}
