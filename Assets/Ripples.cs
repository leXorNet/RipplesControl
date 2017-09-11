using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ripples : MonoBehaviour {
	
	public Ripple RipplePrefab;
	float timeDown = 0.0f;

	CameraControl cameraControl;

	// Use this for initialization
	void Start () {
		cameraControl = Camera.main.GetComponent<CameraControl>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			timeDown = Time.time;

		} else if (Input.GetMouseButtonUp (0)) {
			var ripplePos = cameraControl.ScreenToGround(Input.mousePosition);
			ripplePos.z = 0;

			var rippleInstance = Instantiate (RipplePrefab);
			rippleInstance.transform.SetParent (transform);
			rippleInstance.transform.position = ripplePos;

			if (Time.time - timeDown > 0.25f) {
				rippleInstance.SetLevel (2);
			} else {
				rippleInstance.SetLevel (1);
			}
		}

	}
}
