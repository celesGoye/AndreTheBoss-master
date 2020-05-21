using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery_EncounterPage : MonoBehaviour
{
	public List<string> encounterList;
	public int EncounterCount;
	public Transform Encounters;
    // Start is called before the first frame update
    void Start()
    {
        encounterList.Add("first");
        encounterList.Add("second");
        encounterList.Add("third");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
