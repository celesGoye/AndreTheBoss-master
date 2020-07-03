using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexCellAction : MonoBehaviour
{
	public HexCell currentHexCell;
	public Text txtname;
	public Button btn_building;
	public Button btn_monster;
	
	private GameManager gameManager;

	public void OnEnable()
	{
		if(gameManager==null)
			gameManager = GameObject.FindObjectOfType<GameManager>();
		
		if(gameManager.monsterManager.GetCurrentMonsterCount()>=GameConfig.MonsterSpawnLimits[gameManager.GetBossLevel()])
			btn_monster.interactable=false;
		else
			btn_monster.interactable=true;
	}
	
    public void UpdateHexCellPanel(HexCell hexcell)
    {
		currentHexCell=hexcell;
        txtname.text = currentHexCell.hexType.ToString();
    } 
}
