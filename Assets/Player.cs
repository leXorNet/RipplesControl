using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	Vector2 target = new Vector2();

	public float smoothTime = 0.3f;
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

			bool fixedMoveSpeed = false;
			if (fixedMoveSpeed) {
				force *= direction.magnitude * 0.3f;
			}

			//Debug.Log(direction.normalized + " - " + force);
			target += direction.normalized * force;
			return;
		}
	}

}
