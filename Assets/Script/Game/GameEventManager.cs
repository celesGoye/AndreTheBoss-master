using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameEventManager : MonoBehaviour
{
	GameEventReader eventReader;

	//List<GameEvent> gameEvents;

	GameObject EventRoot;

	public GameEventDisplayer pref_gameEventDisplayer;

	List<GameEventDisplayer> gameEventDisplayers;

	private GameManager gm;

	public int MaxEventOnMap = 5;

	public void OnEnable()
	{
		if (gm == null)
			gm = FindObjectOfType<GameManager>();
	}

	public void InitGameEventManager()
	{
		eventReader = new GameEventReader();
		//gameEvents = new List<GameEvent>();
		gameEventDisplayers = new List<GameEventDisplayer>();

		EventRoot = new GameObject("Event Root");
		EventRoot.transform.SetParent(this.transform);
		EventRoot.transform.localPosition = Vector3.zero;

		//eventReader.TestGameEvent();
	}
	public void GenerateEvent()
	{
		int totalEventType = (int)GameEventType.GameEventNum;
		int which = UnityEngine.Random.Range(0, totalEventType);
		int eventNum = eventReader.getTotalEventNumOfType(which);
		int whichEvent = UnityEngine.Random.Range(0, eventNum);
		GameEventType type = (GameEventType)which;
		GameEvent gameEvent = eventReader.getNewGameEvent(type, whichEvent);

		GameEventDisplayer displayer = CreateNewEventDisplayer(gameEvent);
		if (displayer != null)
			gameEventDisplayers.Add(displayer);
	}

	public GameEventDisplayer CreateNewEventDisplayer(GameEvent gameEvent)
	{
		if(gameEvent != null)
		{
			GameEventDisplayer newDisplayer = GameObject.Instantiate(pref_gameEventDisplayer);
			newDisplayer.transform.SetParent(EventRoot.transform);
			HexCell cell = gm.hexMap.GetRandomCellToSpawn();
			gm.hexMap.SetGameEventDisplayerCell(newDisplayer, cell);
			newDisplayer.gameEvent = gameEvent;

			//gm.hexMap.RevealCell(cell);
			gm.gameCamera.FocusOnPoint(cell.transform.localPosition);

			return newDisplayer;
		}

		return null;
	}

	public void OnTurnBegin()
	{
		foreach (GameEventDisplayer displayer in gameEventDisplayers)
		{
			GameEvent gameEvent = displayer.gameEvent;
			gameEvent.counter--;
		}



		int shouldgen = MaxEventOnMap - gameEventDisplayers.Count;
		// generate events
		for(int i = 0; i < shouldgen; i++)
		{
			GenerateEvent();
		}
	}

	public void OnTurnEnd()
	{
		List<GameEventDisplayer> shouldRemove = new List<GameEventDisplayer>();
		foreach(GameEventDisplayer displayer in gameEventDisplayers)
		{
			GameEvent gameEvent = displayer.gameEvent;
			
			if(gameEvent.counter <= 0)
			{
				shouldRemove.Add(displayer);
			}
		}

		foreach(GameEventDisplayer displayer in shouldRemove)
		{
			displayer.currentCell.gameEventDisplayer = null;
			gameEventDisplayers.Remove(displayer);

			GameObject.DestroyImmediate(displayer.gameObject);
		}
	}

}

