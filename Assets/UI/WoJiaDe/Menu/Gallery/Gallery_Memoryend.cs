using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gallery_Memoryend : MonoBehaviour
{
    public int memoryId;
	
	public Text name;
	public Text content;
	
	private MemoryReader reader;
	public void UpdateItem()
	{
		name.text="Memory"+memoryId;
		
		reader=new MemoryReader();
		content.text=reader.GetEventMemoryData(memoryId);
		
	}
}
