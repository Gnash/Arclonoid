using UnityEngine;
using System.Collections;

public class Powerup_Grow : Powerup {
	
	protected override void applyPowerupToGameManager() {
		GameManager.instance.ApplyGrowPowerup ();
	}
}
