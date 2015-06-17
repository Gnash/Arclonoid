using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private const float SPAWN_CHANCE = 0.1f;

	public static GameManager instance = null;

	private static uint lastLevel = 3;

	public AudioClip destructionSound;
	public AudioClip ballLossSound;
	public AudioClip levelClearSound;
	public AudioClip gameOverSound;
	public AudioClip backgroundMusic;

	public GameObject[] powerups;
	public GameObject smallPaddlePrefab;
	public GameObject normalPaddlePrefab;
	public GameObject largePaddlePrefab;

	public Text gameOverText;

	public GameObject ballPrefab;
	public Vector3 ballSpawn;

	private const uint START_LIVES = 3; 
	public static uint livesRemaining = 3;
	public uint currentLevel = 1;

	private AudioSource audioSource;

	private bool droppedBall = false;
	private bool mustSpawnBall = false;
	private bool destroyedBrick = false;
	private bool gameOver = false;
	private bool paddleIsShrunk = false;
	private bool paddleIsGrown = false;


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
		SpawnBall ();
	}

	public void SpawnBall () {
		GameObject ball = Instantiate (ballPrefab);
		GameObject paddle = GameObject.FindGameObjectWithTag ("Paddle");
		paddle.GetComponent<PaddleControlScript> ().AttachBall (ball);
	}
	
	// Update is called once per frame
	void Update () {
		if (droppedBall) {
			HandleDroppedBall();
			droppedBall = false;
		}
		if (mustSpawnBall) {
			SpawnBall();
			mustSpawnBall = false;
		}
		if (destroyedBrick) {
			CheckForLevelClear();
			destroyedBrick = false;
		}
		if (gameOver && Input.GetKeyDown (KeyCode.Mouse0)) {
			Application.LoadLevel ("main_menu");
		}
	}

	private void HandleDroppedBall() {
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Ball");
		if (balls.Length == 0) {
			ResetPaddleSize();
			LoseLife();
		}
	}

	private void LoseLife() {
		livesRemaining--;
		if (livesRemaining > 0) {
			audioSource.PlayOneShot (ballLossSound);
			mustSpawnBall = true;
		} else {
			PauseGame();
			StartCoroutine(InitiateGameOver());
		}
	}

	private IEnumerator InitiateGameOver() {
		audioSource.PlayOneShot(gameOverSound);
		yield return new WaitForSeconds (gameOverSound.length);
		gameOverText.enabled = true;
		gameOver = true;
	}

	private void CheckForLevelClear() {
		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("Brick");
		if (bricks.Length == 0) {
			StartCoroutine(FinishLevel());
		}
	}

	private IEnumerator FinishLevel() {
		PauseGame ();
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

	private void PauseGame() {
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Ball");
		for (int i = 0; i < balls.Length; i++) {
			GameObject ball = balls[i];
			BallMovement script = ball.GetComponent<BallMovement>();
			if (script != null) {
				script.Pause();
			}
		}
	}
	
	public void NotifyBrickDestruction(GameObject brick) {
		PlayDestructionSound ();
		SpawnPowerup (brick.transform.position);
		destroyedBrick = true;
	}

	private void SpawnPowerup(Vector3 position) {
		float randomValue = Random.value;
		if (randomValue <= SPAWN_CHANCE) {
			GameObject powerup = CreateRandomPowerup();
			powerup.transform.position = position;
		}
	}

	private GameObject CreateRandomPowerup() {
		int randomIndex = Mathf.RoundToInt(Random.value * (powerups.Length - 1));
		return Instantiate(powerups [randomIndex]);
	}
		
	private void PlayDestructionSound() {
		audioSource.PlayOneShot (destructionSound);
	}

	public void NotifyBallLoss() {		
		droppedBall = true;
	}

	public void ApplyMultiplicationPowerup() {
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Ball");
		for (int i = 0; i < balls.Length; i++) {
			GameObject ball = balls[i];
			Vector2 originalVelocity = ball.GetComponent<Rigidbody2D>().velocity;
			GameObject clonedBallLeft = Instantiate(ball);
			clonedBallLeft.GetComponent<BallMovement>().SetVelocity(Quaternion.Euler(0, 0, 45) * originalVelocity);
			GameObject clonedBallRight = Instantiate(ball);
			clonedBallRight.GetComponent<BallMovement>().SetVelocity(Quaternion.Euler(0, 0, -45) * originalVelocity);
		}
	}

	public void ApplySlowPowerup() {
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Ball");
		for (int i = 0; i < balls.Length; i++) {
			GameObject ball = balls[i];
			ball.GetComponent<BallMovement>().ResetSpeed();
		}
	}

	public void ApplyGrowPowerup() {
		if (paddleIsShrunk) {
			ResetPaddleSize();
		} else {
			ReplacePaddleWithPrefab(largePaddlePrefab);
			paddleIsGrown = true;
		}
	}

	public void ApplyShrinkPowerup() {
		if (paddleIsGrown) {
			ResetPaddleSize();
		} else {
			ReplacePaddleWithPrefab(smallPaddlePrefab);
			paddleIsShrunk = true;
		}
	}

	private void ResetPaddleSize() {
		ReplacePaddleWithPrefab (normalPaddlePrefab);
		paddleIsGrown = false;	
		paddleIsShrunk = false;
	}

	private void ReplacePaddleWithPrefab(GameObject newPaddlePrefab) {
		GameObject currentPaddle = GameObject.FindGameObjectWithTag ("Paddle");
		GameObject newPaddle = Instantiate (newPaddlePrefab, currentPaddle.transform.position, Quaternion.identity) as GameObject;
		currentPaddle.SetActive (false);
		Destroy(currentPaddle);
	}

	public static void ResetLives() {
		livesRemaining = START_LIVES;
	}
}
