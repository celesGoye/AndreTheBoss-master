using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_MemoryPage : MonoBehaviour
{
	public List<string> memoryList;
	public int MemoryCount;
	public Transform Memories;
    // Start is called before the first frame update
    void Start()
    {
        memoryList.Add("first");
        memoryList.Add("second");
        memoryList.Add("third");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
