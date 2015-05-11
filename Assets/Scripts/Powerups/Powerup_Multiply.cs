using UnityEngine;
using System.Collections;

public class Powerup_Multiply : Powerup {

	protected override void applyPowerupToGameManager() {
		GameManager.instance.applyMultiplicationPowerup ();
	}
}
