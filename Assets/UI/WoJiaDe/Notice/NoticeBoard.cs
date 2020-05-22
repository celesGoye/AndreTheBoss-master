using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeBoard : MonoBehaviour
{
	public Transform content;
	
	private GameManager gameManager;
	
	public void OnEnable()
	{
		gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
	}
    public void OnConfirm()
	{
		this.gameObject.SetActive(false);
	}
}
