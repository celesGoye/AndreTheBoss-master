using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterActionManager : MonoBehaviour
{
	public MonsterManager monsterManager;
	public List<Monster> monstersTookAction;
	public int actionPoint;
	
	public GameInteraction gameInteraction;
	public int MaxActionPoint=3;
	
	public void OnEnable()
	{
	}
	
	public void InitMonsterAcitonManager()
	{
		monstersTookAction=new List<Monster>();
		OnMonsterTurnBegin();
	}
	
	public void MonsterAttack(Monster monster)
	{
		monster.actionType=ActionType.PostAction;
		monster.remainedStep=0;
		if(!monstersTookAction.Contains(monster))
		{
			monstersTookAction.Add(monster);
		}
		actionPoint=MaxActionPoint-monstersTookAction.Count;
		if(actionPoint<=0)
			ActionPointExhausted();
		
		gameInteraction.pawnStatusPanel.UpdatePawnStatusPanel(monster);
	}
    
	public void OnMonsterTurnBegin()
	{
		actionPoint=MaxActionPoint;
		monstersTookAction.Clear();
		foreach(Monster monster in monsterManager.MonsterPawns)
		{
			monster.actionType=ActionType.PreAction;
			monster.remainedStep=monster.currentDexterity;
		}
	}
	
	public void SetActionType(int step,Monster monster)
	{

		monster.remainedStep-=step;
		monster.actionType=ActionType.InAction;
		
		if(!monstersTookAction.Contains(monster))
		{
			monstersTookAction.Add(monster);
		}
		
		actionPoint=MaxActionPoint-monstersTookAction.Count;
		if(actionPoint<=0)
			ActionPointExhausted();
		
		gameInteraction.pawnStatusPanel.UpdatePawnStatusPanel(monster);
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
