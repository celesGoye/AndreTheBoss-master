using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour

{
	public bool shake;
	public float intensity;
	private Vector3 deltaPos = Vector3.zero;


	// Use this for initialization

	void Start ()

	{

	}


	// Update is called once per frame

	void Update ()

	{

		transform.localPosition -= deltaPos;
		if(shake)
			deltaPos = Random.insideUnitSphere *intensity;
		else
			deltaPos=Vector3.zero;

		transform.localPosition += deltaPos;

	}

}