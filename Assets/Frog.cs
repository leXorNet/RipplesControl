using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour {

	public Ripple RipplePrefab;
	float time = 0.0f;
	float spawnRate = 0.25f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		time += Time.deltaTime;

		if (time >= spawnRate) {
			var rippleInstance = Instantiate (RipplePrefab);
			//rippleInstance.transform.SetParent (transform);
			rippleInstance.transform.position = transform.position;
			rippleInstance.SetLevel (1);

			time = 0.0f;
		}
	}
}
