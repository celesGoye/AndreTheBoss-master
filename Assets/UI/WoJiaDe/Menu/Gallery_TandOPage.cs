using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_TandOPage : MonoBehaviour
{
	public Gallery_TheTandOPage theTandOPage;
	public Transform frontPage;
	public Transform catalog;	
	public GalleryPanel gallery;
	public int currentid;
	
    public void OnEnable()
	{
		currentid=0;
		theTandOPage.gameObject.SetActive(false);
		frontPage.gameObject.SetActive(true);
		gallery.previousLayer=0;
	}
	
	public void OnTandOBtn()
	{
		theTandOPage.gameObject.SetActive(true);
		frontPage.gameObject.SetActive(false);
		gallery.previousLayer=5;
	}
	
	public void OnNext()
	{
		if(currentid<catalog.childCount-2)
			catalog.GetChild(currentid+2).GetComponent<Gallery_TandOButton>().OnTandOBtn();
		else
			catalog.GetChild(0).GetComponent<Gallery_TandOButton>().OnTandOBtn();
	}
	public void OnPrevious()
	{
		if(currentid==0)
			catalog.GetChild(catalog.childCount-2+(catalog.childCount%2)).GetComponent<Gallery_TandOButton>().OnTandOBtn();
		else
			catalog.GetChild(currentid-2).GetComponent<Gallery_TandOButton>().OnTandOBtn();
	}
}
