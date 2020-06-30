using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_TandOend : MonoBehaviour
{
	public Text txtname;
	public Text kind;
	public Text story;
	public Text effect;
	public Image image;
	
	public HexType hexType;
	
	private Sprite sprite;
	private OtherDescriptionReader otherDescriptionReader;
	private OtherDescriptionReader.OtherData otherData;
	
    public void OnEnable()
	{
		otherDescriptionReader=new OtherDescriptionReader();
	}
	
	public void UpdateTandO()
	{
		txtname.text=hexType.ToString();
		kind.text="Terrain";
		if((sprite=Resources.Load("Image/galleryThings/terrain/"+hexType.ToString(), typeof(Sprite)) as Sprite)!=null)
		{
			image.sprite=sprite;
		}
		
		otherData=otherDescriptionReader.GetTerrainData(hexType);
		story.text=otherData.description;
		effect.text=otherData.effect;
	}
}
