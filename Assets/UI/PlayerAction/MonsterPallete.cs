using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPallete : MonoBehaviour
{
	public GameManager gameManager;
    public MonsterDisplay Prefab_monster;
	public MonsterSpawnPanel monsterSpawnPanel;
	
	public Transform content;
	
	public float width;
	public float height;
	public float size;
	
	public Color color;
	public MonsterType currentType;
	
	public Text name;
	public MonsterSpawnButton monsterSpawnButton;
	
    private CharacterReader characterReader;
	private List<MonsterType> SpawnableMonsters;
	private int monstercount;
	
	private int i=0;
	public void OnEnable()
	{
		SpawnableMonsters=new List<MonsterType>();
		UpdateMonsterPallete();
		currentType=MonsterType.NUM;
		monsterSpawnPanel.UpdateSpawnPanel();
		name.text="";
		//monsterSpawnButton.GetComponent<Button>().interactable=false;
	}
	
	
	public void UpdateMonsterPallete()
	{
		for(int i=0;i<(int)MonsterType.NUM;i++)
		{
			if((MonsterType)i==MonsterType.boss)
				continue;
			if(!SpawnableMonsters.Contains((MonsterType)i))
				SpawnableMonsters.Add((MonsterType)i);
		}
		MonsterDisplay monster;
		for(int i=0;i<SpawnableMonsters.Count;i++)
		{
				if(content.childCount<=i)
				{
					monster=GenerateMonsterDisplay(i);
					monster.type=SpawnableMonsters[i];
				}
				else
				{
					monster=content.GetChild(i).GetComponent<MonsterDisplay>();
					monster.type=SpawnableMonsters[i];
				}
				
			monster.gameObject.GetComponent<Image>().color=Color.white;
		}
		
        content.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (SpawnableMonsters.Count<=12?3:(SpawnableMonsters.Count)/4+1)*height*UnityEngine.Screen.height); 
	}
	
	public MonsterDisplay GenerateMonsterDisplay(int index)
	{
		MonsterDisplay newmonster=Instantiate<MonsterDisplay>(Prefab_monster);
		newmonster.transform.SetParent(content);
		newmonster.monsterPallete=this;
		
		newmonster.index=index;
		newmonster.size=size;
		newmonster.width=width;
		newmonster.height=height;
		return newmonster;
	}
	
	public void OnMonsterButton()
	{
		for(int i=0;i<SpawnableMonsters.Count;i++)
		{
			MonsterDisplay monster=content.transform.GetChild(i).GetComponent<MonsterDisplay>();
			monster.gameObject.GetComponent<Image>().color=monster.type!=currentType?Color.white:color;
		}
		name.text=currentType.ToString();
		//monsterSpawnButton.spawnType=currentType;
		monsterSpawnPanel.UpdateSpawnPanel();
	}
}
