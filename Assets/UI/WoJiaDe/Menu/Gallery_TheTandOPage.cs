using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_TheTandOPage : MonoBehaviour
{
	public Gallery_TandOPage tando;
	public Gallery_TandOend leftPage;
	public Gallery_TandOend rightPage;
	
	private int currentid;
	
	private bool isTerrainL;
	private HexType hexTypeL;
	private ObstacleType obstacleTypeL;
	
	private bool isTerrainR;
	private HexType hexTypeR;
	private ObstacleType obstacleTypeR;
	
	

	public void OnEnable()
	{
		//Debug.Log("the t or o type is: "+(isTerrain?hexType.ToString():obstacleType.ToString()));
	}
	public void UpdateTandO()
	{
		currentid=tando.currentid;
		

		isTerrainL=tando.catalog.transform.GetChild(currentid).GetComponent<Gallery_TandOButton>().isTerrain;
		hexTypeL=tando.catalog.transform.GetChild(currentid).GetComponent<Gallery_TandOButton>().hexType;
		obstacleTypeL=tando.catalog.transform.GetChild(currentid).GetComponent<Gallery_TandOButton>().obstacleType;
		leftPage.isTerrain=isTerrainL;
		if(isTerrainL)
			leftPage.hexType=hexTypeL;
		else
			leftPage.obstacleType=obstacleTypeL;
		
		
		
		if(tando.catalog.childCount-currentid==1)
			rightPage.transform.gameObject.SetActive(false);
		else
		{
			isTerrainR=tando.catalog.transform.GetChild(currentid+1).GetComponent<Gallery_TandOButton>().isTerrain;
			hexTypeR=tando.catalog.transform.GetChild(currentid+1).GetComponent<Gallery_TandOButton>().hexType;
			obstacleTypeR=tando.catalog.transform.GetChild(currentid+1).GetComponent<Gallery_TandOButton>().obstacleType;
			
			rightPage.transform.gameObject.SetActive(true);
			rightPage.isTerrain=isTerrainR;
			if(isTerrainR)
				rightPage.hexType=hexTypeR;
			else
				rightPage.obstacleType=obstacleTypeR;
		}
		
		leftPage.UpdateTandO();
		rightPage.UpdateTandO();
	}
}
