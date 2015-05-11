using UnityEngine;
using System.Collections;

public class IndestructibleBrickScript : BrickScript {

	public AudioClip collisionSound;

	private AudioSource audioSource;

	void Start() {
		audioSource = GetComponent<AudioSource>();
	}

	protected override void handleBallCollision() {
		audioSource.PlayOneShot (collisionSound);
	}
}
