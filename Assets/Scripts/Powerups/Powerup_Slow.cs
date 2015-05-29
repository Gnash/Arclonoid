using UnityEngine;
using System.Collections;

public class Powerup_Slow : Powerup {

	protected override void applyPowerupToGameManager() {
		GameManager.instance.applySlowPowerup ();
	}
}
