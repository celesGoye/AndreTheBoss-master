using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_Item : MonoBehaviour
{
	public int index;
	public float width;
	public float size;
    public ItemType type;
	public int num;
	public int numneed;
	public Image image;
	public Text text;
	public ItemReader itemReader;
	
	private Item item;
	
	public void OnEnable()
	{
		
		itemReader=FindObjectOfType<GameManager>().itemReader;
	}
	public void Update()
	{
		Vector2 v=new Vector2(width*index,0);
		this.GetComponent<RectTransform>().anchoredPosition = v*UnityEngine.Screen.height;
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,size*UnityEngine.Screen.height);
		
		item=itemReader.GetItemData(type);
		if(item.sprite!=null)
		image.sprite=item.sprite;
		if(num<numneed)
			text.text="<color=red>"+num+"</color>/"+numneed;
		else
			text.text=num+"/"+numneed;
	}
	
	public string ToString()
	{
		return ("type:"+type+"/nnum:"+num+"/nnumneed"+numneed);
	}
}
