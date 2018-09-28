using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureOffsetScript : MonoBehaviour {
    private Material material;
    public Vector2 textureOffset;

    // Use this for initialization
    void Start()
    {
        material = GetComponent<Renderer>().material;

        // Generate a random offset in randomly the positive or negative x and y directions
        //textureOffset.x = Random.Range(0.1f, 0.4f) * ((Random.Range(1, 3) * 2) - 3);
        //textureOffset.y = Random.Range(0.1f, 0.4f) * ((Random.Range(1, 3) * 2) - 3);
        //texture = GetComponent<Renderer>().material.SetTextureOffset("lava_texture", new Vector2(1.0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += textureOffset * Time.deltaTime;
    }
}
