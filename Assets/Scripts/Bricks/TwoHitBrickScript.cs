using UnityEngine;
using System.Collections;

public class TwoHitBrickScript : BrickScript {

	public Sprite damagedSprite;
	public AudioClip damageSound;

	private uint hitpoints = 2;

	private AudioSource audioSource;
	private SpriteRenderer spriteRenderer;


	void Start() {
		audioSource = GetComponent<AudioSource> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	protected override void handleBallCollision() {
		hitpoints--;
		if (hitpoints > 0) {
			audioSource.PlayOneShot(damageSound);
			spriteRenderer.sprite = damagedSprite;
		} else {
			base.handleBallCollision();
		}
	}

}
