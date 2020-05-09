using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterActionManager : MonoBehaviour
{
	public MonsterManager monsterManager;
	public List<Monster> monstersTookAction;
	public int actionPoint;
	
	public GameInteraction gameInteraction;
	public static int MaxActionPoint=3;
	
	public void OnEnable()
	{
	}
	
	public void InitMonsterAcitonManager()
	{
		monstersTookAction=new List<Monster>();
		MonsterActionOnPlayerTurnBegin();
	}
    
	public void MonsterActionOnPlayerTurnBegin()
	{
		actionPoint=MaxActionPoint;
		monstersTookAction.Clear();
		foreach(Monster monster in monsterManager.MonsterPawns)
		{
			monster.actionType=ActionType.PreAction;
			monster.remainedStep=monster.currentDexterity;
			Debug.Log("action manager         "+monster.remainedStep);
		}
	}
	
	public void SetActionType(int step,Pawn pawn)
	{
		if(pawn.Type!=PawnType.Monster)
			return;
		
		Monster monster=new Monster();
		foreach(Monster m in monsterManager.MonsterPawns)
		{
			if(m==pawn)
			{
				monster=m;
				break;
			}
		}
		
		monster.remainedStep-=step;
		monster.actionType=ActionType.InAction;
		
		if(!monstersTookAction.Contains(monster))
		{
			monstersTookAction.Add(monster);
		}
		
		actionPoint=MaxActionPoint-monstersTookAction.Count;
		if(actionPoint<=0)
			ActionPointExhausted();
		
		Debug.Log("action point :"+actionPoint);
		gameInteraction.pawnStatusPanel.UpdatePawnStatusPanel(pawn);
	}
	
	public void ActionPointExhausted()
	{
		foreach(Monster monster in monsterManager.MonsterPawns)
		{
			if(monster.actionType==ActionType.PreAction)
			{
				monster.actionType=ActionType.NoAction;
				monster.remainedStep=0;
			}
		}
	}
}
