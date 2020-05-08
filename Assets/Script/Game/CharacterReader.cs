using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
public class CharacterReader
{
    private string path = "/Resources/CharacterData.xml";
	private string pathUpgrade = "/Resources/CharacterUpgrade.xml";
	private string pathDescription="/Resources/CharacterDescription.xml";
	
    public XmlDocument xmlDoc;
    public XmlDocument xmlDocUpgrade;
    public XmlDocument xmlDocDescription;
    public class CharacterData
    {
        public int attack;
        public int defense;
        public int HP; 
        public int dexterity; 
        public int attackRange;
		public int magic;
		public int resistance;

        public override string ToString()
        {
            return "Attack: " + attack + " defense: " + defense + " HP: " + HP + " dexterity: " + dexterity + " attackRange: " + attackRange+" magic: "+magic+" resistance: "+resistance;
        }
    };
	
		
	public class CharacterDescription
	{
		public string story;
		public string description;
		public Sprite avatar;
		public Sprite image;
	};

    public void ReadFile()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + path);
    }
	
	public void ReadUpgradeFile()
	{
		xmlDocUpgrade=new XmlDocument();
		xmlDocUpgrade.Load(Application.dataPath+pathUpgrade);
	}
	
	public void ReadDescription()
	{
		xmlDocDescription=new XmlDocument();
		xmlDocDescription.Load(Application.dataPath+pathDescription);
	}

    public CharacterData GetCharacterData(PawnType pawnType, string characterName, int level)
    {
        CharacterData data = new CharacterData();
        string xpath = characterName;
        if (pawnType == PawnType.Enemy)
            xpath = "/characters/enemies/" + xpath;
        else if (pawnType == PawnType.Monster)
            xpath = "/characters/monsters/" + xpath;
        
        XmlElement node = (XmlElement)xmlDoc.SelectSingleNode(xpath).ChildNodes[level-1];
        
        if(node == null)
        {
            Debug.Log("On CharacterReader: " + characterName + " not found");
            return null;
        }

        data.attack = int.Parse(node["attack"].InnerXml);
        data.defense = int.Parse(node["defense"].InnerXml);
        data.HP = int.Parse(node["hp"].InnerXml);
        data.dexterity = int.Parse(node["dexterity"].InnerXml);
        data.attackRange = int.Parse(node["attackRange"].InnerXml);
		data.magic=int.Parse(node["magic"].InnerXml);
		data.resistance=int.Parse(node["resistance"].InnerXml);
        return data;
    }

    public bool InitPawnData(ref Pawn pawn, PawnType pawnType, int characterTypeEnum, int level)
    {
        if (pawn == null)
            return false;
		
        if(pawnType == PawnType.Enemy)
        {
            Enemy enemy = (Enemy)pawn;
            CharacterData data = GetCharacterData(pawnType, ((EnemyType)characterTypeEnum).ToString(), level);
            enemy.InitializeEnemy((EnemyType)characterTypeEnum, ((EnemyType)characterTypeEnum).ToString(), 
                data.attack, data.defense, data.HP, data.dexterity, data.attackRange ,data.magic,data.resistance);
			enemy.Level=level;
        }
        else if(pawnType == PawnType.Monster)
        {
            Monster monster = (Monster)pawn;
            CharacterData data = GetCharacterData(pawnType, ((MonsterType)characterTypeEnum).ToString(), level);
            monster.InitializeMonster((MonsterType)characterTypeEnum, ((MonsterType)characterTypeEnum).ToString(),
                data.attack, data.defense, data.HP, data.dexterity, data.attackRange,data.magic,data.resistance);
			monster.Level=level;
        }
        return true;
    }
	public Dictionary<ItemType,int> GetCharacterUpgrade(string type,int level)
	{
		Dictionary<ItemType,int> data=new Dictionary<ItemType,int>();
		string xpath=type;
		xpath="/monsters/"+xpath;
		XmlElement node = (XmlElement)xmlDocUpgrade.SelectSingleNode(xpath).ChildNodes[level];
		
		if(node == null)
        {
            Debug.Log("On CharacterReader GetUpgrade: " + type + " not found");
            return null;
        }
		foreach (XmlElement element in node.ChildNodes)
        {
			data[(ItemType)System.Enum.Parse(typeof(ItemType),element.Name.Trim())]=int.Parse(element.InnerXml);
        }
		return data;
	}
}
