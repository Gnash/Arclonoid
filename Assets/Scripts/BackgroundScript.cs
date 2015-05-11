using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {

	public Sprite[] backgroundImages;

	// Use this for initialization
	void Start () {
		int imageCount = backgroundImages.Length;
		int selectedImageIndex = Mathf.RoundToInt(Random.value * (imageCount - 1));

		GetComponent<SpriteRenderer> ().sprite = backgroundImages [selectedImageIndex];
	}
}
