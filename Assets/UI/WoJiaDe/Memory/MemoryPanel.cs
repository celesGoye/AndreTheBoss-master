using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPanel : MonoBehaviour
{
	public Transform memoryunlocked;
	public MemoryDisplay memorydisplay;
	
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
			memoryunlocked.gameObject.SetActive(true);
		unlocklevel=gameManager.GetBossLevel();
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
