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

    //public XmlDocument xmlDoc;
    public XmlDocument xmlDocMonster;
    public XmlDocument xmlDocEnemy;
    public XmlDocument xmlDocUpgrade;
    //public XmlDocument xmlDocDescription;
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

        public override string ToString()
        {
            return "Attack: " + attack + " defense: " + defense + " HP: " + HP + " dexterity: " + dexterity + " attackRange: " + attackRange+" magicAttack: "+magicAttack+" magicDefense: "+magicDefense;
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
        //xmlDoc = new XmlDocument();
        //xmlDoc.Load(Application.dataPath + path);

        xmlDocMonster = new XmlDocument();
        xmlDocEnemy = new XmlDocument();

        xmlDocMonster.Load(Application.dataPath + pathMonster);
        xmlDocEnemy.Load(Application.dataPath + pathEnemy);

        xmlDocUpgrade = new XmlDocument();
        xmlDocUpgrade.Load(Application.dataPath + pathUpgrade);

        if (xmlDocUpgrade != null)
            Debug.Log(xmlDocUpgrade.OuterXml);


        //xmlDocDescription = new XmlDocument();
        //xmlDocDescription.Load(Application.dataPath + pathDescription);
    }

    /*
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
		data.magicAttack=int.Parse(node["magic"].InnerXml);
		data.magicDefense=int.Parse(node["resistance"].InnerXml);
        return data;
    }
    */

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

        string xpath = "/enemy/level[" + level + "]/" + enemyName;

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
        data.attackRange = int.Parse(node["attackrange"].InnerXml);
        data.magicAttack = int.Parse(node["magicattack"].InnerXml);
        data.magicDefense = int.Parse(node["magicdefense"].InnerXml);
        data.dropsoul = int.Parse(node["dropsoul"].InnerXml);

        Debug.Log(data.ToString());

        return data;
    }

    public bool InitEnemyData(ref Enemy enemy, int level, EnemyType type)
    {
        if (enemy == null)
            return false;

        CharacterData data = GetEnemyData(level, type.ToString());
        enemy.InitializeEnemy(type, type.ToString(), level,
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
		string xpath="/monsters/unlocklevel["+(unlocklevel+1)+"]/"+monsterName;
		XmlElement node = (XmlElement)xmlDocUpgrade.SelectSingleNode(xpath).ChildNodes[toLevel-1];
		
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
}
