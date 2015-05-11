using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

	public float scrollSpeed = 1;
	private float height;
	
	private Vector3 startPosition;
	
	// Use this for initialization
	void Start () {
		height = GetComponent<SpriteRenderer>().sprite.bounds.size.y;	
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float newPosition = Mathf.Repeat (Time.time * scrollSpeed, height);  
		transform.position = startPosition + Vector3.down * newPosition;
	}
}
