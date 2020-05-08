using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_TheItemPage : MonoBehaviour
{
    public Gallery_ItemPage itemPage;
	public Gallery_Itemend leftPage;
	public Gallery_Itemend rightPage;
	
	public int currentid;
	public ItemType itemTypeL;
	public ItemType itemTypeR;
	public void OnEnable()
	{
		
	}
	
	public void UpdateItem()
	{
		currentid=itemPage.currentid;
		leftPage.itemType=itemPage.itemList[currentid];
		if(itemPage.itemList.Count-currentid==1)
			rightPage.transform.gameObject.SetActive(false);
		else
		{
			rightPage.itemType=itemPage.itemList[currentid+1];
			rightPage.transform.gameObject.SetActive(true);
		}
		
		leftPage.UpdateItem();
		rightPage.UpdateItem();
	}
}
