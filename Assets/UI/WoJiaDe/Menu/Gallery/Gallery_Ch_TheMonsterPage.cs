using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_Ch_TheMonsterPage : MonoBehaviour
{
    public MonsterType type;
	public Gallery_Ch_MonsterPage monsterPage;
	
	public Text txtname;
	public Text desc;
	public Text story;
	public Text skill;
	public Text race;
	public Image image;
	
	private CharacterReader characterReader;
	private List<CharacterReader.CharacterSkillUI> skilldata;
	private CharacterReader.CharacterDescription description;
	private Sprite sprite;

	public void OnEnable()
	{
		if(characterReader==null)
			characterReader=GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>().characterReader;
		
	}
	public void UpdateMonsterFromShortcut(MonsterType monstertype)
	{
		type=monstertype;
		UpdateCurrentMonster();
	}
	
	public void UpdateMonster()
	{
		type=monsterPage.monsterList[monsterPage.currentid];
		UpdateCurrentMonster();
	}
	
	public void UpdateCurrentMonster()
	{
		if(characterReader==null)
			characterReader=GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>().characterReader;
		string name=type.ToString();
		skilldata=characterReader.GetMonsterSkillUI(name);
		description=characterReader.GetCharacterDescription(PawnType.Monster,name);

		txtname.text=name;
		string skilltext="";
		for(int i=0;i<5;i++)
		{
			var strb = new System.Text.StringBuilder(skilldata[i].description);
			for(int j=0;skilldata[i].description.Length-18*j>18;j++)
				strb.Insert(25*j+18, "\n\u3000\u3000\u3000\u3000\u3000\u3000");
			skilldata[i].description = strb.ToString();
			skilltext+=skilldata[i].name.PadRight(6,'\u3000')+skilldata[i].description+"\n";
		}
		skill.text=skilltext;
		race.text=description.race;
		story.text=description.story;
		desc.text=description.description;
		
		if((sprite=Resources.Load("Image/character/"+name, typeof(Sprite)) as Sprite)!=null)
			image.sprite =sprite;
		else if((sprite=Resources.Load("Image/character/"+name+"5", typeof(Sprite)) as Sprite)!=null)
			image.sprite=sprite;
	}
}
