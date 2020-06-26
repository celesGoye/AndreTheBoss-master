using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_MemoryPage : MonoBehaviour
{
	public int MemoryCount;
	public Transform memories;
	public Transform buttons;
	
	private int currentPage;
	private int totalPage;
	private GameManager gameManager;
	private MemoryPanel memoryPanel;
	private MemoryReader memoryReader;

	
	public void OnEnable()
	{
		if(gameManager==null)
        {
			gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
			memoryPanel=gameManager.gameInteraction.memoryPanel;
		}
		if(memoryReader==null)
		{
			memoryReader=new MemoryReader();
			memoryReader.ReadFile();
		}
		currentPage=0;
		UpdateMemoryPage();
	}


	public void UpdateMemoryPage()
	{
		int memorycount=memoryPanel.unlocklevel;
		
		int memoryshownum=memorycount-currentPage*10;
		for(int i=0;i<10;i++)
		{
			if(i<memoryshownum)
			{
				memories.GetChild(i).gameObject.SetActive(true);
				Gallery_MemoryButton memoryButton=memories.GetChild(i).GetComponent<Gallery_MemoryButton>();
				memoryButton.memorypage=this;
				memoryButton.UpdateMemoryButton(currentPage*10+i+1);
			}
			else
				memories.GetChild(i).gameObject.SetActive(false);
		}
				
		totalPage=memorycount/10;
		buttons.gameObject.SetActive(totalPage==0?false:true);
	}
	
    public void OnMemoryBtn(int index)
	{
		memoryPanel.DisplayMemory(index);
		Debug.Log("OnMemoryBtn"+index);
	}
	
	public void OnNextPage()
	{
		if(currentPage<totalPage)
			currentPage++;
		else
			currentPage=0;
		UpdateMemoryPage();
	}
	
	public void OnPreviousPage()
	{
		if(currentPage==0)
			currentPage=totalPage;
		else
			currentPage--;
		UpdateMemoryPage();
	}
	
}
