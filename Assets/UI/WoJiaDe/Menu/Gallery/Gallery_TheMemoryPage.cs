using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_TheMemoryPage : MonoBehaviour
{
     public Gallery_MemoryPage memoryPage;
	public Gallery_Memoryend leftPage;
	public Gallery_Memoryend rightPage;
	
	public int currentid;
	public int memoryIdL;
	public int memoryIdR;
	
	public void OnEnable()
	{
		
	}
	
	public void UpdateMemory()
	{
		currentid=memoryPage.currentid;
		leftPage.memoryId=memoryPage.memoryPanel.memories[currentid];
		if(memoryPage.memoryPanel.memories.Count-currentid==1)
			rightPage.transform.gameObject.SetActive(false);
		else
		{
			rightPage.memoryId=memoryPage.memoryPanel.memories[currentid+1];
			rightPage.transform.gameObject.SetActive(true);
		}
		
		leftPage.UpdateItem();
		rightPage.UpdateItem();
	}
}
