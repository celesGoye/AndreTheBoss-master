using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
	public GameManager gameManager;
	public int buildRange;
	
	public bool buildmode;
	public Text txtbuildmode;
	
	public void OnEnable()
	{
		txtbuildmode.text="buildmode ON";
	}
    public void OnMyMonster()
	{
		
	}
	public void OnBuildMode()
	{
		if(buildmode)
		{
			EndBuildMode();
			return;
		}
		buildmode=true;
		txtbuildmode.text="buildmode OFF";
		gameManager.hexMap.UpdateBuildableCells(gameManager.monsterManager.MonsterPawns[0].currentCell, buildRange ,buildmode);
        gameManager.gameCamera.FocusOnPoint(gameManager.monsterManager.MonsterPawns[0].transform.position);
	}
	public void OnSkipTurn()
	{
		
	}
	public void EndBuildMode()
	{
		buildmode=false;
		gameManager.hexMap.HideIndicator();
		txtbuildmode.text="buildmode ON";
	}
}
