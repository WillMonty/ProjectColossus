using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for any type of Colossus ability.
/// </summary>
public abstract class ColossusAbility : MonoBehaviour {

	protected bool abilityEnabled;

	public static  SteamVR_TrackedController leftControllerTracked;
	public static  SteamVR_TrackedController rightControllerTracked;

	public abstract void Enable();
	public abstract void Disable();
}