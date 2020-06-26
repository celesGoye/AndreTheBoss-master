using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPreview : MonoBehaviour
{
	public float width;
	public float height;
	public float offsetx;
	public float offsety;
	
	private CharacterReader characterReader;
	
	public Text skilldescription;
	public Text skillname;
	
	public void OnEnable()
	{
		if(characterReader == null)
			characterReader = FindObjectOfType<GameManager>().GetComponent<GameManager>().characterReader;
	}
	
	public void OnDisable()
	{
		this.GetComponent<RectTransform>().anchoredPosition =new Vector3(0,0,0);
	}
	
	
	
    void Update()
    {
        this.GetComponent<RectTransform>().anchoredPosition =Input.mousePosition+new Vector3(offsetx,offsety,0);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,width*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,height*UnityEngine.Screen.height);
    }
	
	public void UpdatePreview(string type,int index)
	{
		if(characterReader==null)
			return;
		
		skillname.text=characterReader.GetMonsterSkillUI(type,index).name;
		skilldescription.text=characterReader.GetMonsterSkillUI(type,index).description;
	}
}
