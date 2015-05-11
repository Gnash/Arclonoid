using UnityEngine;
using System.Collections;

public class BallLoss : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		Destroy(other.gameObject);
		GameManager.instance.notifyBallLoss ();
	}		                                               
}
