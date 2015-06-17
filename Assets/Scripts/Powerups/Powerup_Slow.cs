using UnityEngine;
using System.Collections;

public class Powerup_Slow : Powerup {

	protected override void ApplyPowerupToGameManager() {
		GameManager.instance.ApplySlowPowerup ();
	}
}
