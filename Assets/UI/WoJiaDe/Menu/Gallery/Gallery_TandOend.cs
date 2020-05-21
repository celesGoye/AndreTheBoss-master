using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_TandOend : MonoBehaviour
{
	public Text name;
	public Text kind;
	public Text story;
	public Text effect;
	public Image image;
	
	public bool isTerrain;
	public HexType hexType;
    
	public void UpdateTandO()
	{
		name.text=hexType.ToString();
		kind.text="Terrain";
	}
}
