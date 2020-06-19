using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionableMonsters : MonoBehaviour
{
	public PlayerPanel playerPanel;
	public float offset;
	public float size;
	
	public int numperline;
	
	public MonsterActionPanel Prefab_action;
	
	private int actionNum;
	private GameManager gameManager;
	
	public void OnEnable()
	{
		if(gameManager == null)
			gameManager = FindObjectOfType<GameManager>();
	}

	public void UpdateActionableMonsters()
	{
		actionNum=gameManager.monsterActionManager.GetActionNum();

		if(this.transform.childCount>actionNum)
		{
			for(int i=actionNum;i<this.transform.childCount;i++)
			{
				Transform child=this.transform.GetChild(i);
				GameObject.Destroy(child.gameObject);
			}
		}
		
		for(int i=this.transform.childCount;i<actionNum;i++)
		{
				if(this.transform.childCount<=i)
				{
					GenerateActionPanel();
				}
		}
		for(int i=0;i<actionNum;i++)
		{
				MonsterActionPanel panel=this.transform.GetChild(i).GetComponent<MonsterActionPanel>();
				if(gameManager.monsterActionManager.actionableMonsters.Count>i)
				{
					panel.GetCurrentMonster(gameManager.monsterActionManager.actionableMonsters[actionNum-1-i]);
					panel.SetActionPanel(true);
				}
				else
				{
					panel.SetActionPanel(false);
				}
				panel.index=actionNum-1-i;
				panel.UpdateActionPanel();
		}
	}
	
	public void GenerateActionPanel()
	{
		MonsterActionPanel newpanel=Instantiate<MonsterActionPanel>(Prefab_action);
		newpanel.transform.SetParent(this.transform);
		newpanel.size=size;
		newpanel.offset=offset;
		//newpanel.numperline=numperline;
	}
	
	public void Update()
	{
		foreach (Transform child in transform)
		{
			if(child.GetComponent<MonsterActionPanel>()!=null)
				child.GetComponent<MonsterActionPanel>().UpdatePosition();
		}
		
        this.transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (transform.childCount<=numperline?numperline+1:(transform.childCount+1))*offset*UnityEngine.Screen.height); 
	}
}
