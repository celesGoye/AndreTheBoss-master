using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexCellStatusAndAction : MonoBehaviour
{
    public Text txtDescribe;
    public Text txtType;
	
	public HexCell currentHex;
	
	private GameManager gameManager;

    public void UpdateHexStatusPanel(HexCell hexCell)
    {
        UpdatePanel(hexCell.hexType);
    }
    private void UpdatePanel(HexType hexType)
    {
        txtDescribe.text="describe".ToString();
		txtType.text=hexType.ToString();
    }
}
