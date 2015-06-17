using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour {

	private bool ballStuckInBrick = false;

	void OnCollisionEnter2D(Collision2D collision) {
		if (!ballStuckInBrick) {
			HandleBallCollision ();
		}
		ballStuckInBrick = true;
	}

	void OnCollisionExit2D(Collision2D collision) {
		ballStuckInBrick = false;
	}

	protected virtual void HandleBallCollision() {		
		GameManager.instance.NotifyBrickDestruction (gameObject);
		Destroy (gameObject);
	}
}
