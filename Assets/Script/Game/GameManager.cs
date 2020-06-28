using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Utility
    public CharacterReader characterReader;
    public ItemReader itemReader;
    
    // Sub Managers
    public HexMap hexMap;
    public GameCamera gameCamera;
    public GameTurnManager gameTurnManager;
    public EnemyManager enemyManager;
    public MonsterManager monsterManager;
	public BuildingManager buildingManager;
	public HealthBarManager healthbarManager;
	public ItemManager itemManager;
    public GameInteraction gameInteraction;
    public GameEventManager gameEventManager;
	public SaveManager saveManager;
	
	public Option_Die PlayerDiePanel;
	public AudioManager audioManager;
	public AnimationManager animationManager;
	public MonsterActionManager monsterActionManager;

    // Main Character
    public Boss boss;

    // Misc
    // TODO: Add more

    public void OnEnable()
    {
        InitCharacterReader();
		InitReaders();
        hexMap.GenerateCells();
        hexMap.HideCells();
		monsterManager.InitMonsterManager();
        InitBoss();
        enemyManager.InitEnemyManager();
        gameCamera.FocusOnPoint(boss.transform.position);
		buildingManager.InitBuildingManager();
		monsterActionManager.InitMonsterAcitonManager();
        gameTurnManager.InitGameTurnManager();
        enemyManager.testAltar();
        gameEventManager.InitGameEventManager();
    }

    private void InitBoss()
    {
        boss = (Boss)monsterManager.CreateMonster(MonsterType.boss, hexMap.GetRandomCellToSpawn(), 1);
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
	
	public int GetBossLevel()
	{
		return boss.GetLevel();
	}

    public void InitCharacterReader()
    {
        characterReader = new CharacterReader();
        characterReader.ReadFile();
    }
	
	public void InitReaders()
	{
		itemReader=new ItemReader();
		itemReader.ReadFile();
	}

    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
    }

    public void SaveGame()
    {
        // stub
        saveManager.Save();
    }
	
	public void OnBossDie()
    {
        PlayerDiePanel.gameObject.SetActive(true);
    }

    public CharacterReader getCharacterReader()
    {
        return this.characterReader;
    }
}
