using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gm;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameComplete()
    {
        Animator animator = gm.GetComponent<Animator>();
        animator.SetBool("LoadComplete", true);
    }
}
