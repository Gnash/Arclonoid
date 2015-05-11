using UnityEngine;
using System.Collections;

public class BallMovement : MonoBehaviour {

	public float speed = 6;
	public float speedIncrease = 0.2f;
	public float freeRebounds = 10;
	public float up = 0.5f;
	private Rigidbody2D rigidBody;
	private bool stuckInPaddle = false;
	private uint reboundCount = 0;

	private Vector2 lastDirection;
	private Vector2 startDirection;

	void Awake() {
		lastDirection = new Vector2 (0.5f, 1).normalized;
	}

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		rigidBody.velocity = startDirection;
	}

	void Update() {
		updateVelocity ();
	}

	private void updateVelocity() {
		rigidBody.velocity = rigidBody.velocity.normalized * speed;
	}

	public void setVelocity(Vector2 velocity) {
		startDirection = velocity;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.name != "Paddle") {
			return;
		}
		if (stuckInPaddle) {
			return;
		}
		increaseSpeed ();
		stuckInPaddle = true;
		float x = hitFactor (transform.position, collision.transform.position, collision.collider.bounds.size.x);
		if (rigidBody && rigidBody.velocity != Vector2.zero) {
			rigidBody.velocity = new Vector2 (x, up).normalized * speed;
		}
	}

	private void increaseSpeed() {
		reboundCount++;
		if (reboundCount > freeRebounds) {
			speed += speedIncrease;
		}
	}

	private float hitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleWidth) {
		return (ballPos.x - paddlePos.x) / paddleWidth;
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.name == "Paddle") {
			stuckInPaddle = false;
		}
	}

	public void pause() {
		if (rigidBody) {
			lastDirection = rigidBody.velocity.normalized;
			rigidBody.velocity = Vector2.zero;
		}
	}

	public void unpause() {
		if (rigidBody) {
			rigidBody.velocity = lastDirection;
		}
	}
}
