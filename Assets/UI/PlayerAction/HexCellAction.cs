using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexCellAction : MonoBehaviour
{
	public HexCell currentHexCell;
	public Text txtname;

    public void UpdateHexCellPanel(HexCell hexcell)
    {
		currentHexCell=hexcell;
        txtname.text = currentHexCell.hexType.ToString();
    } 
}
