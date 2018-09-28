using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for any type of Colossus ability.
/// </summary>
public abstract class ColossusAbility : MonoBehaviour {

	protected bool abilityEnabled;

	protected SteamVR_TrackedController leftControllerTracked;
	protected SteamVR_TrackedController rightControllerTracked;

	public abstract void Enable();
	public abstract void Disable();

	public void SetupTrackedControllers()
	{
		leftControllerTracked = this.GetComponent<SteamVR_ControllerManager>().left.GetComponent<SteamVR_TrackedController>();
		rightControllerTracked = this.GetComponent<SteamVR_ControllerManager>().right.GetComponent<SteamVR_TrackedController>();
	}
}
