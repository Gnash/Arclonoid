using UnityEngine;
using System.Collections;

public class Powerup_Shrink : Powerup {
	
	protected override void applyPowerupToGameManager() {
		GameManager.instance.ApplyShrinkPowerup ();
	}
}
