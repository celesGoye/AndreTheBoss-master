using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gallery_ItemPage : MonoBehaviour
{
	public ItemManager itemManager;
	public List<ItemType> itemList;
	public Transform items;
	public Transform frontPage;
	public Transform buttons;
	public GalleryPanel gallery;
	public Gallery_TheItemPage theItemPage;

	public int currentid;
	
	private int currentPage;
	private int totalPage;
	
	public void OnEnable()
	{
		currentPage=0;
		currentid=0;
		theItemPage.gameObject.SetActive(false);
		frontPage.gameObject.SetActive(true);
		gallery.previousLayer=0;
		UpdateItemPage();
	}
	
	public void UpdateItemPage()
	{
		itemList=itemManager.ItemsGot;
		int itemCount=itemList.Count;
		int itemShowNum=itemCount-currentPage*10;
		for(int i=0;i<10;i++)
		{
			if(i<itemShowNum)
			{
				items.GetChild(i).gameObject.SetActive(true);
				items.GetChild(i).GetComponent<Gallery_ItemButton>().type=itemList[currentPage*10+i];
				items.GetChild(i).GetComponent<Gallery_ItemButton>().id=currentPage*10+i;
				items.GetChild(i).GetComponent<Gallery_ItemButton>().OnEnable();
			}
			else
				items.GetChild(i).gameObject.SetActive(false);
		}
				
		totalPage=itemCount/10;
		buttons.gameObject.SetActive(totalPage==0?false:true);
	}
	
	public void OnItemBtn()
	{
		theItemPage.gameObject.SetActive(true);
		frontPage.gameObject.SetActive(false);
		gallery.previousLayer=6;
	}
	
	public void OnNextItem()
	{
		if(currentid<itemList.Count-2)
			currentid+=2;
		else
			currentid=0;
		OnItemBtn();
		theItemPage.UpdateItem();
	}
	
	public void OnPreviousItem()
	{
		if(currentid==0)
			currentid=itemList.Count-2+itemList.Count%2;
		else
			currentid-=2;
		OnItemBtn();
		theItemPage.UpdateItem();
	}
	
	public void OnNextPage()
	{
		if(currentPage<totalPage)
			currentPage++;
		else
			currentPage=0;
		UpdateItemPage();
	}
	
	public void OnPreviousPage()
	{
		if(currentPage==0)
			currentPage=totalPage;
		else
			currentPage--;
		UpdateItemPage();
	}
}
