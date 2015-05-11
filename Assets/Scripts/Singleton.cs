using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour {

	private static Singleton instance = null;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} else if (this != instance) {
			Destroy(this);
		}
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
