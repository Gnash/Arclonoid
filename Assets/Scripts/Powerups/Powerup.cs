using UnityEngine;
using System.Collections;

public abstract class Powerup : MonoBehaviour {

	public float speed = 1.5f;

	void Start() {
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -1) * 1.5f;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name == "Paddle") {
			applyPowerupToGameManager();
			Destroy(gameObject);
		}
	}

	protected abstract void applyPowerupToGameManager();
}
