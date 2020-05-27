using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUpgradePanel : MonoBehaviour
{
	public Text txtname;
	public Text txtlevel;
	public Text txtlevelafter;
	public Text txtdescription;
	public Upgrade_Item soul;
	
	public float width;
	public float size;
	
	private int requireSoul;
	private GameManager gameManager;
	private Building building;
	
    public void UpdateBuildingUpgradePanel(HexCell hexCell)
	{
		building=hexCell.building;
		txtname.text=building.GetBuildingType().ToString();
		txtlevel.text="Lv."+building.GetCurrentLevel();
		txtlevelafter.text="Lv."+(building.GetCurrentLevel()+1);
		txtdescription.text="<size=22>"+building.GetUpgradeDescription()+"</size>";
		if(gameManager==null)
			gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
		requireSoul= Building.GetRequireSouls(building.GetBuildingType(),building.GetCurrentLevel());
		soul.num=gameManager.itemManager.ItemsOwn[ItemType.Soul];
		soul.numneed=requireSoul;
		
		UpdateItem(soul);
	}
		
	public void UpdateItem(Upgrade_Item item)
	{
		item.index=0;
		item.size=size;
		item.width=width;
	}

	public void OnUpgrade()
	{
		gameManager.itemManager.ConsumeItem(ItemType.Soul, building.LevelUp());
        gameManager.gameInteraction.Clear();
	}
}
