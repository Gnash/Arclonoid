using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void Update() {
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			GameManager.resetLives();
			Application.LoadLevel ("level_1");
		}
	}
}
