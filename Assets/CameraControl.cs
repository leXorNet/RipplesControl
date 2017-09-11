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
		cameraPos.y = Player.position.y - 7.0f; // + Screen.currentResolution.height / 4;
		cameraPos.z = Camera.main.transform.position.z;

		Camera.main.transform.position = cameraPos;
	}

	public Vector3 ScreenToGround(Vector3 point)
	{
		Vector3 res = Vector3.zero;
		Ray r = Camera.main.ScreenPointToRay(point);
		Plane groundPlane = new Plane(Vector3.forward, Vector3.zero);
		float rayDistance;
		if (groundPlane.Raycast(r, out rayDistance))
		{
			res = r.GetPoint(rayDistance);
		}
		return res;

		//return Camera.main.ScreenToWorldPoint (point);
	}


}
