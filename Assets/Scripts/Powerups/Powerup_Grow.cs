using UnityEngine;
using System.Collections;

public class Powerup_Grow : Powerup {
	
	protected override void ApplyPowerupToGameManager() {
		GameManager.instance.ApplyGrowPowerup ();
	}
}
