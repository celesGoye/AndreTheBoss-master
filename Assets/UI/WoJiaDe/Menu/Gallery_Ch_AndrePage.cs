using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_Ch_AndrePage : MonoBehaviour
{
    public Text txtname;
	public Text story;
	public Text skill;
	public Text race;
	public Image image;
	
	private CharacterReader characterReader;
	private List<CharacterReader.MonsterSkillUI> skilldata;
	private CharacterReader.CharacterDescription description;
	
	public void OnEnable()
	{
		string name="boss";
		if(characterReader==null)
			characterReader=GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>().characterReader;
		skilldata=characterReader.GetMonsterSkillUI(name);
		description=characterReader.GetCharacterDescription(PawnType.Monster,name);

		//txtname.text="Andre";
		string skilltext="";
		for(int i=0;i<5;i++)
		{
			if(skilldata[i].description.Length>18)
			{
				var strb = new System.Text.StringBuilder(skilldata[i].description);
				strb.Insert(18, "\n\u3000\u3000\u3000\u3000\u3000\u3000");
				skilldata[i].description = strb.ToString();
			}
			skilltext+=skilldata[i].name.PadRight(6,'\u3000')+skilldata[i].description+"\n";
		}
		skill.text="<size=22>"+skilltext+"</size>";
		story.text="<size=22>"+description.story+"</size>";
		race.text=description.race;
	}
}
