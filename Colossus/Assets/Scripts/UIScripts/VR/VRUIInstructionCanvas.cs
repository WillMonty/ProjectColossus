using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRUIInstructionCanvas : MonoBehaviour {
	//Singleton for AbilityToggles to access
	public static VRUIInstructionCanvas instance = null;

	public Text selectedText;
	public InstructionObject[] instructions;
	Dictionary<string, InstructionObject> instructionsDict;

	InstructionObject current;

	void Start()
	{
		#region Singleton Design Pattern
		// Check for an instance, if it doesn't exist, than set to this
		if (instance == null)
		{
			instance = this;
		}
		else if(instance!=this) // If a new Gamemanger is loaded and it isn't the one that is loaded already than delete it
		{
			if (FindObjectsOfType(GetType()).Length > 1)
				Destroy(gameObject);
		}
		#endregion

		instructionsDict = new Dictionary<string, InstructionObject>();
		for(int i = 0; i < instructions.Length; i++)
		{
			instructionsDict.Add(instructions[i].name, instructions[i]);
		}
	}

	public void AbilitySelect(string name)
	{
		InstructionObject foundInstruction;
		if(instructionsDict.TryGetValue(name, out foundInstruction))
		{
			if(current.text != null)
				current.text.SetActive(false);

			if(current.controllerImage != null)
				current.controllerImage.SetActive(false);
			
			current = foundInstruction;
			current.text.SetActive(true);
			current.controllerImage.SetActive(true);
			selectedText.text = name;
		}	
	}

}

[System.Serializable]
public struct InstructionObject {
	public string name;
	public GameObject text;
	public GameObject controllerImage;
}
