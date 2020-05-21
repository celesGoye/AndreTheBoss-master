using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_TandOButton : MonoBehaviour
{
	public bool isTerrain;
	public HexType hexType;
	public Gallery_TandOPage tandoPage;
	public int id;
	void Awake()
	{
		this.GetComponent<Button>().onClick.AddListener(OnTandOBtn);
		id=this.transform.GetSiblingIndex();
	}
	public void OnTandOBtn()
	{
		tandoPage.currentid=id-id%2;
		tandoPage.OnTandOBtn();
		
		tandoPage.theTandOPage.UpdateTandO();
	}
}
