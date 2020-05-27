using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPreview : MonoBehaviour
{
	public Text txtname;
	public Image image;
	public Text data1;
	public Text data2;
	public Text skill;
	
	public int letterPerLine;
	
	private GameManager gameManager;
	private CharacterReader characterReader;
	private List<CharacterReader.CharacterSkillUI> skilldata;
	private CharacterReader.CharacterData characterData;
	
	public void OnEnable()
	{
		if(gameManager==null)
			gameManager=FindObjectOfType<GameManager>();
		characterReader = gameManager.characterReader;
	}
	
	public void UpdatePreview(MonsterType type)
	{
		name=type.ToString();
		skilldata=characterReader.GetMonsterSkillUI(name);
		characterData=characterReader.GetMonsterData(gameManager.monsterManager.GetMonsterUnlockLevel(type), type.ToString(), 1);

		txtname.text=name;
		string skilltext="";
		var strb = new System.Text.StringBuilder(skilldata[0].description);
		for(int j=0;skilldata[0].description.Length-letterPerLine*j>letterPerLine;j++)
			strb.Insert((7+letterPerLine)*j+letterPerLine, "\n\u3000\u3000\u3000\u3000\u3000\u3000");
		skilldata[0].description = strb.ToString();
		skilltext+=skilldata[0].name.PadRight(6,'\u3000')+skilldata[0].description+"\n";
		skill.text="<size=22>"+skilltext+"</size>";
		
		data1.text="1\n"
				+(characterData.HP)+"\n"
				+(characterData.attack)+"\n"
				+(characterData.defense);
		data2.text=(characterData.dexterity)+"\n"
				+(characterData.magicAttack)+"\n"
				+(characterData.magicDefense)+"\n"
				+(characterData.attackRange);
	}
}
