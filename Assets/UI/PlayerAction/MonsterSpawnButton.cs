using UnityEngine;
using UnityEngine.UI;

public class MonsterSpawnButton : MonoBehaviour
{
    private GameManager gameManager;
    private Button spawnMonsterButton;

    public MonsterType spawnType;

    public void SpawnMonster(MonsterType type)
    {
        HexCell cell = gameManager.hexMap.selectedCell;
        if (cell == null)
            return;
        else
        {
            gameManager.monsterManager.CreateMonster(type, cell, 1);
        }
        gameManager.gameInteraction.Clear();
    }

    public void OnEnable()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        spawnMonsterButton = GetComponent<Button>();
        spawnMonsterButton.onClick.AddListener(() => SpawnMonster(spawnType));
    }

    public void OnDisable()
    {
        spawnMonsterButton.onClick.RemoveAllListeners();
    }
}
