using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VRUIToggleAbility : VRUIToggle {


	//Holds which type of ability this toggle will represent
	public enum AbilityTypes {Head, LeftHand, RightHand};
	[Header("Ability Info")]
	public AbilityTypes abilityType;

	[HideInInspector]
	public ColossusHeadAbilities headAbility;
	[HideInInspector] 
	public ColossusHandAbilities leftHandAbility;
	[HideInInspector] 
	public ColossusHandAbilities rightHandAbility;

	public override void ToggleChanged(bool enabled)
	{
		if(enabled)
		{
			//Set ability associated with this toggle
			AbilityManagerScript abilitiesInstance = AbilityManagerScript.instance;
			switch(abilityType)
			{
				case(AbilityTypes.Head):
					abilitiesInstance.SetColossusAbility(headAbility);
					VRUIInstructionCanvas.instance.AbilitySelect(headAbility.ToString());
					break;
				case(AbilityTypes.LeftHand):
					abilitiesInstance.SetColossusAbility(leftHandAbility, true);
					VRUIInstructionCanvas.instance.AbilitySelect(leftHandAbility.ToString());
					break;
				case(AbilityTypes.RightHand):
					abilitiesInstance.SetColossusAbility(rightHandAbility, false);
					VRUIInstructionCanvas.instance.AbilitySelect(rightHandAbility.ToString());
					break;
			}
		}
		else
		{
			image.color = offColor;
		}

		//Enable appropriate colossus body part??
	}
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(VRUIToggleAbility))]
public class VRUIToggleAbility_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		VRUIToggleAbility script = (VRUIToggleAbility)target;

		//Draw appropriate ability enum
		switch(script.abilityType)
		{
			case(VRUIToggleAbility.AbilityTypes.Head):
				script.headAbility = (ColossusHeadAbilities)EditorGUILayout.EnumPopup("Head Ability:", script.headAbility);
				break;
			case(VRUIToggleAbility.AbilityTypes.LeftHand):
				script.leftHandAbility = (ColossusHandAbilities)EditorGUILayout.EnumPopup("Left Hand Ability:", script.leftHandAbility);
				break;
			case(VRUIToggleAbility.AbilityTypes.RightHand):
				script.rightHandAbility = (ColossusHandAbilities)EditorGUILayout.EnumPopup("Right Hand Ability:", script.rightHandAbility);
				break;
		}
	}
}
#endif
