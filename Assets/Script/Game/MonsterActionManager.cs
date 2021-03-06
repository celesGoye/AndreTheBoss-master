﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterActionManager : MonoBehaviour
{
	public MonsterManager monsterManager;
	public List<Monster> actionableMonsters;
	
	public GameInteraction gameInteraction;
	
	public void OnEnable()
	{
	}
	
	public void InitMonsterAcitonManager()
	{
		actionableMonsters=new List<Monster>();
		OnMonsterTurnBegin();
	}
	
	public void MonsterAttack(Monster monster)
	{
		if(monster.actionType==ActionType.MoveEnds)
		{
			monster.actionType=ActionType.Nonactionable;
			
			if(actionableMonsters.Contains(monster))
				actionableMonsters.Remove(monster);
		}
		else
			monster.actionType=ActionType.AttackEnds;
		//monster.remainedStep=0;
		
		gameInteraction.playerPanel.actionableMonsters.UpdateActionableMonsters();
		gameInteraction.pawnStatusPanel.UpdatePawnStatusPanel(monster);
	}
    
	public void OnMonsterTurnBegin()
	{
		UpdateActionableMonsters();
	}
	
	public void UpdateActionableMonsters()
	{
		actionableMonsters.Clear();
		foreach(Monster monster in monsterManager.MonsterPawns)
		{
			monster.actionType=ActionType.Actionable;
			monster.remainedStep=monster.currentDexterity;
			actionableMonsters.Add(monster);
		}
	}
	
	public void SetActionType(int step,Monster monster)
	{

		monster.remainedStep-=step;
		if(monster.remainedStep<=0)
		{
			if(monster.actionType==ActionType.AttackEnds)
			{
				
				monster.actionType=ActionType.Nonactionable;
			
				if(actionableMonsters.Contains(monster))
					actionableMonsters.Remove(monster);
			}
			else
				monster.actionType=ActionType.MoveEnds;
		}
		
		gameInteraction.playerPanel.actionableMonsters.UpdateActionableMonsters();
		gameInteraction.pawnStatusPanel.UpdatePawnStatusPanel(monster);
	}
	
	/*public void ActionPointExhausted()
	{
		foreach(Monster monster in monsterManager.MonsterPawns)
		{
			if(monster.actionType==ActionType.PreAction)
			{
				monster.actionType=ActionType.NoAction;
				monster.remainedStep=0;
			}
		}
	}*/
	
	public int GetActionNum()
	{
		return actionableMonsters.Count;
	}
	
	public void RemoveMonster(Monster monster)
	{
		if(actionableMonsters.Contains(monster))
			actionableMonsters.Remove(monster);
	}
}
