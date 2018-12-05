using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
//Based on this guide: https://unity3d.college/2017/06/17/steamvr-laser-pointer-menus/
public class VRUIToggle : MonoBehaviour
{
	//Holds which type of ability this toggle will represent
	public enum ToggleTypes {Head, LeftHand, RightHand};
	public ToggleTypes toggleType;
	public Color onColor;
	public Color offColor;
	public Color pressColor;

	[HideInInspector]
	public ColossusHeadAbilities headAbility;
	[HideInInspector] 
	public ColossusHandAbilities leftHandAbility;
	[HideInInspector] 
	public ColossusHandAbilities rightHandAbility;

	private BoxCollider boxCollider;
	private RectTransform rectTransform;
	private Image image;
	private Toggle toggle;

	private void OnEnable()
	{
		Validate();
	}

	private void OnValidate()
	{
		Validate();
	}

	private void Validate()
	{
		image = GetComponent<Image>();
		toggle = GetComponent<Toggle>();
		rectTransform = GetComponent<RectTransform>();

		boxCollider = GetComponent<BoxCollider>();
		if (boxCollider == null)
		{
			boxCollider = gameObject.AddComponent<BoxCollider>();
		}

		boxCollider.size = GetComponent<RectTransform>().sizeDelta;
	}

	public void ToggleChanged(bool enabled)
	{
		if(!enabled)
		{
			image.color = offColor;
		}
	}


	public virtual void ToggleClicked()
	{
		toggle.isOn = !toggle.isOn;
		if(toggle.isOn)
		{
			//Set ability associated with this toggle
			AbilityManagerScript abilitiesInstance = AbilityManagerScript.instance;
			switch(toggleType)
			{
				case(ToggleTypes.Head):
					abilitiesInstance.SetColossusAbility(headAbility);
					break;
				case(ToggleTypes.LeftHand):
					abilitiesInstance.SetColossusAbility(leftHandAbility, true);
					break;
				case(ToggleTypes.RightHand):
					abilitiesInstance.SetColossusAbility(rightHandAbility, false);
					break;
			}
		}

		image.color = pressColor;

		//Enable appropriate colossus body part??
	}

	public void ToggleRelease()
	{
		if(toggle.isOn)
			image.color = onColor;
		else
			image.color = offColor;
	}

	public void ToggleHover(bool isIn)
	{
		if(isIn)
		{
			
		}
		else
		{
			ToggleRelease();
		}
	}
}

[CanEditMultipleObjects]
[CustomEditor(typeof(VRUIToggle))]
public class VRUIToggle_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		VRUIToggle script = (VRUIToggle)target;

		//Draw appropriate ability enum
		switch(script.toggleType)
		{
			case(VRUIToggle.ToggleTypes.Head):
				script.headAbility = (ColossusHeadAbilities)EditorGUILayout.EnumPopup("Head Ability:", script.headAbility);
				break;
			case(VRUIToggle.ToggleTypes.LeftHand):
				script.leftHandAbility = (ColossusHandAbilities)EditorGUILayout.EnumPopup("Left Hand Ability:", script.leftHandAbility);
				break;
			case(VRUIToggle.ToggleTypes.RightHand):
				script.rightHandAbility = (ColossusHandAbilities)EditorGUILayout.EnumPopup("Right Hand Ability:", script.rightHandAbility);
				break;
		}
	}
}
