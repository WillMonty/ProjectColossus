using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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

	public string nameForInstruction;

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
					break;
				case(AbilityTypes.LeftHand):
					abilitiesInstance.SetColossusAbility(leftHandAbility, true);
					break;
				case(AbilityTypes.RightHand):
					abilitiesInstance.SetColossusAbility(rightHandAbility, false);
					break;
			}

			if(VRUIInstructionCanvas.instance != null)
				VRUIInstructionCanvas.instance.AbilitySelect(nameForInstruction);
		}
		else
		{
			image.color = offColor;
		}

		//Enable appropriate colossus body part??
	}
}

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
