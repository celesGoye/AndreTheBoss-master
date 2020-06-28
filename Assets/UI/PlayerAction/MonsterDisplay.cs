using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDisplay : MonoBehaviour
{
    public int index;
	public float width;
	public float height;
	public float size;
    public MonsterType type;
	
	public Image image;
	public Text text;
	public Button button;
	public MonsterPallete monsterPallete;
	
	private Sprite sprite;
	
	public void OnEnable()
	{
		button.onClick.AddListener(OnMonsterBtn);	
	}
	
	public void UpdateMonsterDisplay()
	{
		if((sprite=Resources.Load("UI/avatar/"+type.ToString(), typeof(Sprite)) as Sprite)!=null)
			image.sprite =sprite;
		else if((sprite=Resources.Load("UI/avatar/"+type.ToString()+"5", typeof(Sprite)) as Sprite)!=null)
			image.sprite=sprite;
	}
	
	public void OnMonsterBtn()
	{
		monsterPallete.currentType=type;
		monsterPallete.OnMonsterButton();
	}
	
	public void Update()
	{
		Vector2 v=new Vector2(width*(index%4),-height*(index/4));
		this.GetComponent<RectTransform>().anchoredPosition = v*UnityEngine.Screen.height;
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,size*UnityEngine.Screen.height);
	}
	public void OnPointerEnter()
	{
		monsterPallete.OnPointerEnter(type);
	}
	public void OnPointerExit()
	{
		monsterPallete.OnPointerExit();
	}
}
