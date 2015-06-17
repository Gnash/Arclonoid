using UnityEngine;
using System.Collections;

public class Powerup_Shrink : Powerup {
	
	protected override void ApplyPowerupToGameManager() {
		GameManager.instance.ApplyShrinkPowerup ();
	}
}
