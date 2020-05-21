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

	public void OnEnable()
	{
		//Debug.Log("the t or o type is: "+(isTerrain?hexType.ToString():obstacleType.ToString()));
	}
	public void UpdateTandO()
	{
		currentid=tando.currentid;
		
		HexType hexTypeL=tando.catalog.transform.GetChild(currentid).GetComponent<Gallery_TandOButton>().hexType;
		leftPage.hexType=hexTypeL;

		if(tando.catalog.childCount-currentid==1)
			rightPage.transform.gameObject.SetActive(false);
		else
		{
			HexType hexTypeR=tando.catalog.transform.GetChild(currentid+1).GetComponent<Gallery_TandOButton>().hexType;
			rightPage.transform.gameObject.SetActive(true);
			rightPage.hexType=hexTypeR;
		}
		
		leftPage.UpdateTandO();
		rightPage.UpdateTandO();
	}
}
