using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPanel : MonoBehaviour
{
	public Transform memoryunlocked;
	public MemoryDisplay memorydisplay;
	public List<int> memories;
	
	private GameManager gameManager;
	private MemoryReader memoryReader;
	
	public int unlocklevel;
	
	public void Start()
	{
		unlocklevel=0;
		gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		memories=new List<int>();
		
		memoryReader=new MemoryReader();
		memoryReader.ReadFile();
		 if(PlayerPrefs.GetInt("IsNewGame") == 1)
			DisplayMemory(0);
	}
	
	public void OnMonsterTurnBegin()
	{
		if(gameManager.boss.GetLevel()!=unlocklevel)
			for(int i=unlocklevel+1;i<=gameManager.boss.level;i++)
				UnlockMemory(i);
		unlocklevel=gameManager.GetBossLevel();
	}
	
	public void UnlockMemory(int index)
	{
		//memoryunlocked.gameObject.SetActive(true);
		if(memories.Contains(index)==false)
			memories.Add(index);
		gameManager.gameInteraction.uilog.UpdateLog("<color="+TextColor.RedColor+">New memory unlocked. </color>\n<color="+TextColor.GreyColor+">View in the Menu -> Gallery -> Memories</color>");
	}
	
    public void OnConfirm()
	{
		memoryunlocked.gameObject.SetActive(false);
		memorydisplay.gameObject.SetActive(false);
	}
	
	public void DisplayMemory(int index)
	{
		memorydisplay.UpdateMemory(memoryReader.GetSpecialMemoryData(index));
		memorydisplay.gameObject.SetActive(true);
	}
}
