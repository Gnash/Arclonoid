using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	private Text livesRemainingText;

	// Use this for initialization
	void Start () {
		livesRemainingText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		livesRemainingText.text = "Lives: " + GameManager.livesRemaining;
	}
}
