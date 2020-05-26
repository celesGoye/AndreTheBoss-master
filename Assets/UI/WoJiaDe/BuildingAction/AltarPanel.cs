﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltarPanel : MonoBehaviour
{
	//public int cd;
	public DeadCharacterPanel deadPanel;
	public Button button_revive;
	public Text txt_tips;
	
	private GameManager gameManager;
	private HexCell hexCell;
	private Enemy enemy;
	private int currentcd;
	
	public void OnEnable()
	{
		if(gameManager==null)
			gameManager = FindObjectOfType<GameManager>();
	}
	
	public bool IsReviveOK()
	{
		if(enemy==null)
		{
			txt_tips.text="<size=22>No one on the record.</size>";
			return false;
		}
		else if(hexCell.pawn!=null&&(hexCell.pawn.Type==PawnType.Monster||hexCell.pawn.Type==PawnType.Enemy))
		{
			txt_tips.text="<size=22>Can't revive when altar is occupied.</size>";
			return false;
		}
		txt_tips.text="";
		return true;
	}
	
    public void UpdateAltarPanel(HexCell cell)
	{
		hexCell=cell;
		enemy=gameManager.enemyManager.getDeadEnemy();
		if(IsReviveOK())
		{
			button_revive.interactable=true;
			deadPanel.gameObject.SetActive(true);
			deadPanel.UpdateDeadPanel(enemy);
		}
		else
		{
			deadPanel.gameObject.SetActive(false);
			button_revive.interactable=false;
		}
		
	}
	
	public void OnRevive()
	{
		//revive
		gameManager.enemyManager.setDeadEnemy();
        gameManager.gameInteraction.Clear();
	}
	
	public void OnReviveCancel()
	{
        gameManager.gameInteraction.Clear();
	}
}