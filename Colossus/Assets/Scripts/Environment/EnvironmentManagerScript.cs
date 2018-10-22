using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManagerScript : MonoBehaviour {

	// Static instance of the EnvironmentManagerScript which allows it to be accessed from any script
	public static EnvironmentManagerScript instance = null;

	[Header("Game Pieces")]
	public List<GameObject> gamePieces = new List<GameObject>(); //Pieces of the scene only shown/used InGame

	[Header("Trash")]
	public uint maxRubbishCount;
	static uint maxCount;
	static List<GameObject> rubbishList;

	// Use this for initialization
	void Start () 
	{
		#region Singleton Design Pattern
		// Check for an instance, if it does exist, than set to this
		if (instance == null)
		{
			instance = this;
		}
		else if(instance != this) // If a new Gamemanger is loaded and it isn't the one that is loaded already than delete it
		{
			if (FindObjectsOfType(GetType()).Length > 1)
				Destroy(gameObject);
		}
		#endregion

		rubbishList = new List<GameObject>();
		maxCount = maxRubbishCount;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Turns objects present in the gamePieces array on or off
	/// </summary>
	public void GamePiecesSwitch()
	{
		for(int i = 0; i < gamePieces.Count; i++)
		{
			if(gamePieces[i] != null)
			{
				gamePieces[i].SetActive(!gamePieces[i].gameObject.activeSelf);
			}
		}
	}

	public static void AddRubbishToList(GameObject rubbishObject)
	{
		rubbishList.Add(rubbishObject);

		if (rubbishList.Count > maxCount)
		{
			Destroy(rubbishList[0]);
			rubbishList.RemoveAt(0);
		}
	}
}
