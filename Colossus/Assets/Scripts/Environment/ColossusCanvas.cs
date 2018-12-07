using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossusCanvas : MonoBehaviour {

	public GameObject timer;
	public CanvasPiece[] pieces;
	Dictionary<string, CanvasPiece> pieceDict;
	CanvasPiece current;


	void Start ()
	{
		pieceDict = new Dictionary<string, CanvasPiece>();
		for(int i = 0; i < pieces.Length; i++)
		{
			pieceDict.Add(pieces[i].name, pieces[i]);
		}
	}

	/// <summary>
	/// Shows a canvas piece by name if it exists
	/// </summary>
	/// <param name="name">Name of the canvas piece to be shown</param>
	public void SetCanvas(string name)
	{
		CanvasPiece found; 

		if(pieceDict.TryGetValue(name, out found))
		{
			//Make sure this display is valid to use
			if(!found.oneTimeUse || (found.oneTimeUse && !found.Shown))
			{
				if(current.piece != null)
					current.piece.SetActive(false);
				
				found.piece.SetActive(true);
				found.Shown = true;
				current = found;
			}
		}
		else
		{
			Debug.LogError("No canvas piece named " + name);
		}
	}

	public void Clear()
	{
		if(current.piece != null)
		{
			current.piece.SetActive(false);
		}
	}
}

[System.Serializable]
public struct CanvasPiece {
	public string name;
	public GameObject piece;
	public bool oneTimeUse;
	bool shown;

	public bool Shown {
		get
		{
			return shown;
		}
		set
		{
			shown = value;
		}
	}
}
