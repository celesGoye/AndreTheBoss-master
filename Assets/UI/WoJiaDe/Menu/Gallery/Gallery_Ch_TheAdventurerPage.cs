using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_Ch_TheAdventurerPage : MonoBehaviour
{
    public EnemyType type;
	public Gallery_Ch_AdventurerPage adventurerPage;
	
	public Text txtname;
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
	
	public void UpdateAdventurer()
	{
		type=adventurerPage.adventurerList[adventurerPage.currentid];
		string name=type.ToString();
		skilldata=characterReader.GetEnemySkillUI(name);
		description=characterReader.GetCharacterDescription(PawnType.Enemy,name);

		txtname.text=name;
		string skilltext="";
		for(int i=0;i<skilldata.Count;i++)
		{
			var strb = new System.Text.StringBuilder(skilldata[i].description);
			for(int j=0;skilldata[i].description.Length-18*j>18;j++)
				strb.Insert(25*j+18, "\n\u3000\u3000\u3000\u3000\u3000\u3000");
			skilldata[i].description = strb.ToString();
			skilltext+=skilldata[i].name.PadRight(6,'\u3000')+skilldata[i].description+"\n";
		}
		skill.text="<size=22>"+skilltext+"</size>";
		story.text="<size=22>"+description.story+"</size>";
		race.text=description.race;
				
		if((sprite=Resources.Load("Image/character/"+name, typeof(Sprite)) as Sprite)!=null)
			image.sprite =sprite;
	}
}
