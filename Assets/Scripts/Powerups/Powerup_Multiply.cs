using UnityEngine;
using System.Collections;

public class Powerup_Multiply : Powerup {

	protected override void ApplyPowerupToGameManager() {
		GameManager.instance.ApplyMultiplicationPowerup ();
	}
}
