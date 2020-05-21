using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : Building
{
	public bool isvalid;
	
	public Teleporter another;
	
	private GameManager gameManager;
	public void OnEnable()
	{
		if(gameManager == null)
			gameManager = FindObjectOfType<GameManager>();
	}
	
	public void InitTeleporter()
	{
		
		
	}
	
	public static int GetMaxDistance(int level)
	{
		return level*5;
	}
	
	public void ShowTeleporterBuildableHex()
	{
		
	}
}
