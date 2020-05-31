using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
public class CharacterReader
{
    //private string path = "/Resources/CharacterData.xml";
    private string pathMonster = "/Resources/MonsterData.xml";
    private string pathEnemy = "/Resources/EnemyData.xml";
	private string pathUpgrade = "/Resources/CharacterUpgrade.xml";
	private string pathDescription="/Resources/CharacterDescription.xml";
	
	private string pathMonsterSkill="/Resources/MonsterSkills.xml";
	private string pathEnemySkill="/Resources/EnemySkills.xml";

    //public XmlDocument xmlDoc;
    public XmlDocument xmlDocMonster;
    public XmlDocument xmlDocEnemy;
    public XmlDocument xmlDocUpgrade;
	
	public XmlDocument xmlDocMonsterSkill;
	public XmlDocument xmlDocEnemySkill;
    public XmlDocument xmlDocDescription;
    public class CharacterData
    {
        public int attack;
        public int magicAttack;
        public int defense;
        public int magicDefense;
        public int HP; 
        public int dexterity; 
        public int attackRange;
        public int dropsoul;        // reserved for enemy
		public int skillcounts;		// reserved for enemy

        public override string ToString()
        {
            return "Attack: " + attack + " defense: " + defense + " HP: " + HP + " dexterity: " + dexterity + " attackRange: " + attackRange+" magicAttack: "+magicAttack+" magicDefense: "+magicDefense;
        }
    };
	
		
	public class CharacterDescription
	{
		public string race;
		public string story;
		public string description;
		public Sprite avatar;
		public Sprite image;
	};
	
	public class CharacterSkillUI
	{
		public string name;
		public string description;
		public Sprite icon;
	}
	
	

    public void ReadFile()
    {
        //xmlDoc = new XmlDocument();
        //xmlDoc.Load(Application.dataPath + path);

        xmlDocMonster = new XmlDocument();
        xmlDocEnemy = new XmlDocument();

        xmlDocMonster.Load(Application.dataPath + pathMonster);
        xmlDocEnemy.Load(Application.dataPath + pathEnemy);

        xmlDocUpgrade = new XmlDocument();
        xmlDocUpgrade.Load(Application.dataPath + pathUpgrade);

        xmlDocMonsterSkill = new XmlDocument();
        xmlDocMonsterSkill.Load(Application.dataPath + pathMonsterSkill);
		
        xmlDocEnemySkill = new XmlDocument();
        xmlDocEnemySkill.Load(Application.dataPath + pathEnemySkill);
		
        xmlDocDescription = new XmlDocument();
        xmlDocDescription.Load(Application.dataPath + pathDescription);
    }

    public CharacterData GetMonsterData(int unlocklevel, string monsterName, int level)
    {
        // TODO: parser here
        CharacterData data = new CharacterData();

        string xpath = "/monster/unlocklevel["+(unlocklevel+1)+"]/"+monsterName+"/level["+level+"]";

        XmlElement node = (XmlElement)xmlDocMonster.SelectSingleNode(xpath);

        if (node == null)
        {
            Debug.Log("On CharacterReader: " + monsterName + " not found");
            return null;
        }

        data.attack = int.Parse(node["attack"].InnerXml);
        data.defense = int.Parse(node["defense"].InnerXml);
        data.HP = int.Parse(node["hp"].InnerXml);
        data.dexterity = int.Parse(node["dexterity"].InnerXml);
        data.attackRange = int.Parse(node["attackRange"].InnerXml);
        data.magicAttack = int.Parse(node["magicattack"].InnerXml);
        data.magicDefense = int.Parse(node["magicdefense"].InnerXml);

        return data;
    }

    public CharacterData GetEnemyData(int level, string enemyName)
    {
        CharacterData data = new CharacterData();

        string xpath = "/enemy//level[" + level + "]/" + enemyName;

        XmlElement node = (XmlElement)xmlDocEnemy.SelectSingleNode(xpath);

        if (node == null)
        {
            Debug.Log("On CharacterReader: " + enemyName + " not found");
            return null;
        }

        data.attack = int.Parse(node["attack"].InnerXml);
        data.defense = int.Parse(node["defense"].InnerXml);
        data.HP = int.Parse(node["hp"].InnerXml);
        data.dexterity = int.Parse(node["dexterity"].InnerXml);
        data.attackRange = int.Parse(node["attackRange"].InnerXml);
        data.magicAttack = int.Parse(node["magicattack"].InnerXml);
        data.magicDefense = int.Parse(node["magicdefense"].InnerXml);
        data.dropsoul = int.Parse(node["dropsoul"].InnerXml);
		data.skillcounts = int.Parse(node["skillcounts"].InnerXml);

        return data;
    }

    public bool InitEnemyData(ref Enemy enemy, int level, EnemyType type)
    {
        if (enemy == null)
            return false;

        CharacterData data = GetEnemyData(level, type.ToString());
        enemy.InitializeEnemy(type, type.ToString(), level, data.skillcounts,
            data.attack, data.magicAttack, data.defense, data.magicDefense, data.HP, data.dexterity, data.attackRange);
        return true;
    }

    public bool InitMonsterData(ref Monster monster, int unlocklevel, MonsterType type, int level)
    {
        if (monster == null)
            return false;

        CharacterData data = GetMonsterData(unlocklevel, type.ToString(), level);
            monster.InitializeMonster(type, type.ToString(), level,
                data.attack, data.magicAttack, data.defense, data.magicDefense, data.HP, data.dexterity, data.attackRange);
        return true;
    }

	public List<Vector2> GetCharacterUpgrade(int unlocklevel, string monsterName, int toLevel)  // upgrade to #toLevel
	{
        if (toLevel > 5 || unlocklevel < 0 || unlocklevel > 5)
            return null;

		List<Vector2> data=new List<Vector2>();
		string xpath="/monsters/unlocklevel["+(unlocklevel+1)+"]/"+monsterName+"/level["+toLevel+"]";
		XmlElement node = (XmlElement)xmlDocUpgrade.SelectSingleNode(xpath);
		
		if(node == null)
        {
            Debug.Log("On CharacterReader GetUpgrade: " + monsterName + " not found");
            return null;
        }
		foreach (XmlElement element in node.ChildNodes)
        {
			data.Add(new Vector2((int)System.Enum.Parse(typeof(ItemType),element.Name.Trim()),int.Parse(element.InnerXml)));
        }
		return data;
	}
	
	public List<string> GetMonsterSkillName(string monsterName)
	{
		List<string> names=new List<string>();
		string xpath="";
		for(int i=0;i<5;i++)
		{
			xpath="/monster//"+monsterName+"/level["+(i+1)+"]";
			XmlElement node =(XmlElement)xmlDocMonsterSkill.SelectSingleNode(xpath);
			if(node == null)
			{
				Debug.Log("On CharacterReader GetMonsterSkillName: " + monsterName + " skill"+(i+1)+" not found");
				return null;
			}
			names.Add(node["name"].InnerXml);
		}
		return names;
	}
	
	public CharacterSkillUI GetMonsterSkillUI(string monsterName, int skill)
	{
		CharacterSkillUI data=new CharacterSkillUI();
		string xpath="";
		
			xpath="/monster//"+monsterName+"/level["+(skill)+"]";
			XmlElement node =(XmlElement)xmlDocMonsterSkill.SelectSingleNode(xpath);
			if(node == null)
			{
				Debug.Log("On CharacterReader GetMonsterSkillName: " + monsterName + " skill"+(skill)+" not found");
				return null;
			}
			data.name=(node["name"].InnerXml);
			data.description=(node["description"].InnerXml);
		return data;
	}
	
	public List<CharacterSkillUI> GetMonsterSkillUI(string monsterName)
	{
		List<CharacterSkillUI> data=new List<CharacterSkillUI>();
		string xpath="";
		for(int i=0;i<5;i++)
		{
			xpath="/monster//"+monsterName+"/level["+(i+1)+"]";
			XmlElement node =(XmlElement)xmlDocMonsterSkill.SelectSingleNode(xpath);
			if(node == null)
			{
				Debug.Log("On CharacterReader GetMonsterSkillName: " + monsterName + " skill"+(i+1)+" not found");
				return null;
			}
			CharacterSkillUI skill=new CharacterSkillUI();
			skill.name=(node["name"].InnerXml);
			skill.description=(node["description"].InnerXml);
			data.Add(skill);
		}
		return data;
	}
	
	public List<CharacterSkillUI> GetEnemySkillUI(string enemyName)
	{
		List<CharacterSkillUI> data = new List<CharacterSkillUI>();
		string xpath="/enemy//"+enemyName;
		XmlNode node =xmlDocEnemySkill.SelectSingleNode(xpath);
		if(node==null)
		{
			Debug.Log("On CharacterReader GetEnemySkillUI: " + enemyName +" not found");
			return null;
		}
		foreach(XmlNode n in node)
		{
			XmlElement element=(XmlElement)n;
			CharacterSkillUI skill=new CharacterSkillUI();
			skill.name=(element["name"].InnerXml);
			skill.description=(element["description"].InnerXml);
			data.Add(skill);
		}
		return data;	
	}
	
	public CharacterDescription GetCharacterDescription(PawnType type,string name)
	{
		CharacterDescription data=new CharacterDescription();
	
		string xpath="";
		XmlElement node=null;
		if(type==PawnType.Monster)
		{
			xpath="/character/monster//"+name;
			node=(XmlElement)xmlDocDescription.SelectSingleNode(xpath);

		}
		else if(type==PawnType.Enemy)
		{
			xpath="/character/enemy//"+name;
			node=(XmlElement)xmlDocDescription.SelectSingleNode(xpath);
		}
		if(node==null)
		{
			Debug.Log("On CharacterReader GetCharacterDescription: " + name +" not found");
			return null;
		}
		data.race=(node["race"].InnerXml);
		data.story=(node["story"].InnerXml);
		data.description=(node["description"].InnerXml);
		return data;
	}
}
