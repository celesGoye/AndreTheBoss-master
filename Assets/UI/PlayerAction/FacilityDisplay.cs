using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacilityDisplay : MonoBehaviour
{
    public int index;
	public float width;
	public float height;
	public float size;
    public BuildingType type;
	
	public Image image;
	public Button button;
	public FacilityPallete facilityPallete;
	
	private Sprite sprite;
	
	public void OnEnable()
	{
		button.onClick.AddListener(OnFacilityButton);
	}
	
	public void InitFacilityDisplay()
	{
		if((sprite=Resources.Load("Image/galleryThings/buildings/"+type.ToString(), typeof(Sprite)) as Sprite)!=null)
		{
			image.sprite=sprite;
		}
	}
	
	public void OnFacilityButton()
	{
		facilityPallete.currentType=type;
		facilityPallete.currentMaxLevel=Building.GetMaxLevel(type);
		facilityPallete.ValidProduct=Building.GetValidProduct(type);
		facilityPallete.OnFacilityButton();
		
	}
	
	public void Update()
	{
		Vector2 v=new Vector2(0,-height*(index));
		this.GetComponent<RectTransform>().anchoredPosition = v*UnityEngine.Screen.height;
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,size*UnityEngine.Screen.height);
	}
}
