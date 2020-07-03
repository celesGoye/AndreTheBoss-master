using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DismissPanel : MonoBehaviour
{
    public Text name;
	public void OnEnable()
	{
		name.text=FindObjectOfType<GameManager>().gameInteraction.menu.currentMonster.ToString()+"?";
	}
}
