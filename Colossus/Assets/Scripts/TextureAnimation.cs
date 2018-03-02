using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Material))]

public class TextureAnimation : MonoBehaviour {

    public Material mat;

	// Use this for initialization
	void Start () {
        mat = GetComponent<Material>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(mat.mainTextureOffset);
        mat.mainTextureOffset += new Vector2(1f, 1f);
	}
}
