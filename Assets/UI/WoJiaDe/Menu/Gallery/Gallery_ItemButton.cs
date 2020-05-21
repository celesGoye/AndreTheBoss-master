using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_ItemButton : MonoBehaviour
{
	public ItemType type;
	public Gallery_ItemPage itemPage;
	public int id;
	
	void Awake()
	{
		this.GetComponent<Button>().onClick.AddListener(OnItemBtn);
		
	}
	
	public void OnEnable()
	{
		UpdateBtn();
	}
	public void UpdateBtn()
	{
		this.GetComponent<Text>().text="-"+type.ToString()+"-";
		
	}
	
	public void OnItemBtn()
	{
		itemPage.currentid=id-id%2;
		itemPage.OnItemBtn();
		
		itemPage.theItemPage.UpdateItem();
	}
}
