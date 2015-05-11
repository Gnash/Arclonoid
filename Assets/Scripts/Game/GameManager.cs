using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	private static uint lastLevel = 3;

	public AudioClip destructionSound;
	public AudioClip ballLossSound;
	public AudioClip levelClearSound;
	public AudioClip gameOverSound;
	public AudioClip backgroundMusic;

	public Text gameOverText;

	public GameObject ballPrefab;
	public Vector3 ballSpawn;

	public static uint livesRemaining = 3;
	public uint currentLevel = 1;

	private AudioSource audioSource;

	private bool droppedBall = false;
	private bool destroyedBrick = false;
	private bool gameOver = false;


	void Awake() {		
		if (instance == null) {
			instance = this;
		}
		if (instance != this) {
			DestroyImmediate (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		DontDestroyOnLoad (gameObject);
		audioSource = GetComponent<AudioSource> ();
		spawnBall ();
	}

	public void spawnBall () {
		GameObject ball = Instantiate (ballPrefab);
		GameObject paddle = GameObject.FindGameObjectWithTag ("Paddle");
		paddle.GetComponent<PaddleControlScript> ().attachBall (ball);
	}
	
	// Update is called once per frame
	void Update () {
		if (droppedBall) {
			handleDroppedBall();
			droppedBall = false;
		}
		if (destroyedBrick) {
			checkForLevelClear();
			destroyedBrick = false;
		}
		if (gameOver && Input.GetKeyDown (KeyCode.Mouse0)) {
			Application.LoadLevel ("main_menu");
		}
	}

	private void handleDroppedBall() {
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Ball");
		if (balls.Length == 0) {
			loseLife();
		}
	}

	private void returnToMainScreen() {

	}

	private void loseLife() {
		livesRemaining--;
		if (livesRemaining > 0) {
			audioSource.PlayOneShot (ballLossSound);
			spawnBall();
		} else {
			pauseGame();
			StartCoroutine(initiateGameOver());
		}
	}

	private IEnumerator initiateGameOver() {
		audioSource.PlayOneShot(gameOverSound);
		yield return new WaitForSeconds (gameOverSound.length);
		gameOverText.enabled = true;
		gameOver = true;
	}

	private void checkForLevelClear() {
		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("Brick");
		if (bricks.Length == 0) {
			StartCoroutine(finishLevel());
		}
	}

	private IEnumerator finishLevel() {
		pauseGame ();
		yield return new WaitForSeconds (destructionSound.length);
		yield return new WaitForSeconds (0.3f);
		audioSource.PlayOneShot(levelClearSound);
		yield return new WaitForSeconds (levelClearSound.length);
		currentLevel++;
		if (currentLevel <= lastLevel) {
			Application.LoadLevel ("level_" + currentLevel);
		} else {
			Application.LoadLevel ("winning_screen");
		}
	}

	private void pauseGame() {
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Ball");
		for (int i = 0; i < balls.Length; i++) {
			GameObject ball = balls[i];
			BallMovement script = ball.GetComponent<BallMovement>();
			if (script != null) {
				script.pause();
			}
		}
	}
	
	public void notifyBrickDestruction() {
		playDestructionSound ();
		destroyedBrick = true;
	}
		
	private void playDestructionSound() {
		audioSource.PlayOneShot (destructionSound);
	}

	public void notifyBallLoss() {		
		droppedBall = true;
	}

	public void applyMultiplicationPowerup() {
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Ball");
		for (int i = 0; i < balls.Length; i++) {
			GameObject ball = balls[i];
			Vector2 originalVelocity = ball.GetComponent<Rigidbody2D>().velocity;
			GameObject clonedBallLeft = Instantiate(ball);
			clonedBallLeft.GetComponent<BallMovement>().setVelocity(Quaternion.Euler(0, 0, 45) * originalVelocity);
			GameObject clonedBallRight = Instantiate(ball);
			clonedBallRight.GetComponent<BallMovement>().setVelocity(Quaternion.Euler(0, 0, -45) * originalVelocity);
		}

	}
}
