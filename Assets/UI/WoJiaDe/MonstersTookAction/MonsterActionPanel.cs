using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterActionPanel : MonoBehaviour
{
	public Image image;
	public Button button;
	
	public Color moveEnds;
	
	public int index;
	public float offset;
	public float size;
	//public int numperline;
	
	private GameManager gameManager;
	private Monster currentMonster;
	private Sprite sprite;
	
	public void OnEnable()
	{
		if(gameManager==null)
			gameManager=FindObjectOfType<GameManager>();
	}
	
	public void UpdatePosition()
	{
		/*Vector2 v=new Vector2();
		if(index/numperline%2==0)
			v=new Vector2(-offset*(index-index/numperline*numperline),index/numperline*offset);
		else
			v=new Vector2(-offset*(index+0.5f-index/numperline*numperline),index/numperline*offset);*/
		
		Vector2 v=new Vector2(-offset*index,0);
		this.GetComponent<RectTransform>().anchoredPosition = v*UnityEngine.Screen.height;
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,size*UnityEngine.Screen.height);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,size*UnityEngine.Screen.height);
	}
	
	public void GetCurrentMonster(Monster monster)
	{
		currentMonster=monster;
	}
	
    public void SetActionPanel(bool isActive)
	{
		image.gameObject.SetActive(isActive);
		//button.transform.gameObject.SetActive(isActive);
	}
	
	public void UpdateActionPanel()
	{
		if(currentMonster==null)
			return;
		if((sprite=Resources.Load("UI/avatar/"+currentMonster.Name, typeof(Sprite)) as Sprite)!=null)
			image.sprite =sprite;
		else if((sprite=Resources.Load("UI/avatar/"+currentMonster.Name+currentMonster.GetLevel(), typeof(Sprite)) as Sprite)!=null)
			image.sprite=sprite;
		
		
		image.color=currentMonster.actionType==ActionType.MoveEnds?moveEnds:new Color(1,1,1,1);
	}
	
	public void ButtonOnActionPanel()
	{
        gameManager.gameCamera.FocusOnPoint(currentMonster.transform.position);
		gameManager.gameInteraction.UpdateSelectedPawn((Pawn)currentMonster);
	}
}
