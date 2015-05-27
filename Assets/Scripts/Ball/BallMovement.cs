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
	private Vector2 currentMovement;

	private Vector3 hitBrickPosition;

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
		hitBrickPosition = Vector3.zero;
	}

	private void updateVelocity() {
		rigidBody.velocity = rigidBody.velocity.normalized * speed;
		currentMovement = rigidBody.velocity;
	}

	public void setVelocity(Vector2 velocity) {
		startDirection = velocity;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (stuckInPaddle) {
			return;
		}
		if (collision.gameObject.name == "Paddle") {
			handlePaddleCollision (collision);
		} else if (collision.gameObject.tag == "Brick") {
			handleBrickCollision(collision);
		}
	}

	private void handlePaddleCollision(Collision2D collision) {
		increaseSpeed ();
		stuckInPaddle = true;
		float x = hitFactor (transform.position, collision.transform.position, collision.collider.bounds.size.x);
		if (rigidBody && rigidBody.velocity != Vector2.zero) {
			rigidBody.velocity = new Vector2 (x, up).normalized * speed;
		}
	}

	private void handleBrickCollision(Collision2D collision) {
		Vector3 collidingBrickPosition = collision.gameObject.transform.position;
		if (hitBrickPosition == Vector3.zero) {
			hitBrickPosition = collidingBrickPosition;
		} else if (collidingBrickPosition != hitBrickPosition) {
			if (Mathf.Abs(collidingBrickPosition.x - hitBrickPosition.x) < Mathf.Epsilon) {
				invertYDirection();
				Debug.Log("InvertY");
			} else if (Mathf.Abs(collidingBrickPosition.y - hitBrickPosition.y) < Mathf.Epsilon) {
				invertXDirection();
				Debug.Log("InvertX");
			}
		}
	}
	
	private void invertYDirection() {
		lastDirection.y *= -1;
		setVelocity (lastDirection);
	}
	
	private void invertXDirection() {
		lastDirection.x *= -1;
		setVelocity (lastDirection);
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
