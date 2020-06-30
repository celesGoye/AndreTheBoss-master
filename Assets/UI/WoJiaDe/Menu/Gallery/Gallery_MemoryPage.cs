using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_MemoryPage : MonoBehaviour
{
	public int MemoryCount;
	public Transform memories;
	public Transform buttons;
	public Transform frontPage;
	public GalleryPanel gallery;
	public Gallery_TheMemoryPage theMemoryPage;
	public MemoryPanel memoryPanel;
	
	public int currentid;
	
	private int currentPage;
	private int totalPage;
	private GameManager gameManager;
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
		currentid=0;
		gallery.previousLayer=0;
		theMemoryPage.gameObject.SetActive(false);
		frontPage.gameObject.SetActive(true);
		UpdateMemoryPage();
	}


	public void UpdateMemoryPage()
	{
		//int memorycount=memoryPanel.unlocklevel;
		int memorycount=memoryPanel.memories.Count;
		int memoryshownum=memorycount-currentPage*10;
		for(int i=0;i<10;i++)
		{
			if(i<memoryshownum)
			{
				memories.GetChild(i).gameObject.SetActive(true);
				Gallery_MemoryButton memoryButton=memories.GetChild(i).GetComponent<Gallery_MemoryButton>();
				memoryButton.memorypage=this;
				memoryButton.memoryId=memoryPanel.memories[i];
				memoryButton.UpdateMemoryButton(currentPage*10+i+1);
			}
			else
				memories.GetChild(i).gameObject.SetActive(false);
		}	
		
		totalPage=memorycount/10;
		buttons.gameObject.SetActive(totalPage==0?false:true);
	}
	
    public void OnMemoryBtn()
	{
		//memoryPanel.DisplayMemory(memoryPanel.memories[index]);
		theMemoryPage.gameObject.SetActive(true);
		frontPage.gameObject.SetActive(false);
		gallery.previousLayer=4;
	}
		
	public void OnNextMemory()
	{
		if(currentid<memoryPanel.memories.Count-2)
			currentid+=2;
		else
			currentid=0;
		OnMemoryBtn();
		theMemoryPage.UpdateMemory();
	}
	
	public void OnPreviousMemory()
	{
		if(currentid==0)
			currentid=memoryPanel.memories.Count-2+memoryPanel.memories.Count%2;
		else
			currentid-=2;
		OnMemoryBtn();
		theMemoryPage.UpdateMemory();
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
