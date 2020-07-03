using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadCharacterPanel : MonoBehaviour
{
    public Image avatar;
	public Text txtname;
	public Text txtdata1;
	public Text txtdata2;
	public Text skill;
	
	public int letterPerLine;

	private GameManager gameManager;
	private CharacterReader characterReader;
	private List<CharacterReader.CharacterSkillUI> skilldata;
	private CharacterReader.CharacterData data;
	
	public void UpdateDeadPanel(EnemyType enemyType)
	{
		if(gameManager == null)
			gameManager = FindObjectOfType<GameManager>();
		characterReader=gameManager.characterReader;
		skilldata=characterReader.GetEnemySkillUI(enemyType.ToString());
		data=characterReader.GetEnemyData(gameManager.enemyManager.getEnemyLevel(enemyType),enemyType.ToString());
		
		txtname.text=enemyType.ToString();
		txtdata1.text=gameManager.enemyManager.getEnemyLevel(enemyType)+"\n"+data.HP+"\n"+data.attack+"\n"+data.defense;
		txtdata2.text=data.dexterity+"\n"+data.magicAttack+"\n"+data.magicDefense+"\n"+data.attackRange;
		
		string skilltext="";
		for(int i=0;i<skilldata.Count;i++)
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
