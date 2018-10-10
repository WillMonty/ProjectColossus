using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManagerScript : MonoBehaviour {

	[Header("Game Pieces")]
	public List<GameObject> gamePieces = new List<GameObject>(); //Pieces of the scene only shown/used InGame

	[Header("Trash")]
	public uint maxRubbishCount;
	static uint maxCount;
	static List<GameObject> rubbishList;

	// Use this for initialization
	void Start () 
	{
		rubbishList = new List<GameObject>();
		maxCount = maxRubbishCount;
		GamePiecesSwitch();
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
