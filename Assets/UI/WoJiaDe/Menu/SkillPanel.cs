using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    public MenuControl menu;
	public SkillUnlockPanel unlockPanel1;
	public SkillUnlockPanel unlockPanel2;
	public SkillUnlockPanel unlockPanel3;
	public SkillUnlockPanel unlockPanel4;
	public SkillUnlockPanel unlockPanel5;
	
	public SkillPreview preview;
	
	public Pawn currentMonster;
	private CharacterReader characterReader;
	private GameManager gameManager;

	public void InitSkillPanel()
	{
		if(gameManager == null)
			gameManager = FindObjectOfType<GameManager>();
		characterReader=gameManager.characterReader;
		UpdateSkill();
	}
	
	public void UpdateSkill()
	{
		currentMonster=menu.currentMonster;
		unlockPanel1.UpdateUnlockPanel((Monster)currentMonster);
		unlockPanel2.UpdateUnlockPanel((Monster)currentMonster);
		unlockPanel3.UpdateUnlockPanel((Monster)currentMonster);
		unlockPanel4.UpdateUnlockPanel((Monster)currentMonster);
		unlockPanel5.UpdateUnlockPanel((Monster)currentMonster);
	}
	
	public void OnNext()
	{
		UpdateSkill();
	}
	
	public void OnPointerEnter(int index)
	{
		if(currentMonster.level<index)
			return;
		preview.gameObject.SetActive(true);
		preview.UpdatePreview(currentMonster.Name,index);
	}
	
	public void OnPointerExit()
	{
		preview.gameObject.SetActive(false);
	}
}
