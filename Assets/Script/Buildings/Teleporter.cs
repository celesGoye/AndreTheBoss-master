using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : Building
{
	public Teleporter another;
	public Transform Enterance1;
	public Transform Enterance2;
	
	private bool isValid;
	private GameManager gameManager;
	
	private Material invalidMat;
	public void OnEnable()
	{
		if(gameManager == null)
			gameManager = FindObjectOfType<GameManager>();
		isValid=true;
		invalidMat=new Material(Shader.Find("Unlit/Color"));
		invalidMat.color=new Color(0.5f,0.5f,0.5f,1f);
	}
		
	public void SetIsValid(bool isvalid)
	{
		isValid=isvalid;
		if(another.GetIsValid()!=isvalid)
			another.SetIsValid(isvalid);
		
		Enterance1.GetChild(0).gameObject.SetActive(isvalid);
		Enterance1.GetChild(1).gameObject.SetActive(!isvalid);

		Enterance2.GetChild(0).gameObject.SetActive(isvalid);
		Enterance2.GetChild(1).gameObject.SetActive(!isvalid);
		
	}
	
	public bool GetIsValid()
	{
		return isValid;
	}
	
	public override void OnPlayerTurnBegin()
	{
		SetIsValid(true);
	}
	
	public override void OnLevelUp()
	{
		if(currentLevel>another.GetCurrentLevel())
		{
			another.LevelUp();
		}
	}
	
    public static string GetDescription(int maxdistance)
	{
		string description="Max transmission distance:"+maxdistance;
		return description;
	}
	
	public override string GetUpgradeDescription()
	{
		string description = "Max.";
		if(currentLevel<Building.GetMaxLevel(buildingType))
			description="换个颜色，换种心情;)";
			//description="Max transmission distance:"+GetMaxDistance(currentLevel)+" → "+GetMaxDistance(currentLevel+1);
		return description;
	}
	
	public override void DestroyBuilding()
	{
		if(another!=null)
		{
			another.another=null;
			another.DestroyBuilding();
		}
		
		GameObject.DestroyImmediate(gameObject);
	}
	
	public static int GetMaxDistance(int level)
	{
		return level*5;
	}
	
	
}
