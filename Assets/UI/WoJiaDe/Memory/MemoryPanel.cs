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
		
		memoryReader=new MemoryReader();
		memoryReader.ReadFile();
	}
	
	public void OnMonsterTurnBegin()
	{
		if(gameManager.boss.level!=unlocklevel)
			UnlockMemory(gameManager.boss.level);
		unlocklevel=gameManager.GetBossLevel();
	}
	
	public void UnlockMemory(int index)
	{
		memoryunlocked.gameObject.SetActive(true);
		if(memories.Contains(index)==false)
			memories.Add(index);
	}
	
    public void OnConfirm()
	{
		memoryunlocked.gameObject.SetActive(false);
		memorydisplay.gameObject.SetActive(false);
	}
	
	public void DisplayMemory(int index)
	{
		memorydisplay.UpdateMemory(memoryReader.GetNormalMemoryData(index));
		memorydisplay.gameObject.SetActive(true);
	}
}
