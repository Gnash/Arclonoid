using UnityEngine;
using System.Collections;

public class BallMovement : MonoBehaviour {

	public float spawnSpeed = 6;
	public float currentSpeed;
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
		currentSpeed = spawnSpeed;
		rigidBody = GetComponent<Rigidbody2D> ();
		rigidBody.velocity = startDirection;
	}

	void Update() {
		UpdateVelocity ();
		hitBrickPosition = Vector3.zero;
	}

	private void UpdateVelocity() {
		rigidBody.velocity = rigidBody.velocity.normalized * currentSpeed;
		currentMovement = rigidBody.velocity;
	}

	public void SetVelocity(Vector2 velocity) {
		startDirection = velocity;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (stuckInPaddle) {
			return;
		}
		if (collision.gameObject.name == "Paddle") {
			HandlePaddleCollision (collision);
		} else if (collision.gameObject.tag == "Brick") {
			HandleBrickCollision(collision);
		}
	}

	private void HandlePaddleCollision(Collision2D collision) {
		IncreaseSpeed ();
		stuckInPaddle = true;
		float x = HitFactor (transform.position, collision.transform.position, collision.collider.bounds.size.x);
		if (rigidBody && rigidBody.velocity != Vector2.zero) {
			rigidBody.velocity = new Vector2 (x, up).normalized * currentSpeed;
		}
	}

	private void HandleBrickCollision(Collision2D collision) {
		Vector3 collidingBrickPosition = collision.gameObject.transform.position;
		if (hitBrickPosition == Vector3.zero) {
			hitBrickPosition = collidingBrickPosition;
		} else if (collidingBrickPosition != hitBrickPosition) {
			if (Mathf.Abs(collidingBrickPosition.x - hitBrickPosition.x) < Mathf.Epsilon) {
				InvertYDirection();
				Debug.Log("InvertY");
			} else if (Mathf.Abs(collidingBrickPosition.y - hitBrickPosition.y) < Mathf.Epsilon) {
				InvertXDirection();
				Debug.Log("InvertX");
			}
		}
	}
	
	private void InvertYDirection() {
		currentMovement.y *= -1;
		SetVelocity (currentMovement);
	}
	
	private void InvertXDirection() {
		currentMovement.x *= -1;
		SetVelocity (currentMovement);
	}

	private void IncreaseSpeed() {
		reboundCount++;
		if (reboundCount > freeRebounds) {
			currentSpeed += speedIncrease;
		}
	}

	private float HitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleWidth) {
		return (ballPos.x - paddlePos.x) / paddleWidth;
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.name == "Paddle") {
			stuckInPaddle = false;
		}
	}

	public void ResetSpeed() {
		this.currentSpeed = spawnSpeed;
	}

	public void Pause() {
		if (rigidBody) {
			lastDirection = rigidBody.velocity.normalized;
			rigidBody.velocity = Vector2.zero;
		}
	}

	public void Unpause() {
		if (rigidBody) {
			rigidBody.velocity = lastDirection;
		}
	}
}
