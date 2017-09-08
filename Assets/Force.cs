using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour {

	public float Strength = 1.0f;
	public float Range = 1.0f;
	private bool consumed = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Consume()
	{
		consumed = true;
	}

	public bool IsConsumed()
	{
		return consumed;
	}
}
