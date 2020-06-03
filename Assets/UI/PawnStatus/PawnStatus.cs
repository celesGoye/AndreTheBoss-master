using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PawnStatus : MonoBehaviour
{
    public Text txtAttak;
    public Text txtDefense;
    public Text txtHP;
    public Text txtDexterity;
    public Text txtAttackRange;
	public Text txtMagic;
	public Text txtResistant;
    
	public Text txtName;
	public Text txtLevel;
	public Text txtActionType;
	public Text txtRemainedStep;
	public Image imgAvatar;
	
	private Sprite sprite;
	private Pawn currentPawn;
	
    public void UpdatePawnStatusPanel(Pawn pawn)
    {	
		currentPawn=pawn;
		if(pawn as Monster!=null)
			UpdatePanel(pawn.pawnType,pawn.currentAttack, pawn.currentDefense, pawn.currentHP, pawn.currentDexterity,
					pawn.currentAttackRange,pawn.Name,pawn.GetMaxHP(),pawn.GetLevel(),pawn.currentMagicAttack,
					pawn.currentMagicDefense,((Monster)pawn).remainedStep,((Monster)pawn).actionType);
		else
			UpdatePanel(pawn.pawnType,pawn.currentAttack, pawn.currentDefense, pawn.currentHP, pawn.currentDexterity,
					pawn.currentAttackRange,pawn.Name,pawn.GetMaxHP(),pawn.GetLevel(),pawn.currentMagicAttack,
					pawn.currentMagicDefense,0,ActionType.NoAction);
    }
	
	public void UpdatePawnStatusPanel()
    {	
		if(currentPawn!=null)
        UpdatePanel(currentPawn.pawnType,currentPawn.currentAttack, currentPawn.currentDefense, currentPawn.currentHP, currentPawn.currentDexterity,
					currentPawn.currentAttackRange,currentPawn.Name,currentPawn.GetMaxHP(),currentPawn.GetLevel(),currentPawn.currentMagicAttack,
					currentPawn.currentMagicDefense,((Monster)currentPawn).remainedStep,((Monster)currentPawn).actionType);
    }
	
    private void UpdatePanel(PawnType type,int attack, int def, int hp, int dex, int atkRange,string name,int maxHp,int level,int magic,int resistance,int remainedStep,ActionType actionType)
    {
        txtAttak.text ="ATK:"+ attack;
        txtDefense.text ="DEF:"+ def;
		if((float)hp/maxHp<0.4f)
			txtHP.text ="<color=#FF0000>"+ hp+"</color>/"+maxHp;
		else
			txtHP.text =hp+"/"+maxHp;
        txtDexterity.text = "DEX:"+dex;
        txtAttackRange.text = "RNG:"+atkRange;
		txtMagic.text="MAG:"+magic;
		txtResistant.text="RES:"+resistance;
        txtName.text = ""+name;
		txtLevel.text="."+level;
		if((sprite=Resources.Load("UI/avatar/avatar_"+name, typeof(Sprite)) as Sprite)!=null)
			imgAvatar.sprite =sprite;
		
		if(type==PawnType.Monster)
		{
			txtRemainedStep.gameObject.SetActive(true);
			txtRemainedStep.text=""+remainedStep;
			txtActionType.gameObject.SetActive(true);
			txtActionType.text=actionType.ToString();
		}
		else
		{
		txtRemainedStep.transform.gameObject.SetActive(false);
		txtActionType.transform.gameObject.SetActive(false);
		}
    }
}
