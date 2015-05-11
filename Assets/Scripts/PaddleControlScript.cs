using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaddleControlScript : MonoBehaviour {

	public AudioClip ballReboundSound;

	private float paddleWidth;
	private IList<GameObject> attachedBalls;
	private AudioSource audioSource;

	void Awake() {
		attachedBalls = new List<GameObject> ();
	}

	// Use this for initialization
	void Start () {
		paddleWidth = GetComponent<BoxCollider2D> ().size.x / 2;
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePosScreen = Input.mousePosition;
		Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint (mousePosScreen);
		float xMouse = mousePosWorld.x;
		if (xMouse - paddleWidth < -2.9f) {
			xMouse = -2.9f + paddleWidth;
		} else if (xMouse + paddleWidth > 2.9f) {
			xMouse = 2.9f - paddleWidth;
		}
		transform.position = new Vector3 (xMouse, 0, 0);
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			fireBall();
		}
	}

	private void fireBall() {
		if (attachedBalls.Count > 0) {
			GameObject ballToFire = attachedBalls[0];
			ballToFire.GetComponent<BallMovement>().unpause();
			ballToFire.GetComponent<Collider2D>().enabled = true;
			ballToFire.transform.parent = null;
			attachedBalls.RemoveAt(0);
		}
	}

	public void attachBall(GameObject ball) {
		ball.transform.parent = transform;
		ball.transform.localPosition = new Vector3(0, 0.175f, 0);
		ball.GetComponent<Collider2D> ().enabled = false;

		BallMovement ballScript = ball.GetComponent<BallMovement> ();
		ballScript.pause ();

		attachedBalls.Add (ball);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		audioSource.PlayOneShot(ballReboundSound);
		if (collision.gameObject.tag == "Ball") {
			audioSource.PlayOneShot(ballReboundSound);
		}
	}
}
