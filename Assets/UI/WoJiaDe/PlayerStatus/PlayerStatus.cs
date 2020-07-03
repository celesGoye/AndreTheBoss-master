using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public Text txtlevel;
	public Text txtmonsternum;
	
	private GameManager gameManager;
	
	public void OnEnable()
	{
		gameManager=GameObject.FindObjectOfType<GameManager>();
		txtlevel.text="Lv."+gameManager.GetBossLevel();
		txtmonsternum.text=gameManager.monsterManager.GetCurrentMonsterCount()+"/"+GameConfig.MonsterSpawnLimits[gameManager.GetBossLevel()];
	}
}
