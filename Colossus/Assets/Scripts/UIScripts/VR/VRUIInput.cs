using UnityEngine;
using UnityEngine.UI;

//Based on this guide: https://unity3d.college/2017/06/17/steamvr-laser-pointer-menus/
public class VRUIInput : MonoBehaviour
{
	private SteamVR_LaserPointer laserPointer;
	private SteamVR_TrackedController trackedController;

	private VRUIToggle currToggle;

	private void OnEnable()
	{
		laserPointer = GetComponent<SteamVR_LaserPointer>();
		laserPointer.PointerIn -= HandlePointerIn;
		laserPointer.PointerIn += HandlePointerIn;
		laserPointer.PointerOut -= HandlePointerOut;
		laserPointer.PointerOut += HandlePointerOut;

		trackedController = GetComponent<SteamVR_TrackedController>();
		if (trackedController == null)
		{
			trackedController = GetComponentInParent<SteamVR_TrackedController>();
		}
		trackedController.TriggerClicked -= HandleTriggerClicked;
		trackedController.TriggerClicked += HandleTriggerClicked;
		trackedController.TriggerUnclicked -= HandleTriggerUnclicked;
		trackedController.TriggerUnclicked += HandleTriggerUnclicked;
	}

	private void HandleTriggerClicked(object sender, ClickedEventArgs e)
	{
		if (currToggle != null)
		{
			currToggle.ToggleClicked();
		}
	}

	private void HandleTriggerUnclicked(object sender, ClickedEventArgs e)
	{
		if (currToggle != null)
		{
			currToggle.ToggleRelease();
		}
	}

	private void HandlePointerIn(object sender, PointerEventArgs e)
	{
		VRUIToggle hovered = e.target.GetComponent<VRUIToggle>();
		if (hovered != null)
		{
			currToggle = hovered;
			currToggle.ToggleHover(true);
		}
	}

	private void HandlePointerOut(object sender, PointerEventArgs e)
	{
		VRUIToggle hovered = e.target.GetComponent<VRUIToggle>();
		if (hovered != null)
		{
			currToggle.ToggleHover(false);
			currToggle = null;
		}
	}
}