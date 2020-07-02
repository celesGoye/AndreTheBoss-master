using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_MemoryButton : MonoBehaviour
{
   public Gallery_MemoryPage memorypage;
   public int id;
   public int memoryId;
   
   private Text name;
   
   void Awake()
	{
		this.GetComponent<Button>().onClick.AddListener(OnMemoryBtn);
		
	}
	
   public void OnEnable()
   {
	   name=this.GetComponent<Text>();
   }
   
   public void UpdateMemoryButton(int index)
   {
	   id=index;
	   name.text="Memory "+memoryId;
	   //Debug.Log("index:"+index+"; mem:"+memoryId);
   }
   
   public void OnMemoryBtn()
	{
		memorypage.currentid=id%2==0?id:(id-1);
		memorypage.OnMemoryBtn();
	}
}
