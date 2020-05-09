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
    //0,0
	public Text txtName;
	public Text txtLevel;
	public Text txtActionType;
	public Text txtRemainedStep;
	public Image imgAvatar;//这俩东西写在description里
	
	public MonsterManager monsterManager;
	
	private Sprite sprite;
	
    public void UpdatePawnStatusPanel(Pawn pawn)
    {	
        UpdatePanel(pawn.Type,pawn.currentAttack, pawn.currentDefense, pawn.currentHP, pawn.currentDexterity,
					pawn.currentAttackRange,pawn.Name,pawn.GetMaxHP(),pawn.GetLevel(),pawn.currentMagicAttack,
					pawn.currentMagicDefense,pawn.remainedStep,pawn.actionType);
    }
	
	//0,0
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
		txtActionType.text=actionType.ToString();
		if((sprite=Resources.Load("UI/avatar/avatar_"+name, typeof(Sprite)) as Sprite)!=null)
			imgAvatar.sprite =sprite;
		
		if(type==PawnType.Monster)
		{
			txtRemainedStep.transform.gameObject.SetActive(true);
			txtRemainedStep.text=""+remainedStep;
		}
		else
			txtRemainedStep.transform.gameObject.SetActive(false);
    }
}
