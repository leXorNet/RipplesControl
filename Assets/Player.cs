﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	Vector2 target = new Vector2();
	float timeStay = 0.0f;

	private float smoothTime = 0.2f;
	private Vector2 velocity = Vector2.zero;

	// Use this for initialization
	void Start () {
		Die ();
	}

	// Update is called once per frame
	void Update () {
		Vector2 newPosition = Vector2.SmoothDamp ((Vector2)transform.position, target, ref velocity, smoothTime, 10.0f, Time.deltaTime);
		transform.position = new Vector3 (newPosition.x, newPosition.y);
	}

	void Die() {
		transform.position = Vector3.zero;
		velocity = target = Vector2.zero;
	}

	float GetDamage(Collider2D other){
		var damage = other.GetComponent<Damage> ();
		if (damage == null) {
			return 0.0f;
		}

		return damage.Strength;
	}

	bool GetBlocker(Collider2D other){
		var blocker = other.GetComponent<Blocker> ();
		if (blocker == null) {
			return false;
		}

		return true;
	}

	float GetForce(Collider2D other, bool consume){
		var force = other.GetComponent<Force> ();
		if (force == null || force.IsConsumed()) {
			return 0.0f;
		}

		if (consume) {
			force.Consume();
		}

		return force.Strength;		
	}

	float GetStream(Collider2D other, out Vector2 direction){
		var stream = other.GetComponent<Stream> ();
		if (stream == null) {
			direction = new Vector2 ();
			return 0.0f;
		}

		direction = stream.Direction;
		return stream.Strength;		
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		//Debug.Log ("Collision with " + other.collider.name);

		var damage = GetDamage (other.collider);
		if (damage > 0) {
			Die ();
			return;
		}

		var blocker = GetBlocker (other.collider);
		if (blocker) {
			velocity = Vector2.zero;

			// Invert force
			var position = (Vector2)transform.position;
			var counterForce = (position - target).normalized;
			var counterStrength = 0.3f;

			target = position - Vector2.Reflect(counterForce, other.contacts[0].normal) * counterStrength;
			return;
		}

		var force = GetForce (other.collider, true);	// Todo: only consume if ripple?
		if (force > 0) {
			var myPosition = new Vector2 (transform.position.x, transform.position.y);
			var otherPosition = new Vector2 (other.transform.position.x, other.transform.position.y);
			var direction = myPosition - otherPosition;

			bool fixedMoveAngle = true;
			if (fixedMoveAngle) {
				float round = Mathf.Deg2Rad * 45;
				float angle = Mathf.Atan2 (direction.y, direction.x);

				if (angle % round != 0) {
					float newAngle = Mathf.Round (angle / round) * round;
					direction = new Vector2 (Mathf.Cos (newAngle), Mathf.Sin (newAngle));
				}
			}

			//Debug.Log(direction.normalized + " - " + force);
			target += direction.normalized * force;
			return;
		}


	}


	void OnCollisionStay2D(Collision2D other)
	{
		// Execute once every second.
		if (Time.time - timeStay > 1.0f) {
			timeStay = Time.time;	
		}

		// Execute every frame
		Vector2 streamDirection;
		var stream = GetStream (other.collider, out streamDirection);
		if (stream > 0) {
			target += streamDirection * stream;
		}
	}
}
