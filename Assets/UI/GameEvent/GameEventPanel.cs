using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameEventPanel : MonoBehaviour
{
    public GameEvent gameEvent;
	public Monster currentMonster;
	public HexCell currentCell;
	public GameEventDisplayer displayer;
	
	public Text name;
	public Text description;
	public Text result;
	public Image cg;
	public Transform optionPart;
	public Transform nonoptionPart;
	public Transform mainPanel;
	public Transform resultPanel;
	
	public Text effect;
	
	public int optionCount;
	public List<Button> optionsButton;
	
	private string imagePath="Image/event/";
	
	private BuffEntry currentBuff;
	private List<ItemEntry> currentItems;
	private List<GameEventOption> currentOptions;
	private MonsterType currentMonsterType;
	private int currentMonsterLevel;
	private string effectString;
	
	private GameManager gm;
	
	GameEventReader eventReader;
	
	public void OnEnable()
	{
		if(gm==null)
			gm=GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
	}
	
	public void UpdateGameEventPanel(GameEvent gameEvent)
	{
		Clear();
		mainPanel.gameObject.SetActive(true);
		this.gameEvent=gameEvent;
		switch((int)gameEvent.eventType)
		{
			case 0:
				try
				{
					NormalNonoptionGainbuffEvent convertedEvent=(NormalNonoptionGainbuffEvent)gameEvent;		
					EnableNonoptionPart();
					currentBuff=convertedEvent.GetBuff();					
					effectString=convertedEvent.GetEffectDescription();
					effect.text=effectString;
				} catch (InvalidCastException ex)
				{
                Debug.Log(ex.StackTrace);
				}
				break;
			case 1:
				try
				{
					NormalNonoptionGainitemsEvent convertedEvent=(NormalNonoptionGainitemsEvent)gameEvent;		
					EnableNonoptionPart();
					currentItems=convertedEvent.GetItems();
					effectString=convertedEvent.GetEffectDescription();
					effect.text=effectString;
				} catch (InvalidCastException ex)
				{
                Debug.Log(ex.StackTrace);
				}
				break;
			case 2:				
				try
				{
					NormalOptionEvent convertedEvent=(NormalOptionEvent)gameEvent;		
					EnableOptionPart();
					currentOptions=convertedEvent.GetOptions();
					for(int i=0;i<optionsButton.Count;i++)
					{
						if(i<currentOptions.Count)
						{
							optionsButton[i].gameObject.SetActive(true);
							optionsButton[i].GetComponent<Text>().text=currentOptions[i].name;
						}
						else
							optionsButton[i].gameObject.SetActive(false);
					}
				} catch (InvalidCastException ex)
				{
                Debug.Log(ex.StackTrace);
				}

				break;
			case 3:
				try
				{
					MysterypersonGainitemsEvent convertedEvent=(MysterypersonGainitemsEvent)gameEvent;		
					EnableOptionPart();
					currentOptions=convertedEvent.GetOptions();
					for(int i=0;i<optionsButton.Count;i++)
					{
						if(i<currentOptions.Count)
						{
							optionsButton[i].gameObject.SetActive(true);
							optionsButton[i].GetComponent<Text>().text=currentOptions[i].name;
						}
						else
							optionsButton[i].gameObject.SetActive(false);
					}
				} catch (InvalidCastException ex)
				{
                Debug.Log(ex.StackTrace);
				}
				break;
			case 4:
				try
				{
					MysterypersonGaincharacterEvent convertedEvent=(MysterypersonGaincharacterEvent)gameEvent;		
					EnableOptionPart();
					currentMonsterType=convertedEvent.GetMonsterType();
					currentMonsterLevel=convertedEvent.GetLevel();
					currentOptions=convertedEvent.GetOptions();
					for(int i=0;i<optionsButton.Count;i++)
					{
						if(i<currentOptions.Count)
						{
							optionsButton[i].gameObject.SetActive(true);
							optionsButton[i].GetComponent<Text>().text=currentOptions[i].name;
						}
						else
							optionsButton[i].gameObject.SetActive(false);
					}
				} catch (InvalidCastException ex)
				{
                Debug.Log(ex.StackTrace);
				}
				break;
			default:
				break;
		}
		name.text=gameEvent.eventName;
		description.text=gameEvent.eventDescription;
		eventReader = new GameEventReader();
		Sprite sprite;
		if((sprite=Resources.Load(imagePath+eventReader.GetGameEventIndex(gameEvent), typeof(Sprite)) as Sprite)!=null)
		{
			cg.sprite=sprite;
		}
	}
	
	public void Clear()
	{
		nonoptionPart.gameObject.SetActive(false);
		optionPart.gameObject.SetActive(false);
		resultPanel.gameObject.SetActive(false);
		currentBuff=null;
		currentItems=null;
		currentOptions=null;
		currentMonsterType=MonsterType.NUM;
		effectString="";
	}
	
	public void EnableOptionPart()
	{
		optionPart.gameObject.SetActive(true);
	}
	
	public void EnableNonoptionPart()
	{
		nonoptionPart.gameObject.SetActive(true);
	}
	
	public void OnContinue()
	{
		resultPanel.gameObject.SetActive(true);
		mainPanel.gameObject.SetActive(false);
		result.text="";
		if(currentBuff!=null)
		{
			currentMonster.addBuff(currentBuff.attributeType,currentBuff.value,currentBuff.counter);
			currentMonster.UpdateCurrentValue();
			result.text+=currentMonster.ToString()+":"+effectString+"\n";
		}
		if(currentItems!=null)
		{
			List<ItemEntry> finalItems=new List<ItemEntry>();
			float posibility = (float)UnityEngine.Random.Range(1, 100);
			int sum=0;
			foreach(ItemEntry item in currentItems)
			{
				if(item.posibility==100)
				{
					finalItems.Add(item);
					continue;
				}
				sum+=item.posibility;
				if(posibility<sum)
				{
					finalItems.Add(item);
					break;
				}
			}
			if(finalItems.Count>0)
			{
				result.text+="Get ";
				foreach(ItemEntry item in finalItems)
				{
					gm.itemManager.GetItem(item.itemType, item.number);
					result.text+=TextColor.SetTextColor(item.itemType.ToString(),TextColor.ItemColor)+"*"+item.number+";";
				}
			}
		}
	}
	
	public void OnButtonA()
	{
		resultPanel.gameObject.SetActive(true);
		mainPanel.gameObject.SetActive(false);
		OnOptionSelect(currentOptions[0]);
	}
	
	public void OnButtonB()
	{
		resultPanel.gameObject.SetActive(true);
		mainPanel.gameObject.SetActive(false);
		OnOptionSelect(currentOptions[1]);
	}
	
	public void OnButtonC()
	{
		resultPanel.gameObject.SetActive(true);
		mainPanel.gameObject.SetActive(false);
		OnOptionSelect(currentOptions[2]);
	}
	
	public void OnOptionSelect(GameEventOption option)
	{
		if(option==null)
			return;
		
		result.text="";
		if(option.result!=null)
			result.text+=option.result+"\n";
		
		if(option.items!=null)
		{
			List<ItemEntry> finalItems=new List<ItemEntry>();
			float posibility = (float)UnityEngine.Random.Range(1, 100);
			int sum=0;
			foreach(ItemEntry item in option.items)
			{
				if(item.posibility==100)
				{
					finalItems.Add(item);
					continue;
				}
				sum+=item.posibility;
				if(posibility<sum)
				{
					finalItems.Add(item);
					break;
				}
			}
			if(finalItems.Count>0)
			{
				result.text+="Get ";
				foreach(ItemEntry item in finalItems)
				{
					gm.itemManager.GetItem(item.itemType, item.number);
					result.text+=TextColor.SetTextColor(item.itemType.ToString(),TextColor.ItemColor)+"*"+item.number+";";
				}
				result.text+="\n";
			}
		}
		if(option.buffs!=null)
		{
			foreach(BuffEntry buff in option.buffs)
			{
				currentMonster.addBuff(buff.attributeType,buff.value,buff.counter);
				currentMonster.UpdateCurrentValue();
			}
		}
		if(option.costs!=null)
		{
			List<ItemEntry> finalItems=new List<ItemEntry>();
			foreach(ItemEntry item in option.costs)
			{
				if(gm.itemManager.IsHaveEnoughItem(item.itemType, item.number))
					finalItems.Add(item);
				else
				{
					finalItems.Clear();
					break;
				}
			}
			if(finalItems.Count>0)
			{
				result.text+="Paid ";
				foreach(ItemEntry item in finalItems)
				{
					gm.itemManager.ConsumeItem(item.itemType, item.number);
					result.text+=TextColor.SetTextColor(item.itemType.ToString(),TextColor.ItemColor)+"*"+item.number+";";
				}
				result.text+="\n";
			}
			
			if(currentMonsterType!=MonsterType.NUM)
			{
				gm.monsterManager.CreateMonster(currentMonsterType, gm.hexMap.GetEmptyNearestCellAround(currentCell), currentMonsterLevel);
			}
		}
		if(result.text=="")
		{
			result.text+="一般通过Andre";
		}
		
	}
	
	public void OnOK()
	{
		gm.gameEventManager.CloseEvent(displayer);
		this.gameObject.SetActive(false);
	}
}
