using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollector : MonoBehaviour {

    public uint maxRubbishCount;
    private static uint maxCount;
    private static List<GameObject> rubbishList;
    
	// Use this for initialization
	void Start () {
        rubbishList = new List<GameObject>();
        maxCount = maxRubbishCount;
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
