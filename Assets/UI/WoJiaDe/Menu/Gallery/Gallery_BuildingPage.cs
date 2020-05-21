using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_BuildingPage : MonoBehaviour
{
	public Gallery_TheBuildingPage theBuildingPage;
	public Transform frontPage;	
	public Transform catalog;	
	public GalleryPanel gallery;
	public int currentid;
	
	public void OnEnable()
	{
		theBuildingPage.gameObject.SetActive(false);
		frontPage.gameObject.SetActive(true);
		gallery.previousLayer=0;
	}
	
	public void OnBuildingBtn()
	{
		frontPage.gameObject.SetActive(false);
		theBuildingPage.gameObject.SetActive(true);
		gallery.previousLayer=2;	
	}
	public void OnNext()
	{
		if(currentid<catalog.childCount-1)
			catalog.GetChild(++currentid).GetComponent<Gallery_BuildingButton>().OnBuildingBtn();
		else
			catalog.GetChild(0).GetComponent<Gallery_BuildingButton>().OnBuildingBtn();
	}
	public void OnPrevious()
	{
		if(currentid==0)
			catalog.GetChild(catalog.childCount-1).GetComponent<Gallery_BuildingButton>().OnBuildingBtn();
		else
			catalog.GetChild(--currentid).GetComponent<Gallery_BuildingButton>().OnBuildingBtn();
	}
}
