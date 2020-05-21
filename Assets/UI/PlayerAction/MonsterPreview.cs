using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPreview : MonoBehaviour
{
	public Text txtname;
	public Image image;
	public Text skill;
	
	public int letterPerLine;
	
	private CharacterReader characterReader;
	private List<CharacterReader.CharacterSkillUI> skilldata;
	
	public void OnEnable()
	{
		if(characterReader==null)
		characterReader = FindObjectOfType<GameManager>().characterReader;
	}
	
	public void UpdatePreview(MonsterType type)
	{
		name=type.ToString();
		skilldata=characterReader.GetMonsterSkillUI(name);

		txtname.text=name;
		string skilltext="";
		for(int i=0;i<5;i++)
		{
			var strb = new System.Text.StringBuilder(skilldata[i].description);
			for(int j=0;skilldata[i].description.Length-letterPerLine*j>letterPerLine;j++)
				strb.Insert((7+letterPerLine)*j+letterPerLine, "\n\u3000\u3000\u3000\u3000\u3000\u3000");
			skilldata[i].description = strb.ToString();
			skilltext+=skilldata[i].name.PadRight(6,'\u3000')+skilldata[i].description+"\n";
		}
		skill.text="<size=22>"+skilltext+"</size>";
	}
}
