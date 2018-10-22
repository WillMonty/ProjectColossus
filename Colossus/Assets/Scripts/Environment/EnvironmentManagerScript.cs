using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManagerScript : MonoBehaviour {

	// Static instance of the EnvironmentManagerScript which allows it to be accessed from any script
	public static EnvironmentManagerScript instance = null;

	[Header("Map")]
	public GameObject map;
	public float lowerMapAmount;
	public GameObject resistanceContainer;

	[Header("Game Pieces")]
	public List<GameObject> gamePieces = new List<GameObject>(); //Pieces of the scene only shown/used InGame

	[Header("Trash")]
	public uint maxRubbishCount;
	static uint maxCount;
	static List<GameObject> rubbishList;

	[Header("Announcer")]
	public AudioSource sourceAnnouncer;
	public AnnouncerClip[] announcerClips;
	Dictionary<string, AnnouncerClip> clipDict;

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

		//Property Initialization
		rubbishList = new List<GameObject>();
		maxCount = maxRubbishCount;

		clipDict = new Dictionary<string, AnnouncerClip>();
		for(int i = 0; i < announcerClips.Length; i++)
		{
			clipDict.Add(announcerClips[i].name, announcerClips[i]);
		}
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

	/// <summary>
	/// Adds rubbish to a trash collector list
	/// </summary>
	/// <param name="rubbishObject">Object to add</param>
	public static void AddRubbishToList(GameObject rubbishObject)
	{
		rubbishList.Add(rubbishObject);

		if (rubbishList.Count > maxCount)
		{
			Destroy(rubbishList[0]);
			rubbishList.RemoveAt(0);
		}
	}

	/// <summary>
	/// Lowers the map according to the head height of the VR player
	/// </summary>
	/// <param name="headHeight">VR player y position</param>
	public void LowerMap(float headHeight)
	{
		float playerY = headHeight - lowerMapAmount;
		map.transform.position = new Vector3(map.transform.position.x, playerY, map.transform.position.z);

		//Don't bother if there aren't resistance in the scene
		if(resistanceContainer != null)
			resistanceContainer.transform.position = new Vector3(resistanceContainer.transform.position.x, playerY + resistanceContainer.transform.position.y, resistanceContainer.transform.position.z);
	}

	/// <summary>
	/// Plays an announcer clip by name if it exists
	/// </summary>
	/// <param name="name">Name of the clip to be played</param>
	public void PlayAnnouncer(string name)
	{
		sourceAnnouncer.Stop();

		AnnouncerClip found;

		if(clipDict.TryGetValue(name, out found))
		{
			//Make sure clip is valid to play
			if(!found.oneTimeUse || (found.oneTimeUse && !found.Played))
			{
				sourceAnnouncer.clip = found.clip;
				sourceAnnouncer.Play();
				found.Played = true;
			}
		}
		else
		{
			Debug.LogError("No announcer clip with that name");
		}
	}
}

[System.Serializable]
public struct AnnouncerClip {
	public string name;
	public AudioClip clip;
	public bool oneTimeUse;
	bool played;

	public bool Played {
		get
		{
			return played;
		}
		set
		{
			played = value;
		}
	}
}
