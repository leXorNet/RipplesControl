using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ripple : MonoBehaviour {

	private Force force;
	public float Duration = 0.5f;

	bool started = false;
	float time = 0.0f;

	Vector2 startScale = new Vector2 (0.05f, 0.05f);
	Vector2 targetScale;

	// Use awake since its called at the time of instantiation
	void Awake () {
		force = GetComponent<Force> ();
		transform.localScale = new Vector3 (startScale.x, startScale.y);
	}
	
	// Update is called once per frame
	void Update () {

		if (started) {
			time += Time.deltaTime;

			Vector2 newScale = Vector2.Lerp(startScale, new Vector2(force.Range, force.Range), Mathf.Max (0.0f, time / Duration));
			transform.localScale = new Vector3 (newScale.x, newScale.y);

			if (time >= Duration) {
				Destroy (this.gameObject);
			}
		}

	}

	public void SetLevel(int level) {
		//Debug.Log("Ripple level " + level);

		if (level == 1) {
			force.Strength = 1.0f;
			force.Range = 0.5f;

		} else if (level == 2) {
			force.Strength = 2.0f;
			force.Range = 1.0f;
			//Duration = 0.5f;

			SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
			spriteRenderer.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		}

		time = 0.0f;
		started = true;
	}
}