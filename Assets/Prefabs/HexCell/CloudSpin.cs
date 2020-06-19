using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpin : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
	
	private Vector3 vector;
	private float speed;
	
	public void OnEnable()
	{
		vector=new Vector3(0f,0f,1f);
		speed=Random.Range(minSpeed,maxSpeed);
		this.transform.Rotate(vector,Random.Range(0,360));
	}

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(vector,speed*Time.deltaTime);
    }
}
