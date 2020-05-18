using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersTookAction : MonoBehaviour
{
	public PlayerPanel playerPanel;
	public float offset;
	public float size;
	
	public MonsterActionPanel Prefab_action;
	
	private int maxActionPoint;
	private GameManager gameManager;
	
	public void OnEnable()
	{
		if(gameManager == null)
			gameManager = FindObjectOfType<GameManager>();
	}

	public void UpdateMonstersTookAction()
	{
		maxActionPoint=gameManager.monsterActionManager.MaxActionPoint;

		if(this.transform.childCount>maxActionPoint)
		{
			for(int i=maxActionPoint;i<this.transform.childCount;i++)
			{
				Transform child=this.transform.GetChild(i);
				GameObject.Destroy(child.gameObject);
			}
		}
		for(int i=0;i<maxActionPoint;i++)
		{
				if(this.transform.childCount<=i)
				{
					GenerateActionPanel(i);
				}
				MonsterActionPanel panel=this.transform.GetChild(i).GetComponent<MonsterActionPanel>();
				if(gameManager.monsterActionManager.monstersTookAction.Count>i)
				{
					panel.GetCurrentMonster(gameManager.monsterActionManager.monstersTookAction[i]);
					panel.SetActionPanel(true);
				}
				else
				{
					panel.SetActionPanel(false);
				}
				panel.UpdateActionPanel();
		}
		
	}
	
	public void GenerateActionPanel(int index)
	{
		MonsterActionPanel newpanel=Instantiate<MonsterActionPanel>(Prefab_action);
		newpanel.transform.SetParent(this.transform);
		newpanel.gameCamera=gameManager.gameCamera;
		newpanel.index=index;
		newpanel.size=size;
		newpanel.offset=offset;
	}
}
