using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColossusAbility : MonoBehaviour {

	protected bool enabled;

	protected SteamVR_TrackedController leftControllerTracked;
	protected SteamVR_TrackedController rightControllerTracked;

	public abstract void Enable();
	public abstract void Disable();
}
