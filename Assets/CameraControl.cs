using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public Transform Player;


	void Start () {
	}

	void Update () {
		// Have camera follow player
		Vector3 cameraPos = new Vector3();
		cameraPos.x = Camera.main.transform.position.x;
		cameraPos.y = Player.position.y + 2.0f; // + Screen.currentResolution.height / 4;
		cameraPos.z = Camera.main.transform.position.z;

		Camera.main.transform.position = cameraPos;
	}

	public Vector3 ScreenToGround(Vector3 point)
	{
		return Camera.main.ScreenToWorldPoint (point);
	}
}
